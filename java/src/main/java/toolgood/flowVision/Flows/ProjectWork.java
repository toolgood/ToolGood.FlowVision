package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Common.CompressionUtil;
import toolgood.flowVision.Common.RCY;
import toolgood.flowVision.Common.RsaUtil;

import java.io.*;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

public class ProjectWork {
    public String Name;
    public String Code;

    public int ExcelIndex;
    public boolean NumberRequired;

    public Map<String, String> FormulaList;
    public Map<String, FactoryWork> FactoryList;
    public Map<String, FactoryMachineWork> FactoryMachineList;
    public Map<String, FactoryProcedureWork> FactoryProcedureList;
    public Map<String, AppWork> AppList;
    private final Map<String, mathParser.ProgContext> ProgList = new HashMap<>();

    public mathParser.ProgContext CreateProgContext(String exp) throws Exception {
        if (ProgList.containsKey(exp)) {
            return ProgList.get(exp);
        }
        if (exp == null || exp.equals("")) {
            return null;
        }
        AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        mathLexer lexer = new mathLexer(stream);
        CommonTokenStream tokens = new CommonTokenStream(lexer);
        mathParser parser = new mathParser(tokens);
        AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);

        mathParser.ProgContext context = parser.prog();
        if (antlrErrorListener.IsError) {
            throw new Exception(antlrErrorListener.ErrorMsg);
        }
        ProgList.put(exp, context);
        return context;
    }

    public boolean TryGetFormula(String name, mathParser.ProgContext context) throws Exception {
        if (FormulaList.containsKey(name)) {
            context = CreateProgContext(FormulaList.get(name));
            return true;
        }
        context = null;
        return false;
    }

    public boolean HasFormula(String name) {
        return FormulaList.containsKey(name);
    }

    public static ProjectWork ParseJson(String json) throws Exception {
        JSONObject jsonObject = JSONObject.parseObject(json);
        ProjectWork work = ProjectWork.parse(jsonObject);
        for (FactoryWork item : work.FactoryList.values()) {
            item.Init(work);
        }
        for (FactoryMachineWork item : work.FactoryMachineList.values()) {
            item.Init(work);
        }
        for (FactoryProcedureWork item : work.FactoryProcedureList.values()) {
            item.Init(work);
        }
        for (AppWork item : work.AppList.values()) {
            item.Init(work);
        }
        return work;
    }

    static ProjectWork parse(JSONObject jsonObject) throws Exception {
        ProjectWork projectWork = new ProjectWork();
        projectWork.Name = jsonObject.getString("name");
        projectWork.Code = jsonObject.getString("code");
        projectWork.ExcelIndex = jsonObject.getIntValue("excelIndex");
        projectWork.NumberRequired = jsonObject.getBooleanValue("numberRequired");

        projectWork.FormulaList = new HashMap<>();
        if (jsonObject.containsKey("formulaList")) {
            JSONObject formulaList = (JSONObject) jsonObject.get("formulaList");
            for (Map.Entry<String, Object> item : formulaList.entrySet()) {
                projectWork.FormulaList.put(item.getKey(), item.getValue().toString());
            }
        }

        projectWork.FactoryList = new HashMap<>();
        if (jsonObject.containsKey("factoryList")) {
            JSONObject formulaList = (JSONObject) jsonObject.get("factoryList");
            for (Map.Entry<String, Object> item : formulaList.entrySet()) {
                if (item.getValue() instanceof JSONObject jsonObject1) {
                    FactoryWork work = FactoryWork.parse(jsonObject1);
                    if (work != null) {
                        projectWork.FactoryList.put(item.getKey(), work);
                    }
                }
            }
        }
        projectWork.FactoryMachineList = new HashMap<>();
        if (jsonObject.containsKey("factoryMachineList")) {
            JSONObject formulaList = (JSONObject) jsonObject.get("factoryMachineList");
            for (Map.Entry<String, Object> item : formulaList.entrySet()) {
                if (item.getValue() instanceof JSONObject jsonObject1) {
                    FactoryMachineWork work = FactoryMachineWork.parse(jsonObject1);
                    if (work != null) {
                        projectWork.FactoryMachineList.put(item.getKey(), work);
                    }
                }
            }
        }
        projectWork.FactoryProcedureList = new HashMap<>();
        if (jsonObject.containsKey("factoryProcedureList")) {
            JSONObject formulaList = (JSONObject) jsonObject.get("factoryProcedureList");
            for (Map.Entry<String, Object> item : formulaList.entrySet()) {
                if (item.getValue() instanceof JSONObject jsonObject1) {
                    FactoryProcedureWork work = FactoryProcedureWork.parse(jsonObject1);
                    if (work != null) {
                        projectWork.FactoryProcedureList.put(item.getKey(), work);
                    }
                }
            }
        }

        projectWork.AppList = new HashMap<>();
        if (jsonObject.containsKey("appList")) {
            JSONObject formulaList = (JSONObject) jsonObject.get("appList");
            for (Map.Entry<String, Object> item : formulaList.entrySet()) {
                if (item.getValue() instanceof JSONObject jsonObject1) {
                    AppWork work = AppWork.parse(jsonObject1);
                    if (work != null) {
                        projectWork.AppList.put(item.getKey(), work);
                    }
                }
            }
        }
        return projectWork;
    }

    public static ProjectWork LoadJson(String filename) throws Exception {
        File file = new File(filename);

        FileReader fileReader = new FileReader(file);
        BufferedReader reader = new BufferedReader(fileReader);
        StringBuilder stringBuilder = new StringBuilder();
        String line;
        while ((line = reader.readLine()) != null) {
            stringBuilder.append(line);
        }
        reader.close();
        String json = stringBuilder.toString();
        return ParseJson(json);
    }


    public static ProjectWork LoadJsonUsedRsa(String filename) throws Exception {
        String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAu3W3xI6mH9tr3A+sNZVhyIbQWFhePbPWdFeTtM39yR7kO4Akp6Dsb1NYKpKxSGjIwDv1TC6/IUwOgOYYSVa0pgfIujHPrYFO/LlDk6kPAyHluLimKFkHkze5FsY7YAqd2mExqdJ4Zfb1pXgIrVFgOs4o69p9vyBV6kWS0FBOnyyUK92bRYxeqS1raRfM3GUlIEaQW5ZIuJxQtFrfwSnsfDVhkp8rvFVt7I5aqawWeoJZu+/HZqQO/gz+BJ7ntlUWoPgfe13/U3kIOHMTc/Deczb5x3DeBv9XrwJ5+DrzrJV8jTdhiYeNcBNBYaKoHGS15chxt6no4sIDZYsI2N4ciQIDAQAB";

        FileChannel channel = null;
        FileInputStream fs = null;
        try {
            File f = new File(filename);
            fs = new FileInputStream(f);
            channel = fs.getChannel();
            ByteBuffer byteBuffer = ByteBuffer.allocate(4);
            channel.read(byteBuffer);
            int len = byteArrayToInt(byteBuffer);

            byteBuffer = ByteBuffer.allocate(len);
            for (int i = 0; i < len / 4; i++)
                channel.read(byteBuffer);
            byte[] bs = byteBuffer.array();
            bs = RsaUtil.decryptData(bs, publicKey);

            byteBuffer = ByteBuffer.allocate(4);
            channel.read(byteBuffer);
            len = byteArrayToInt(byteBuffer);

            byteBuffer = ByteBuffer.allocate(len);
            while (channel.read(byteBuffer) > 0) {
            }
            byte[] bs2 = byteBuffer.array();

            bs2 = RCY.Encrypt(bs2, bs);
            bs2 = CompressionUtil.BrDecompress(bs2);

            String json = new String(bs2, StandardCharsets.UTF_8);
            return ParseJson(json);
        } finally {
            try {
                channel.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
            try {
                fs.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }

    public static int byteArrayToInt(ByteBuffer byteBuffer) {
        byte[] bytes = byteBuffer.array();
        int res = ((bytes[3] & 0xff) << 24) + ((bytes[2] & 0xff) << 16) + ((bytes[1] & 0xff) << 8) + (bytes[0] & 0xff);
        return res;
    }


}
