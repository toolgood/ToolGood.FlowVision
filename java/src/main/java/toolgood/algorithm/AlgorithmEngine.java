package toolgood.algorithm;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.antlr.v4.runtime.RecognitionException;
import org.joda.time.DateTime;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.algorithm.math.mathParser.ProgContext;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;
import java.util.TreeMap;

public class AlgorithmEngine {
    /**
     * 是否忽略大小写
     */
    public final boolean IgnoreCase;
    private final Map<String, Operand> _tempdict;
    /**
     * 使用EXCEL索引
     */
    public boolean UseExcelIndex = true;
    /**
     * 最后一个错误
     */
    public String LastError;
    /**
     * 保存到临时文档
     */
    public boolean UseTempDict = false;
    private ProgContext _context;

    /// <summary>
    /// 默认不带缓存
    /// </summary>
    public AlgorithmEngine() {
        IgnoreCase = false;
        _tempdict = new TreeMap<>();
    }

    /// <summary>
    /// 带缓存关键字大小写参数
    /// </summary>
    /// <param name="ignoreCase"></param>
    public AlgorithmEngine(boolean ignoreCase) {
        IgnoreCase = ignoreCase;
        if (ignoreCase) {
            _tempdict = new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        } else {
            _tempdict = new TreeMap<>();
        }
    }

    private Operand GetDiyParameterInside(String parameter) {
        if (_tempdict.containsKey(parameter)) {
            return _tempdict.get(parameter);
        }
        Operand result = GetParameter(parameter);
        if (UseTempDict) {
            _tempdict.put(parameter, result);
        }
        return result;
    }

    protected Operand GetParameter(final String parameter) {
        return Operand.Error("Parameter [" + parameter + "] is missing.");
    }

    public void ClearParameters() {
        _tempdict.clear();
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final Operand obj) {
        _tempdict.put(key, obj);
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final boolean obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final short obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final int obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final long obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final float obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final double obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final BigDecimal obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final String obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final MyDate obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameter(final String key, final List<Operand> obj) {
        _tempdict.put(key, Operand.Create(obj));
    }

    /**
     * 添加自定义参数
     */
    public void AddParameterFromJson(final String json) throws Exception {
        if (json.startsWith("{") && json.endsWith("}")) {
            final JsonData jo = JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String item : jo.inst_object.keySet()) {
                    final JsonData v = jo.inst_object.get(item);
                    if (v.IsString())
                        _tempdict.put(item, Operand.Create(v.StringValue()));
                    else if (v.IsBoolean())
                        _tempdict.put(item, Operand.Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        _tempdict.put(item, Operand.Create(v.NumberValue()));
                    else if (v.IsObject())
                        _tempdict.put(item, Operand.Create(v));
                    else if (v.IsArray())
                        _tempdict.put(item, Operand.Create(v));
                    else if (v.IsNull())
                        _tempdict.put(item, Operand.CreateNull());
                }
                return;
            }
        }
        throw new Exception("Parameter is not json String.");
    }

    public boolean Parse(final String exp) throws RecognitionException {
        if (exp == null || exp.equals("")) {
            LastError = "Parameter exp invalid !";
            return false;
        }
        final AntlrCharStream stream = new AntlrCharStream(CharStreams.fromString(exp));
        final mathLexer lexer = new mathLexer(stream);
        final CommonTokenStream tokens = new CommonTokenStream(lexer);
        final mathParser parser = new mathParser(tokens);
        final AntlrErrorListener antlrErrorListener = new AntlrErrorListener();
        parser.removeErrorListeners();
        parser.addErrorListener(antlrErrorListener);
        final ProgContext context = parser.prog();

        if (antlrErrorListener.IsError) {
            _context = null;
            LastError = antlrErrorListener.ErrorMsg;
            return false;
        }
        _context = context;
        return true;
    }

    public Operand Evaluate() throws Exception {
        if (_context == null) {
            LastError = "Please use Parse to compile formula !";
            throw new Exception("Please use Parse to compile formula !");
        }
        final MathVisitor visitor = new MathVisitor();
        visitor.GetParameter = f -> {
            try {
                return GetDiyParameterInside(f);
            } catch (Exception ignored) {
            }
            return null;
        };
        visitor.excelIndex = UseExcelIndex ? 1 : 0;
        return visitor.visit(_context);
    }

    public int TryEvaluate(final String exp, final int defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.IntValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public double TryEvaluate(final String exp, final double defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToNumber("It can't be converted to number!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.NumberValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public String TryEvaluate(final String exp, final String defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                if (obj.IsNull()) {
                    return null;
                }
                obj = obj.ToText("It can't be converted to String!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.TextValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public boolean TryEvaluate(final String exp, final boolean defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToBoolean("It can't be converted to boolean!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.BooleanValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public DateTime TryEvaluate(final String exp, final DateTime defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to DateTime!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.DateValue().ToDateTime();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }

    public MyDate TryEvaluate(final String exp, final MyDate defvalue) {
        try {
            if (Parse(exp)) {
                Operand obj = Evaluate();
                obj = obj.ToDate("It can't be converted to MyDate!");
                if (obj.IsError()) {
                    LastError = obj.ErrorMsg();
                    return defvalue;
                }
                return obj.DateValue();
            }
        } catch (final Exception ex) {
            LastError = ex.getMessage();
        }
        return defvalue;
    }


}