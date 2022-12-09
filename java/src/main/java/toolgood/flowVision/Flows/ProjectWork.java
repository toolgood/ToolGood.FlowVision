package toolgood.flowVision.Flows;

import com.alibaba.fastjson2.JSONObject;
import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;

import java.util.HashMap;
import java.util.Map;

public class ProjectWork implements IProjectWork {
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
        if (FormulaList.containsKey(name)) {
            return true;
        }
        return false;
    }

    public static ProjectWork LoadJson(String json) throws Exception {
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
        projectWork.Name=jsonObject.getString("name");
        projectWork.Code=jsonObject.getString("code");
        projectWork.ExcelIndex=jsonObject.getIntValue("excelIndex");
        projectWork.NumberRequired=jsonObject.getBooleanValue("numberRequired");

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
        return  projectWork;
    }


}
