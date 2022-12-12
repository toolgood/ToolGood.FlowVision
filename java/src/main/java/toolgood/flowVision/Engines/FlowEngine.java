package toolgood.flowVision.Engines;

import org.antlr.v4.runtime.CharStreams;
import org.antlr.v4.runtime.CommonTokenStream;
import org.joda.time.DateTime;
import toolgood.algorithm.Enums.*;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.AntlrCharStream;
import toolgood.algorithm.internals.AntlrErrorListener;
import toolgood.algorithm.internals.MathVisitor;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.litJson.JsonMapper;
import toolgood.algorithm.math.mathLexer;
import toolgood.algorithm.math.mathParser;
import toolgood.flowVision.Engines.OutDatas.ChannelNode;
import toolgood.flowVision.Engines.OutDatas.StockNode;
import toolgood.flowVision.Engines.OutDatas.TreeNode;
import toolgood.flowVision.Engines.Parameters.Setting_Machine;
import toolgood.flowVision.Flows.*;
import toolgood.flowVision.Flows.Enums.CellType;
import toolgood.flowVision.Flows.Enums.InputType;
import toolgood.flowVision.Flows.Interfaces.IInputNameNodeWork;
import toolgood.flowVision.Flows.Interfaces.ISettingFormulaNodeWork;
import toolgood.flowVision.ThirdParty.UnitConversion.AreaConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.DistanceConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.MassConverter;
import toolgood.flowVision.ThirdParty.UnitConversion.VolumeConverter;

import javax.script.Bindings;
import javax.script.ScriptContext;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import java.util.*;
import java.util.function.BiConsumer;
import java.util.function.Consumer;
import java.util.function.Function;
import java.util.function.Supplier;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public final class FlowEngine implements IFlowEngine {
    private ProjectWork Project;
    private Map<String, Setting_Machine> MachineSetting;
    private String AttachData;
    private TreeNode Start;
    private toolgood.flowVision.Flows.AppWork AppWork;
    private final Map<String, SettingFormulaWork> _startFormulaDict = new HashMap<String, SettingFormulaWork>();//用于默认
    private Map<String, SettingFormulaWork> _progDict = new HashMap<String, SettingFormulaWork>();//用于临时
    private final Map<String, TreeNode> _outputNumDict = new HashMap<String, TreeNode>();//用于保存出量来源

    private Map<String, TreeNode> _inputNumDict = new TreeMap<>();
    private Map<String, CustomFlowWork> _scriptDict = new HashMap<String, CustomFlowWork>();// 脚本

    private final Map<String, Operand> _tempdict = new TreeMap<>();// 临时变量
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

    public toolgood.flowVision.Flows.AppWork BindingJson(String appCode, String factoryCode, String json) throws Exception {
        if (Project.AppList.containsKey(appCode) == false) {
            throw new Exception("未找到编号[" + appCode + "]的流程");
        }
        this.AppWork = Project.AppList.get(appCode);
        if (Project.FactoryList.containsKey(factoryCode) == false) {
            throw new Exception("未找到编号[" + appCode + "]的厂区");
        }
        _inputDict.put("厂区编号", Operand.Create(factoryCode));
        _inputDict.put("厂区", Operand.Create(Project.FactoryList.get(factoryCode).Name));

        JsonData jlist = AddParameterFromJson(json);

        if (_tempdict.containsKey("数量")) {
            Operand operand = _tempdict.get("数量");
            if (operand.Type() != OperandType.NUMBER) {
                operand = operand.ToNumber();
                if (operand.IsError()) {
                    throw new Exception("[数量]类型为数字！");
                }
            }
            _inputDict.put("数量", operand);
        } else if (Project.NumberRequired == false) {
            _inputDict.put("数量", Operand.Create(0));
        } else {
            throw new Exception("[数量]必填！");
        }

        for (int i = 0; i < this.AppWork.InputList.size(); i++) {
            AppInputWork item = this.AppWork.InputList.get(i);
            if (jlist.inst_object.containsKey(item.InputName) == false) {
                if (item.IsRequired) {
                    throw new Exception("[" + item.InputName + "]必填！");
                }
                if (item.UseDefaultValue) {
                    if (item.InputType == InputType.Number) {
                        _inputDict.put(item.InputName, Operand.Create(Double.parseDouble(item.DefaultValue)));
                    } else if (item.InputType == InputType.String) {
                        _inputDict.put(item.InputName, Operand.Create(item.DefaultValue));
                    } else if (item.InputType == InputType.Bool) {
                        _inputDict.put(item.InputName, Operand.Create(Boolean.parseBoolean(item.DefaultValue)));
                    } else if (item.InputType == InputType.Date) {
                        _inputDict.put(item.InputName, Operand.Create(DateTime.parse(item.DefaultValue)));
                    } else if (item.InputType == InputType.List) {
                        _inputDict.put(item.InputName, Operand.Create(item.DefaultValue.split("[,，]")));
                    }
                }
            } else {
                if (item.InputType == InputType.Number) {
                    Operand temp = _tempdict.get(item.InputName);
                    if (temp.Type() == OperandType.TEXT && temp.TextValue().equals("")) {
                        if (item.IsRequired) {
                            throw new Exception("[" + item.InputName + "]必填！");
                        }
                        if (item.UseDefaultValue) {
                            _inputDict.put(item.InputName, Operand.Create(Double.parseDouble(item.DefaultValue)));
                        }
                        continue;
                    } else {
                        _inputDict.put(item.InputName, TextToNumber(item, temp));
                    }
                } else if (item.InputType == InputType.String) {
                    _inputDict.put(item.InputName, _tempdict.get(item.InputName).ToText());
                } else if (item.InputType == InputType.Bool) {
                    _inputDict.put(item.InputName, _tempdict.get(item.InputName).ToBoolean());
                } else if (item.InputType == InputType.Date) {
                    _inputDict.put(item.InputName, _tempdict.get(item.InputName).ToDate());
                } else if (item.InputType == InputType.List) {
                    _inputDict.put(item.InputName, Operand.Create(_tempdict.get(item.InputName).ToText().TextValue().split("[,，]")));
                }
            }
            if (item.IsRequired) {
                if (_inputDict.get(item.InputName).IsError()) {
                    throw new Exception("[" + item.InputName + "]类型错误！");
                }
                if (_inputDict.get(item.InputName).Type() == OperandType.TEXT && _inputDict.get(item.InputName).TextValue().equals("")) {
                    throw new Exception("[" + item.InputName + "]类型必填！");
                }
            } else {
                if (_inputDict.containsKey(item.InputName)) {
                    if (_inputDict.get(item.InputName).IsError()) {
                        throw new Exception("[" + item.InputName + "]类型必填！");
                    }
                }
            }
        }
        for (int i = 0; i < this.AppWork.InputList.size(); i++) {
            AppInputWork item = this.AppWork.InputList.get(i);
            if (_inputDict.containsKey(item.InputName) && item.Check(this) == false) {
                if (item.ErrorMessage == null) {
                    throw new Exception("[" + item.InputName + "]有误！");
                } else {
                    throw new Exception(item.ErrorMessage);
                }
            }
        }
        jlist = null;
        _tempdict.clear();

        for (int i = 0; i < this.AppWork.InitValueList.size(); i++) {
            AppInitValueWork initValue = this.AppWork.InitValueList.get(i);
            boolean check = false;
            for (int j = 0; j < initValue.Conditions.size(); j++) {
                SettingFormulaItemWork item = initValue.Conditions.get(i);
                if (item.Check(this)) {
                    check = true;
                    _initDict.put(initValue.Name, item.EvaluateFormula(this, initValue.InputType));
                    break;
                }
            }
            if (check == false && initValue.IsThrowError) {
                throw new Exception(initValue.ErrorMessage);
            }
            if (_initDict.get(initValue.Name).IsError()) {
                if (initValue.ErrorMessage == null || initValue.ErrorMessage.equals("")) {
                    throw new Exception(initValue.Name + " is error");
                }
                throw new Exception(initValue.ErrorMessage);
            }
        }
        return this.AppWork;
    }

    private JsonData AddParameterFromJson(String json) throws Exception {
        try {
            JsonData jo = JsonMapper.ToObject(json);
            if (jo.IsObject()) {
                for (String key : jo.inst_object.keySet()) {
                    JsonData v = jo.inst_object.get(key);
                    if (v.IsString())
                        _tempdict.put(key, Operand.Create(v.StringValue()));
                    else if (v.IsBoolean())
                        _tempdict.put(key, Operand.Create(v.BooleanValue()));
                    else if (v.IsDouble())
                        _tempdict.put(key, Operand.Create(v.NumberValue()));
                    else if (v.IsObject())
                        _tempdict.put(key, Operand.Create(v));
                    else if (v.IsArray())
                        _tempdict.put(key, Operand.Create(v));
                    else if (v.IsNull())
                        _tempdict.put(key, Operand.CreateNull());
                }
                return jo;
            }
        } catch (Exception ex) {
        }
        throw new Exception("json is error.");
    }

    private static final Pattern numRegex = Pattern.compile("^(\\d[0-9\\.]*)([^\\d][\\S\\s]*)$");

    private Operand TextToNumber(AppInputWork appInput, Operand operand) {
        if (operand.Type() == OperandType.NUMBER) {
            return operand;
        }
        Operand result = operand.ToNumber(); // 科学标记法
        if (result.IsError() && operand.Type() == OperandType.TEXT) {
            Matcher m = numRegex.matcher(operand.TextValue());
            if (m.find()) {
                try {
                    double d = Double.parseDouble(m.group(1));
                    double r = UnitConversion(d, m.group(2).trim(), appInput.Unit, appInput.InputName);
                    return Operand.Create(r);
                } catch (Exception ex) {
                }
            }
        }
        return result;
    }

    private double UnitConversion(double src, String oldSrcUnit, String oldTarUnit, String name) throws Exception {
        if (oldSrcUnit == null || oldSrcUnit.equals("")) {
            return src;
        }
        if (oldTarUnit == null || oldTarUnit.equals("")) {
            throw new Exception("输入项[" + name + "]单位不同，无法从[" + oldSrcUnit + "]转成[" + oldTarUnit + "]");
        }
        oldSrcUnit = oldSrcUnit.replaceAll("[\\s \\(\\)（）\\[\\]<>]", "");
        if (oldSrcUnit.equals(oldTarUnit)) {
            return src;
        }

        if (DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
            DistanceConverter c = new DistanceConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
            MassConverter c = new MassConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
            AreaConverter c = new AreaConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        if (VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
            VolumeConverter c = new VolumeConverter(oldSrcUnit, oldTarUnit);
            return c.LeftToRight(src);
        }
        throw new Exception("输入项[" + name + "]单位不同，无法从[" + oldSrcUnit + "]转成[" + oldTarUnit + "]");
    }

    public List<String> BuildTreeNode(String appCode, String factoryCode, String json) throws Exception {
        return BuildTreeNode(appCode, factoryCode, json, null);
    }

    public List<String> BuildTreeNode(String appCode, String factoryCode, String json, String previous) throws Exception {
        AppWork app = BindingJson(appCode, factoryCode, json);
        BindingPreviousJson(previous);

        _tempNames = new HashSet<String>();
        _tempdictCount = new HashMap<>();

        StockNode stock = new StockNode();
        Start = new TreeNode(app.Start, null, null);
        AddSettingFormulaToDict(Start);
        stock.Push(Start, null, 0);
        int errorLayer = -1;
        String errorMessage = null;
        HashSet<String> errorIds = new HashSet<String>();
        Map<String, TreeNode> temp = new HashMap<String, TreeNode>();
        Stack<String> outStatus = new Stack<String>();

        while (stock.IsNotNull()) {
            ChannelNode channelNode = stock.Pop();
            TreeNode currTreeNode = channelNode.Node;
            NodeWork work = currTreeNode.CurrWork;
            String channel = channelNode.Channel;

            // 初始 渠道
            if (channel == null) {
                List<String> channels = work.GetChannels();
                for (int i = 0; i < channels.size(); i++) {
                    stock.Push(currTreeNode, channels.get(i), 0);
                }
                continue;
            }

            //没有下一个节点   work.NextNodes == null || work.NextNodes.Count == 0 || work.NextNodes[channel].Count == 0 ||
            if (channelNode.Index >= work.NextNodes.get(channel).size()) {
                currTreeNode.SetError(channel);
                for (int i = 0; i < currTreeNode.PrevNodes.size(); i++) {
                    ChannelNode prev = currTreeNode.PrevNodes.get(i);
                    stock.Push(prev.Node, prev.Channel, prev.Index + 1);
                }
                RemoveSettingFormulaToDict(currTreeNode);
                errorIds.add(work.Id);
                continue;
            }
            List<NodeWork> arr = work.NextNodes.get(channel);
            NodeWork nextNode = arr.get(channelNode.Index);
            if (errorIds.contains(nextNode.Id)) {
                if (nextNode instanceof ErrorFlowWork) { // 错误，当前设置错误
                    currTreeNode.SetError(channel);
                    for (int i = 0; i < currTreeNode.PrevNodes.size(); i++) {
                        ChannelNode prev = currTreeNode.PrevNodes.get(i);
                        stock.Push(prev.Node, prev.Channel, prev.Index + 1);
                        errorIds.add(prev.Node.Id());
                    }
                    RemoveSettingFormulaToDict(currTreeNode);
                } else { // 普通 不匹配 ，匹配下一个
                    stock.Push(currTreeNode, channel, channelNode.Index + 1);
                }
                continue;
            }
            TreeNode treeNode;
            if (nextNode instanceof ProcedureFlowWork procedureWork) { // 工艺
                if (procedureWork.CheckWithoutFactory(this)) {
                    if (procedureWork.FactoryProcedure.Items.containsKey(factoryCode)) {
                        FactoryProcedureItemWork factoryProcedureItemWork = procedureWork.FactoryProcedure.Items.get(factoryCode);
                        boolean hasMachines = false;
                        if (procedureWork.Machines.containsKey(factoryCode)) {
                            List<ProcedureFlowMachineInfo> machines = procedureWork.Machines.get(factoryCode);
                            hasMachines = true;
                            boolean match = false;
                            for (int i = 0; i < machines.size(); i++) {
                                ProcedureFlowMachineInfo machine = machines.get(i);
                                if (CheckMachine(machine.FactoryMachine.MachineCategoryCode, machine.FactoryMachine.MachineCode) && machine.Check(this)) {
                                    match = true;

                                    if (temp.containsKey(nextNode.Id)) {
                                        treeNode = temp.get(nextNode.Id);
                                    } else {
                                        treeNode = new TreeNode(nextNode, machine.FactoryMachine, factoryProcedureItemWork);
                                        temp.put(nextNode.Id, treeNode);
                                    }
                                    AddSettingFormulaToDict(treeNode);
                                    currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                                    stock.Push(treeNode, null, -1);
                                    break;
                                }
                            }
                            if (match) {
                                continue;
                            }
                        }
                        if (procedureWork.MachineRequired == false) {
                            if (temp.containsKey(nextNode.Id)) {
                                treeNode = temp.get(nextNode.Id);
                            } else {
                                treeNode = new TreeNode(nextNode, null, factoryProcedureItemWork);
                                temp.put(nextNode.Id, treeNode);
                            }
                            AddSettingFormulaToDict(treeNode);
                            currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                            stock.Push(treeNode, null, -1);
                            continue;
                        }
                        if (hasMachines) {
                            errorMessage = SetError(nextNode, "工艺[{nextNode.Label}]厂区未找到匹配的机器", errorLayer);
                        } else {
                            errorMessage = SetError(nextNode, "工艺[{nextNode.Label}]厂区无相关机器", errorLayer);
                        }
                    } else {
                        errorMessage = SetError(nextNode, "工艺[{nextNode.Label}]厂区无相关工艺", errorLayer);
                    }
                }
            } else if (nextNode instanceof ErrorFlowWork error) {
                if (error.Check(this, factoryCode)) {
                    errorMessage = SetError(nextNode, error.ErrorMessage, errorLayer);
                    currTreeNode.SetError(channel);
                    for (int i = 0; i < currTreeNode.PrevNodes.size(); i++) {
                        ChannelNode prev = currTreeNode.PrevNodes.get(i);
                        stock.Push(prev.Node, prev.Channel, prev.Index + 1);
                        errorIds.add(prev.Node.Id());
                    }
                    RemoveSettingFormulaToDict(currTreeNode);
                    errorIds.add(error.Id);
                    continue;
                }
                stock.Push(currTreeNode, channel, channelNode.Index + 1);// 未匹配 匹配下一个
                continue;
            } else if (nextNode.Check(this, factoryCode)) {
                if (temp.containsKey(nextNode.Id)) {
                    treeNode = temp.get(nextNode.Id);
                } else {
                    treeNode = new TreeNode(nextNode, null, null);
                    temp.put(nextNode.Id, treeNode);
                }
                AddSettingFormulaToDict(treeNode);
                currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                if (nextNode.NodeType == CellType.End) {
                    outStatus.push("END");
                    continue;
                }// 结束
                if (nextNode instanceof StatusFlowWork status) {
                    outStatus.push(status.Status);
                }
                stock.Push(treeNode, null, -1);// 下一个节点
                continue;
            } else if (nextNode instanceof StatusFlowWork status) { // 必须在 nextNode.Check(this, factoryCode) 之后
                if (status.CheckStatus(this)) {
                    if (temp.containsKey(nextNode.Id)) {
                        treeNode = temp.get(nextNode.Id);
                    } else {
                        treeNode = new TreeNode(nextNode, null, null);
                        temp.put(nextNode.Id, treeNode);
                    }
                    //AddSettingFormulaToDict(treeNode); //通过之后
                    currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                    outStatus.push(status.Status);
                    continue;
                }
            }
            errorIds.add(nextNode.Id);
            stock.Push(currTreeNode, channel, channelNode.Index + 1);// 未匹配 匹配下一个
        }
        stock = null;
        temp = null;
        errorIds = null;
        _tempNames.clear();
        _tempNames = null;
        _tempdictCount.clear();
        _tempdictCount = null;

        if (Start.NextNodes.size() == 0) {
            if (errorMessage == null || errorMessage.equals("")) {
                errorMessage = "[{appCode}]流程未能匹配！";
            }
            throw new Exception(errorMessage);
        }
        List<String> result = new ArrayList<>(outStatus.size());

        while (outStatus.empty() == false) {
            String status = outStatus.pop();
            result.add(status);
        }
        return result;
    }

    private String SetError(NodeWork node, String message, Integer errorLayer) {
        if (node.Layer > errorLayer) {
            errorLayer = node.Layer;
            return message;
        }
        return null;
    }

    private void AddSettingFormulaToDict(TreeNode node) throws Exception {
        if (node.CurrWork instanceof StartFlowWork start) {
            for (int i = 0; i < start.SettingFormula.size(); i++) {
                SettingFormulaWork item = start.SettingFormula.get(i);
                _startFormulaDict.put(item.Name, item);
            }
            return;
        }
        if (node.CurrWork instanceof ISettingFormulaNodeWork work) {

            for (int i = 0; i < work.SettingFormula().size(); i++) {
                SettingFormulaWork item = work.SettingFormula().get(i);
                if (_tempNames.add(item.Name)) {
                    _progDict.put(item.Name, item);
                    _outputNumDict.put(item.Name, node);
                } else {
                    throw new Exception(GetDuplicateNameErrorMessage(item.Name, node.Id(), node.CurrWork.Label));
                }
            }
        }
        if (node.CurrWork instanceof IInputNameNodeWork inputNameNodeWork) {
            if (inputNameNodeWork.InputName() == null || inputNameNodeWork.InputName().equals("")) {
            } else if (_tempNames.add(inputNameNodeWork.InputName())) {
                _inputNumDict.put(inputNameNodeWork.InputName(), node);
            } else {
                throw new Exception(GetDuplicateNameErrorMessage(inputNameNodeWork.InputName(), node.Id(), node.CurrWork.Label));
            }
        }
        if (node.CurrWork instanceof CustomFlowWork custom) {
            for (int i = 0; i < custom.Names.size(); i++) {
                String name = custom.Names.get(i);
                if (_tempNames.add(name)) {
                    _scriptDict.put(name, custom);
                    _outputNumDict.put(name, node);
                } else {
                    throw new Exception(GetDuplicateNameErrorMessage(name, node.Id(), node.CurrWork.Label));
                }
            }
        }
        _tempdictCount.put(node.CurrWork.Id, _tempdict.size());
    }

    private String GetDuplicateNameErrorMessage(String name, String id, String label) {
        if (_initDict.containsKey(name)) {
            return "变量名[" + name + "]使用两次，" + label + "[" + id + "]与[初始值]名称相同 ";
        }
        if (_inputDict.containsKey(name)) {
            return "变量名[" + name + "]使用两次，" + label + "[" + id + "]与[输入项]名称相同 ";
        }
        if (_progDict.containsKey(name)) {
            String id2 = _progDict.get(name).NodeWork.Id;
            String label2 = _progDict.get(name).NodeWork.Label;
            return "变量名[" + name + "]使用两次，" + label + "[" + id + "]与" + label2 + "[" + id2 + "]内的名称相同 ";
        }
        if (_inputNumDict.containsKey(name)) {
            String id2 = _inputNumDict.get(name).Id();
            String label2 = _inputNumDict.get(name).CurrWork.Label;
            return "变量名[" + name + "]使用两次，" + label + "[" + id + "]与" + label2 + "[" + id2 + "]内的[入量名称]相同 ";
        }
        if (_scriptDict.containsKey(name)) {
            String id2 = _scriptDict.get(name).Id;
            String label2 = _scriptDict.get(name).Label;
            return "变量名[" + name + "]使用两次，" + label + "[" + id + "]与" + label2 + "[" + id2 + "]内的脚本内变量名称相同 ";
        }
        return "变量名[" + name + "]使用两次，id:" + id + "";
    }

    private void RemoveSettingFormulaToDict(TreeNode node) {
        if (node.CurrWork instanceof ISettingFormulaNodeWork work) {
            for (int i = 0; i < work.SettingFormula().size(); i++) {
                SettingFormulaWork item = work.SettingFormula().get(i);
                _tempNames.remove(item.Name);
                _progDict.remove(item.Name);
                _outputNumDict.remove(item.Name);
                _tempdict.remove(item.Name);
            }
        }
        if (node.CurrWork instanceof IInputNameNodeWork inputNameNodeWork) {
            _tempNames.remove(inputNameNodeWork.InputName());
            _inputNumDict.remove(inputNameNodeWork.InputName());
        }
        if (node.CurrWork instanceof CustomFlowWork custom) {
            for (int i = 0; i < custom.Names.size(); i++) {
                String name = custom.Names.get(i);
                _tempNames.remove(name);
                _scriptDict.remove(name);
                _outputNumDict.remove(name);
            }
        }
        if (_tempdictCount.containsKey(node.CurrWork.Id)) {
            List<String> keys = _tempdict.keySet().stream().toList();
            while (keys.size() > _tempdictCount.get(node.CurrWork.Id)) {
                String key = keys.get(keys.size() - 1);
                keys.remove(keys.size() - 1);
                _tempdict.remove(key);
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
                    _tempdict.put(work.InputName(), Operand.Create(treeNode.InputNum.doubleValue()));
                }
            }
        }
        Start.EvaluateInputNum(this);
    }

    public void BindingPreviousJson(String previous) {
        if (previous == null || previous.equals("")) {
            return;
        }
        _initDict.put("上个流程", Operand.CreateJson(previous));
    }


    public Operand GetNum() {
        return _inputDict.get("数量");
    }

    private final Stack<Operand> _tempOutputs = new Stack<Operand>();

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

        mathParser.ProgContext context = parser.prog();
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
        visitor.DistanceUnit = DistanceUnitType.intToEnum(Project.Distance);
        visitor.AreaUnit = AreaUnitType.intToEnum(Project.Area);
        visitor.VolumeUnit = VolumeUnitType.intToEnum(Project.Volume);
        visitor.MassUnit = MassUnitType.intToEnum(Project.Mass);

        Operand result = visitor.visit(context);
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
        MathVisitor visitor = new MathVisitor();
        visitor.GetParameter = f -> {
            try {
                return GetTempParameter(f);
            } catch (Exception e) {
            }
            return null;
        };
        visitor.excelIndex = Project.ExcelIndex;
        visitor.DistanceUnit = DistanceUnitType.intToEnum(Project.Distance);
        visitor.AreaUnit = AreaUnitType.intToEnum(Project.Area);
        visitor.VolumeUnit = VolumeUnitType.intToEnum(Project.Volume);
        visitor.MassUnit = MassUnitType.intToEnum(Project.Mass);

        Operand result = visitor.visit(context);
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
            return !machine.IsStop;
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

    String js_error = null;

    private void EvaluateJs(String script) throws Exception {
        ScriptEngineManager engineManager = new ScriptEngineManager();
        ScriptEngine jsEngine = engineManager.getEngineByName("nashorn");

        Bindings bindings = jsEngine.getBindings(ScriptContext.ENGINE_SCOPE);
        bindings.put("getDatas", (Supplier<String>) this::js_getDatas);
        bindings.put("getValue", (Function<String, Object>) this::js_getValue);
        bindings.put("hasKey", (Function<String, Boolean>) this::js_hasKey);
        bindings.put("setValue", (BiConsumer<String, Object>) this::js_setValue);
        bindings.put("error", (Consumer<String>) this::js_Error);
        jsEngine.eval(script);
        if (js_error != null) throw new Exception(js_error);
    }

    private void js_Error(String msg) {
        if (js_error != null) return;
        js_error = msg;
    }

    private String js_getDatas() {
        return AttachData;
    }

    private void js_setValue(String name, Object value) {
        if (js_error != null) {
        } else if (value == null) {
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
            js_error = "setValue is error!";
        }
    }

    private Object js_getValue(String name) {
        if (js_error != null) return null;

        Operand value = null;
        try {
            value = GetTempParameter(name);
        } catch (Exception e) {
            js_error = e.getMessage();
            return null;
        }
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
            Object[] list = new Object[value.ArrayValue().size()];
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
        js_error = "getValue is error!";
        return null;
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
        } else return Project.HasFormula(name);
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
