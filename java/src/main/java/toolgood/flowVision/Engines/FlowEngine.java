package toolgood.flowVision.Engines;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.joda.time.DateTime;
import toolgood.algorithm.Operand;
import toolgood.algorithm.Enums.OperandType;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.OutDatas.TreeNode;
import toolgood.flowVision.Engines.Parameters.Setting_Machine;
import toolgood.flowVision.Flows.CustomFlowWork;
import toolgood.flowVision.Flows.Enums.InputType;
import toolgood.flowVision.Flows.Interfaces.IInputNameNodeWork;
import toolgood.flowVision.Flows.ProjectWork;
import toolgood.flowVision.Flows.SettingFormulaWork;

import java.util.*;

public final class FlowEngine {
    private ProjectWork Project;
    private Map<String, Setting_Machine> MachineSetting;
    private String AttachData;
    private TreeNode Start;
    private toolgood.flowVision.Flows.AppWork AppWork;
    private Map<String, SettingFormulaWork> _startFormulaDict = new HashMap<String, SettingFormulaWork>();//用于默认
    private Map<String, SettingFormulaWork> _progDict = new HashMap<String, SettingFormulaWork>();//用于临时
    private Map<String, TreeNode> _outputNumDict = new HashMap<String, TreeNode>();//用于保存出量来源

    private Map<String, TreeNode> _inputNumDict = new TreeMap<>();
    private Map<String, CustomFlowWork> _scriptDict = new HashMap<String, CustomFlowWork>();// 脚本

    private Map<String, Operand> _tempdict = new HashMap<String, Operand>();// 临时变量
    private Map<String, Operand> _initDict = new HashMap<String, Operand>();//保存 初始值
    private Map<String, Operand> _inputDict = new HashMap<String, Operand>();//保存 输入项

    public Set<String> _tempNames;
    public Map<String, Integer> _tempdictCount;//用于删除临时变量

    public FlowEngine(ProjectWork project) {
        this(project, null, null);
    }

    public FlowEngine(ProjectWork project, String attachData) {
        this(project, attachData, null);
    }

    public FlowEngine(ProjectWork project, List<Setting_Machine> machines) {
        this(project, null, machines);
    }

    public FlowEngine(ProjectWork project, String attachData, List<Setting_Machine> machines) {
        Project = project;
        AttachData = attachData;
        if (machines != null) {
            MachineSetting = new HashMap<>();
            for (int i = 0; i < machines.size(); i++) {
                Setting_Machine machine = machines.get(i);
                MachineSetting.put(machine.CategoryCode + '|' + machine.Code, machine);
            }
        }
    }


    public void EvaluateInputNum() throws Exception {
        List<String> keys = _inputNumDict.keySet().stream().toList();

        for (int i = keys.size() - 1; i >= 0; i--) {
            String key = keys.get(i);
            TreeNode treeNode = _inputNumDict.get(key);
            treeNode.EvaluateInputNum(this);
            if (treeNode.CurrWork instanceof IInputNameNodeWork work) {
                if (work.InputName() != null && work.InputName().equals("") == false) {
                    _tempdict.put(work.InputName(),Operand.Create(treeNode.InputNum.doubleValue()));
                }
            }
        }
        Start.EvaluateInputNum(this);
    }

    public Operand GetNum() {
        return _inputDict.get("数量");
    }

    private Stack<Operand> _tempOutputs = new Stack<Operand>();

    public void SetOutputNum(Operand operand) {
        if (_tempdict.containsKey("出量")) {
            Operand op = _tempdict.get("出量");
            _tempOutputs.push(op);
        }
        _tempdict.put("出量", operand);
    }

    public void ClearOutputNum() {
        if (_tempOutputs.empty() == false) {
            Operand operand = _tempOutputs.pop();
            _tempdict.put("出量", operand);
        } else {
            _tempdict.put("出量", _inputDict.get("数量"));
        }
    }

    public Operand GetOutputNum() {
        if (_tempdict.containsKey("出量")) {
            return _tempdict.get("出量");
        }
        return _inputDict.get("数量");
    }

    public double TryEvaluate(String exp, double def) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToNumber();
            if (obj.IsError()) {
                return def;
            }
            return obj.NumberValue();
        } catch (Exception ex) {
        }
        return def;
    }

    public boolean TryEvaluate(String exp, boolean def) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToBoolean();
            if (obj.IsError()) {
                return def;
            }
            return obj.BooleanValue();
        } catch (Exception ex) {
        }
        return def;
    }

    public DateTime TryEvaluate(String exp, DateTime def) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToBoolean();
            if (obj.IsError()) {
                return def;
            }
            return obj.DateValue().ToDateTime();
        } catch (Exception ex) {
        }
        return def;
    }

    public String TryEvaluate(String exp, String def) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToText();
            if (obj.IsError()) {
                return def;
            }
            return obj.TextValue();
        } catch (Exception ex) {
        }
        return def;
    }

    public Operand Evaluate(String exp) {
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

        var context = parser.prog();
        parser = null;
        tokens = null;
        lexer = null;
        stream = null;
        if (antlrErrorListener.IsError) {
            antlrErrorListener = null;
            return null;
        }
        antlrErrorListener = null;

        MathVisitor visitor = new MathVisitor();
        visitor.GetParameter = f -> {
            try {
                return GetTempParameter(f);
            } catch (Exception e) {
            }
            return null;
        };
        visitor.excelIndex = Project.ExcelIndex;
        var result = visitor.visit(context);
        visitor = null;
        return result;
    }

    public boolean TryEvaluate(mathParser.ProgContext exp, boolean def) {
        try {
            Operand obj = Evaluate(exp);
            obj = obj.ToBoolean();
            if (obj.IsError()) {
                return def;
            }
            return obj.BooleanValue();
        } catch (Exception ex) {
        }
        return def;
    }

    public Operand Evaluate(mathParser.ProgContext context) {
        var visitor = new MathVisitor();
        visitor.GetParameter = f -> {
            try {
                return GetTempParameter(f);
            } catch (Exception e) {
            }
            return null;
        };
        visitor.excelIndex = Project.ExcelIndex;
        var result = visitor.visit(context);
        visitor = null;
        return result;
    }


    public boolean CheckMachine(String categoryCode, String code) {
        if (MachineSetting == null || MachineSetting.size() == 0) {
            return true;
        }
        String key = categoryCode + '|' + code;
        if (MachineSetting.containsKey(key)) {
            Setting_Machine machine = MachineSetting.get(key);
            if (machine.IsStop) {
                return false;
            }
        }
        return true;
    }

    public Operand EvaluateFormula(mathParser.ProgContext progContext, InputType inputType) {
        Operand operand = Evaluate(progContext);
        if (inputType == InputType.Bool) {
            return operand.ToBoolean();
        } else if (inputType == InputType.Number) {
            return operand.ToNumber();
        } else if (inputType == InputType.String) {
            return operand.ToText();
        } else if (inputType == InputType.List) {
            return operand.ToArray();
        } else if (inputType == InputType.Date) {
            return operand.ToDate();
        }
        return null;
    }

    public Operand GetTempParameter(String parameter) throws Exception {
        if (_initDict.containsKey(parameter)) {
            return _initDict.get(parameter);
        }
        if (_inputDict.containsKey(parameter)) {
            return _inputDict.get(parameter);
        }
        if (_tempdict.containsKey(parameter)) {
            return _tempdict.get(parameter);
        }
        if (_progDict.containsKey(parameter)) {
            SettingFormulaWork work = _progDict.get(parameter);
            Operand result;
            if (_outputNumDict.containsKey(parameter)) {
                TreeNode treeNode = _outputNumDict.get(parameter);
                SetOutputNum(Operand.Create(treeNode.OutputNum)); //设置出量
                result = work.EvaluateFormula(this);
                _tempdict.put(parameter, result);
                ClearOutputNum();// 清除出量
            } else {
                result = work.EvaluateFormula(this);
                _tempdict.put(parameter, result);
            }
            return result;
        }
        mathParser.ProgContext context = null;
        if (Project.TryGetFormula(parameter, context)) { // 项目公式
            Operand result = Evaluate(context);
            _tempdict.put(parameter, result);
            return result;
        }
        if (_scriptDict.containsKey(parameter)) {
            CustomFlowWork script = _scriptDict.get(parameter);
            try {
                if (_outputNumDict.containsKey(parameter)) {
                    TreeNode treeNode = _outputNumDict.get(parameter);
                    SetOutputNum(Operand.Create(treeNode.OutputNum)); //设置出量
                    EvaluateJs(script.Script); //脚本运行后会保存到临时变量内
                    ClearOutputNum();// 清除出量
                } else {
                    EvaluateJs(script.Script); //脚本运行后会保存到临时变量内
                }
                if (_tempdict.containsKey(parameter)) {
                    return _tempdict.get(parameter);
                }
            } catch (Exception ex) {
                return Operand.Error("[" + script.Label + "]脚本:[" + parameter + "]" + ex.getMessage());
            }
        }
        if (_startFormulaDict.containsKey(parameter)) {
            SettingFormulaWork startWork = _startFormulaDict.get(parameter);
            Operand result = startWork.EvaluateFormula(this);
            _tempdict.put(parameter, result);
            return result;
        }
        return Operand.Error(parameter + "的公式未找到！");
    }

    private void EvaluateJs(String script) {
//        var engine = new Engine()
//                .SetValue("getDatas", new Func<object>(js_getDatas))
//                .SetValue("hasKey", new Func<string, bool>(js_hasKey))
//                .SetValue("setValue", new Action<string, object>(js_setValue))
//                .SetValue("getValue", new Func<string, object>(js_getValue))
//                .SetValue("error", new Action<string>(js_Error));
//
//        engine.Evaluate(script);
////        engine.Dispose();
//        engine = null;
    }

    private void js_Error(String msg) throws Exception {
        throw new Exception(msg);
    }

    private String js_getDatas() {
        return AttachData;
    }

    private void js_setValue(String name, Object value) throws Exception {
        if (value == null) {
            _tempdict.put(name, Operand.CreateNull());
        } else if (value instanceof Integer numInt) {
            _tempdict.put(name, Operand.Create(numInt));
        } else if (value instanceof Double numDouble) {
            _tempdict.put(name, Operand.Create(numDouble));
        } else if (value instanceof Boolean numBool) {
            _tempdict.put(name, Operand.Create(numBool));
        } else if (value instanceof String str) {
            _tempdict.put(name, Operand.Create(str));
        } else if (value instanceof DateTime date) {
            _tempdict.put(name, Operand.Create(date));
        } else {
            throw new Exception("setValue is error!");
        }
    }

    private Object js_getValue(String name) throws Exception {
        Operand value = GetTempParameter(name);
        if (value.Type() == OperandType.BOOLEAN) {
            return value.BooleanValue();
        } else if (value.Type() == OperandType.TEXT) {
            return value.TextValue();
        } else if (value.Type() == OperandType.DATE) {
            return value.DateValue().ToDateTime();
        } else if (value.Type() == OperandType.NUMBER) {
            return value.NumberValue();
        } else if (value.Type() == OperandType.NULL) {
            return null;
        } else if (value.Type() == OperandType.ARRARY) {
            var list = new Object[value.ArrayValue().size()];
            for (int i = 0; i < value.ArrayValue().size(); i++) {
                Operand op = value.ArrayValue().get(i);
                if (op.Type() == OperandType.BOOLEAN) {
                    list[i] = value.BooleanValue();
                } else if (op.Type() == OperandType.TEXT) {
                    list[i] = value.TextValue();
                } else if (op.Type() == OperandType.DATE) {
                    list[i] = value.DateValue().ToDateTime();
                } else if (op.Type() == OperandType.NUMBER) {
                    list[i] = value.NumberValue();
                } else if (op.Type() == OperandType.NULL) {
                    list[i] = null;
                } else {
                    list[i] = value.TextValue();
                }
            }
            return list;
        }
        throw new Exception("getValue is error!");
    }

    private Boolean js_hasKey(String name) {
        if (_initDict.containsKey(name)) {
            return true;
        } else if (_inputDict.containsKey(name)) {
            return true;
        } else if (_tempdict.containsKey(name)) {
            return true;
        } else if (_progDict.containsKey(name)) {
            return true;
        } else if (Project.HasFormula(name)) {
            return true;
        }
        return false;
    }

    public void Dispose() {
        Project = null;
        MachineSetting = null;
        AttachData = null;

        _tempNames = null;
        _progDict = null;
        _inputNumDict = null;
        _scriptDict = null;
        _initDict = null;
        _inputDict = null;
        Start = null;
    }
}
