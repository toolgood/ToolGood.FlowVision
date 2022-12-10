using Antlr4.Runtime;
using Jint;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ToolGood.Algorithm;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.LitJson;
using ToolGood.FlowVision.Engines.OutDatas;
using ToolGood.FlowVision.Engines.Parameters;
using ToolGood.FlowVision.Flows;
using UnitConversion;
using static ToolGood.Algorithm.mathParser;

namespace ToolGood.FlowVision.Engines
{
    public sealed class FlowEngine : IDisposable
    {
        internal ProjectWork Project;//{ get; private set; }
        internal Dictionary<string, Setting_Machine> MachineSetting;// { get; private set; }
        internal string AttachData;// { get; private set; }
        internal TreeNode Start;
        internal AppWork AppWork;

        #region 初始化 Project MachineSetting HolidaySetting
        public FlowEngine(ProjectWork project, string attachData = null, List<Setting_Machine> machines = null)
        {
            Project = project;
            AttachData = attachData;
            if (machines != null) {
                MachineSetting = new Dictionary<string, Setting_Machine>();
                for (int i = 0; i < machines.Count; i++) {
                    var machine = machines[i];
                    MachineSetting[machine.CategoryCode + '|' + machine.Code] = machine;
                }
            }
        }
        #endregion

        internal Dictionary<string, SettingFormulaWork> _startFormulaDict = new Dictionary<string, SettingFormulaWork>();//用于默认
        internal Dictionary<string, SettingFormulaWork> _progDict = new Dictionary<string, SettingFormulaWork>();//用于临时
        internal Dictionary<string, TreeNode> _outputNumDict = new Dictionary<string, TreeNode>();//用于保存出量来源

        internal Dictionary<string, TreeNode> _inputNumDict = new Dictionary<string, TreeNode>();
        internal Dictionary<string, CustomFlowWork> _scriptDict = new Dictionary<string, CustomFlowWork>();// 脚本

        internal Dictionary<string, Operand> _tempdict = new Dictionary<string, Operand>();// 临时变量
        internal Dictionary<string, Operand> _initDict = new Dictionary<string, Operand>();//保存 初始值
        internal Dictionary<string, Operand> _inputDict = new Dictionary<string, Operand>();//保存 输入项

        #region BindingJson
        internal AppWork BindingJson(string appCode, string factoryCode, string json)
        {
            #region 流程
            AppWork app;
            if (Project.AppList.TryGetValue(appCode, out app) == false) {
                throw new Exception($"未找到编号[{appCode}]的流程");
            }
            AppWork = app;
            #endregion

            #region 厂区
            if (Project.FactoryList.ContainsKey(factoryCode) == false) {
                throw new Exception($"未找到编号[{factoryCode}]的厂区");
            }
            _inputDict["厂区编号"] = Operand.Create(factoryCode);
            _inputDict["厂区"] = Operand.Create(Project.FactoryList[factoryCode].Name);
            #endregion

            #region 数量
            var jlist = AddParameterFromJson(json);
            if (_tempdict.TryGetValue("数量", out Operand operand)) {
                if (operand.Type != OperandType.NUMBER) { operand = operand.ToNumber(); if (operand.IsError) { throw new Exception("[数量]类型为数字！"); } }
                _inputDict["数量"] = operand;
            } else if (Project.NumberRequired == false) {
                _inputDict["数量"] = Operand.Create(0);
            } else {
                throw new Exception("[数量]必填！");
            }
            #endregion

            #region 输入值检验
            for (int i = 0; i < app.InputList.Count; i++) {
                var item = app.InputList[i];
                if (jlist.inst_object.ContainsKey(item.InputName) == false) {
                    if (item.IsRequired) { throw new Exception($"[{item.InputName}]必填！"); }
                    if (item.UseDefaultValue) {
                        if (item.InputTypeNum == (int)InputType.Number) {
                            _inputDict[item.InputName] = Operand.Create(double.Parse(item.DefaultValue));
                        } else if (item.InputTypeNum == (int)InputType.String) {
                            _inputDict[item.InputName] = Operand.Create(item.DefaultValue);
                        } else if (item.InputTypeNum == (int)InputType.Bool) {
                            _inputDict[item.InputName] = Operand.Create(bool.Parse(item.DefaultValue));
                        } else if (item.InputTypeNum == (int)InputType.Date) {
                            _inputDict[item.InputName] = Operand.Create(DateTime.Parse(item.DefaultValue));
                        } else if (item.InputTypeNum == (int)InputType.List) {
                            _inputDict[item.InputName] = Operand.Create(item.DefaultValue.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                } else {
                    if (item.InputTypeNum == (int)InputType.Number) {
                        if (_tempdict[item.InputName].Type == OperandType.TEXT && string.IsNullOrEmpty(_tempdict[item.InputName].TextValue)) {
                            if (item.IsRequired) { throw new Exception($"[{item.InputName}]必填！"); }
                            if (item.UseDefaultValue) {
                                _inputDict[item.InputName] = Operand.Create(double.Parse(item.DefaultValue));
                            }
                            continue;
                        } else {
                            _inputDict[item.InputName] = TextToNumber(item, _tempdict[item.InputName]);
                        }
                    } else if (item.InputTypeNum == (int)InputType.String) {
                        _inputDict[item.InputName] = _tempdict[item.InputName].ToText();
                    } else if (item.InputTypeNum == (int)InputType.Bool) {
                        _inputDict[item.InputName] = _tempdict[item.InputName].ToBoolean();
                    } else if (item.InputTypeNum == (int)InputType.Date) {
                        _inputDict[item.InputName] = _tempdict[item.InputName].ToMyDate();
                    } else if (item.InputTypeNum == (int)InputType.List) {
                        _inputDict[item.InputName] = Operand.Create(_tempdict[item.InputName].ToText().TextValue.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries));
                    }
                }
                if (item.IsRequired) {
                    if (_inputDict[item.InputName].IsError) { throw new Exception($"[{item.InputName}]类型错误！"); }
                    if (_inputDict[item.InputName].Type == OperandType.TEXT && string.IsNullOrEmpty(_inputDict[item.InputName].TextValue)) {
                        throw new Exception($"[{item.InputName}]类型必填！");
                    }
                } else {
                    if (_inputDict.ContainsKey(item.InputName)) {
                        if (_inputDict[item.InputName].IsError) { throw new Exception($"[{item.InputName}]类型错误！"); }
                    }
                }
            }
            for (int i = 0; i < app.InputList.Count; i++) {
                var item = app.InputList[i];
                if (_inputDict.ContainsKey(item.InputName) && item.Check(this) == false) {
                    if (item.ErrorMessage == null) {
                        throw new Exception($"[{item.InputName}]有误！");
                    } else {
                        throw new Exception(item.ErrorMessage);
                    }
                }
            }
            jlist = null;
            _tempdict.Clear();
            #endregion

            #region 初始值 检验  初始值可以替换 输入值
            for (int i = 0; i < app.InitValueList.Count; i++) {
                var initValue = app.InitValueList[i];
                bool check = false;
                for (int j = 0; j < initValue.Conditions.Count; j++) {
                    var item = initValue.Conditions[j];
                    if (item.Check(this)) {
                        check = true;
                        _initDict[initValue.Name] = item.EvaluateFormula(this, initValue.InputType);
                        break;
                    }
                }
                if (check == false && initValue.IsThrowError) {
                    throw new Exception(initValue.ErrorMessage);
                }
                if (_initDict[initValue.Name].IsError) {
                    if (string.IsNullOrEmpty(initValue.ErrorMessage)) {
                        throw new Exception(initValue.Name + " is error");
                    }
                    throw new Exception(initValue.ErrorMessage);
                }
            }
            #endregion

            return app;
        }
        private JsonData AddParameterFromJson(string json)
        {
            try {
                var jo = JsonMapper.ToObject(json);
                if (jo.IsObject) {
                    foreach (var item in jo.inst_object) {
                        var v = item.Value;
                        if (v.IsString)
                            _tempdict[item.Key] = Operand.Create(v.StringValue);
                        else if (v.IsBoolean)
                            _tempdict[item.Key] = Operand.Create(v.BooleanValue);
                        else if (v.IsDouble)
                            _tempdict[item.Key] = Operand.Create(v.NumberValue);
                        else if (v.IsObject)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsArray)
                            _tempdict[item.Key] = Operand.Create(v);
                        else if (v.IsNull)
                            _tempdict[item.Key] = Operand.CreateNull();
                    }
                    return jo;
                }
            } catch (Exception) { }
            throw new Exception("json is error.");
        }

        private static readonly Regex numRegex = new Regex(@"^(\d[0-9\.]*)([^\d][\S\s]*)$", RegexOptions.Compiled);
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo("en-US");
        private Operand TextToNumber(AppInputWork appInput, Operand operand)
        {
            if (operand.Type == OperandType.NUMBER) { return operand; }
            var result = operand.ToNumber(); // 科学标记法
            if (result.IsError && operand.Type == OperandType.TEXT) {
                var m = numRegex.Match(operand.TextValue);
                if (m.Success) {
                    if (double.TryParse(m.Groups[1].Value, NumberStyles.Any, cultureInfo, out double d)) {
                        var r = UnitConversion(d, m.Groups[2].Value.Trim(), appInput.Unit, appInput.InputName);
                        return Operand.Create(r);
                    }
                }
            }
            return result;
        }
        private static readonly Regex unitRegex = new Regex(@"[\s \(\)（）\[\]<>]", RegexOptions.Compiled);
        private double UnitConversion(double src, string oldSrcUnit, string oldTarUnit, string name)
        {
            if (string.IsNullOrWhiteSpace(oldSrcUnit)) { return src; }
            if (string.IsNullOrWhiteSpace(oldTarUnit)) { throw new Exception($"输入项[{name}]单位不同，无法从[{oldSrcUnit}]转成[{oldTarUnit}]"); }
            oldSrcUnit = unitRegex.Replace(oldSrcUnit, "");
            if (oldSrcUnit == oldTarUnit) { return src; }

            if (DistanceConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new DistanceConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (MassConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new MassConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (AreaConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new AreaConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            if (VolumeConverter.Exists(oldSrcUnit, oldTarUnit)) {
                var c = new VolumeConverter(oldSrcUnit, oldTarUnit);
                return c.LeftToRight(src);
            }
            throw new Exception($"输入项[{name}]单位不同，无法从[{oldSrcUnit}]转成[{oldTarUnit}]");
        }
        #endregion

        #region BindingPreviousJson

        internal void BindingPreviousJson(string previous)
        {
            if (string.IsNullOrWhiteSpace(previous)) { return; }
            _initDict["上个流程"] = Operand.CreateJson(previous);
        }
        #endregion

        #region BuildTreeNode
        internal HashSet<string> _tempNames;
        internal Dictionary<string, int> _tempdictCount;//用于删除临时变量

        /// <summary>
        /// 返回的是状态码
        /// </summary>
        /// <param name="appCode"></param>
        /// <param name="factoryCode"></param>
        /// <param name="json"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<string> BuildTreeNode(string appCode, string factoryCode, string json, string previous=null)
        {
            var app = BindingJson(appCode, factoryCode, json);
            BindingPreviousJson(previous);

            _tempNames = new HashSet<string>();
            _tempdictCount = new Dictionary<string, int>();

            StockNode stock = new StockNode();
            Start = new TreeNode(app.Start, null, null);
            AddSettingFormulaToDict(Start);
            stock.Push(Start, null, 0);
            int errorLayer = -1;
            string errorMessage = null;
            HashSet<string> errorIds = new HashSet<string>();
            Dictionary<string, TreeNode> temp = new Dictionary<string, TreeNode>();
            Stack<string> outStatus = new Stack<string>();

            while (stock.TryPop(out ChannelNode channelNode)) {
                var currTreeNode = channelNode.Node;
                var work = currTreeNode.CurrWork;
                var channel = channelNode.Channel;

                // 初始 渠道
                if (channel == null) { var channels = work.GetChannels(); for (int i = 0; i < channels.Count; i++) { stock.Push(currTreeNode, channels[i], 0); } continue; }

                //没有下一个节点   work.NextNodes == null || work.NextNodes.Count == 0 || work.NextNodes[channel].Count == 0 ||
                if (channelNode.Index >= work.NextNodes[channel].Count) {
                    currTreeNode.SetError(channel);
                    for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); }
                    RemoveSettingFormulaToDict(currTreeNode);
                    errorIds.Add(work.Id);
                    continue;
                }
                var arr = work.NextNodes[channel];
                var nextNode = arr[channelNode.Index];
                if (errorIds.Contains(nextNode.Id)) {
                    if (nextNode is ErrorFlowWork) { // 错误，当前设置错误 
                        currTreeNode.SetError(channel);
                        for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); errorIds.Add(prev.Node.Id); }
                        RemoveSettingFormulaToDict(currTreeNode);
                    } else { // 普通 不匹配 ，匹配下一个
                        stock.Push(currTreeNode, channel, channelNode.Index + 1);
                    }
                    continue;
                }
                TreeNode treeNode;
                if (nextNode is ProcedureFlowWork procedureWork) { // 工艺
                    if (procedureWork.CheckWithoutFactory(this)) {
                        if (procedureWork.FactoryProcedure.Items.TryGetValue(factoryCode, out FactoryProcedureItemWork factoryProcedureItemWork)) {
                            bool hasMachines = false;
                            if (procedureWork.Machines.TryGetValue(factoryCode, out List<ProcedureFlowMachineInfo> machines)) {
                                hasMachines = true;
                                bool match = false;
                                for (int i = 0; i < machines.Count; i++) {
                                    var machine = machines[i];
                                    if (CheckMachine(machine.FactoryMachine.MachineCategoryCode, machine.FactoryMachine.MachineCode) && machine.Check(this)) {
                                        match = true;
                                        if (temp.TryGetValue(nextNode.Id, out treeNode) == false) {
                                            treeNode = new TreeNode(nextNode, machine.FactoryMachine, factoryProcedureItemWork);
                                            temp[nextNode.Id] = treeNode;
                                        }
                                        AddSettingFormulaToDict(treeNode);
                                        currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                                        stock.Push(treeNode, null, -1);
                                        break;
                                    }
                                }
                                if (match) { continue; }
                            }
                            if (procedureWork.MachineRequired == false) {
                                if (temp.TryGetValue(nextNode.Id, out treeNode) == false) {
                                    treeNode = new TreeNode(nextNode, null, factoryProcedureItemWork);
                                    temp[nextNode.Id] = treeNode;
                                }
                                AddSettingFormulaToDict(treeNode);
                                currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                                stock.Push(treeNode, null, -1);
                                continue;
                            }
                            if (hasMachines) {
                                SetError(nextNode, $"工艺[{nextNode.Label}]厂区未找到匹配的机器", ref errorLayer, ref errorMessage);
                            } else {
                                SetError(nextNode, $"工艺[{nextNode.Label}]厂区无相关机器", ref errorLayer, ref errorMessage);
                            }
                        } else {
                            SetError(nextNode, $"工艺[{nextNode.Label}]厂区无相关工艺", ref errorLayer, ref errorMessage);
                        }
                    }
                } else if (nextNode is ErrorFlowWork error) {
                    if (error.Check(this, factoryCode)) {
                        SetError(nextNode, error.ErrorMessage, ref errorLayer, ref errorMessage);
                        currTreeNode.SetError(channel);
                        for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); errorIds.Add(prev.Node.Id); }
                        RemoveSettingFormulaToDict(currTreeNode);
                        errorIds.Add(error.Id);
                        continue;
                    }
                    stock.Push(currTreeNode, channel, channelNode.Index + 1);// 未匹配 匹配下一个
                    continue;
                } else if (nextNode.Check(this, factoryCode)) {
                    if (temp.TryGetValue(nextNode.Id, out treeNode) == false) {
                        treeNode = new TreeNode(nextNode, null, null);
                        temp[nextNode.Id] = treeNode;
                    }
                    AddSettingFormulaToDict(treeNode);
                    currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                    if (nextNode.NodeType == CellType.End) { outStatus.Push("END"); continue; }// 结束
                    if (nextNode is StatusFlowWork status) { outStatus.Push(status.Status); }
                    stock.Push(treeNode, null, -1);// 下一个节点
                    continue;
                } else if (nextNode is StatusFlowWork status) { // 必须在 nextNode.Check(this, factoryCode) 之后
                    if (status.CheckStatus(this)) {
                        if (temp.TryGetValue(nextNode.Id, out treeNode) == false) {
                            treeNode = new TreeNode(nextNode, null, null);
                            temp[nextNode.Id] = treeNode;
                        }
                        //AddSettingFormulaToDict(treeNode); //通过之后
                        currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
                        outStatus.Push(status.Status);
                        continue;
                    }
                }
                errorIds.Add(nextNode.Id);
                stock.Push(currTreeNode, channel, channelNode.Index + 1);// 未匹配 匹配下一个
            }
            stock = null;
            temp = null;
            errorIds = null;
            _tempNames.Clear();
            _tempNames = null;
            _tempdictCount.Clear();
            _tempdictCount = null;

            if (Start.NextNodes.Count == 0) {
                if (string.IsNullOrEmpty(errorMessage)) { errorMessage = $"[{appCode}]流程未能匹配！"; }
                throw new Exception(errorMessage);
            }
            List<string> result = new List<string>(outStatus.Count);
            while (outStatus.TryPop(out string status)) {
                result.Add(status);
            }
            return result;
        }
        internal void SetError(NodeWork node, string message, ref int errorLayer, ref string errorMessage)
        {
            if (node.Layer > errorLayer) {
                errorLayer = node.Layer;
                errorMessage = message;
            }
        }

        private void AddSettingFormulaToDict(TreeNode node)
        {
            if (node.CurrWork is StartFlowWork start) {
                for (int i = 0; i < start.SettingFormula.Count; i++) {
                    var item = start.SettingFormula[i];
                    _startFormulaDict[item.Name] = item;
                }
                return;
            }
            if (node.CurrWork is ISettingFormulaNodeWork work) {

                for (int i = 0; i < work.SettingFormula.Count; i++) {
                    var item = work.SettingFormula[i];
                    if (_tempNames.Add(item.Name)) {
                        _progDict.Add(item.Name, item);
                        _outputNumDict.Add(item.Name, node);
                    } else {
                        throw new Exception(GetDuplicateNameErrorMessage(item.Name, node.Id, node.CurrWork.Label));
                    }
                }
            }
            if (node.CurrWork is IInputNameNodeWork inputNameNodeWork) {
                if (string.IsNullOrEmpty(inputNameNodeWork.InputName)) {
                } else if (_tempNames.Add(inputNameNodeWork.InputName)) {
                    _inputNumDict.Add(inputNameNodeWork.InputName, node);
                } else {
                    throw new Exception(GetDuplicateNameErrorMessage(inputNameNodeWork.InputName, node.Id, node.CurrWork.Label));
                }
            }
            if (node.CurrWork is CustomFlowWork custom) {
                for (int i = 0; i < custom.Names.Count; i++) {
                    var name = custom.Names[i];
                    if (_tempNames.Add(name)) {
                        _scriptDict.Add(name, custom);
                        _outputNumDict.Add(name, node);
                    } else {
                        throw new Exception(GetDuplicateNameErrorMessage(name, node.Id, node.CurrWork.Label));
                    }
                }
            }
            _tempdictCount[node.CurrWork.Id] = _tempdict.Count;
        }
        internal string GetDuplicateNameErrorMessage(string name, string id, string label)
        {
            if (_initDict.ContainsKey(name)) {
                return $"变量名[{name}]使用两次，{label}[{id}]与[初始值]名称相同 ";
            }
            if (_inputDict.ContainsKey(name)) {
                return $"变量名[{name}]使用两次，{label}[{id}]与[输入项]名称相同 ";
            }
            if (_progDict.ContainsKey(name)) {
                var id2 = _progDict[name].NodeWork.Id;
                var label2 = _progDict[name].NodeWork.Label;
                return $"变量名[{name}]使用两次，{label}[{id}]与{label2}[{id2}]内的名称相同 ";
            }
            if (_inputNumDict.ContainsKey(name)) {
                var id2 = _inputNumDict[name].Id;
                var label2 = _inputNumDict[name].CurrWork.Label;
                return $"变量名[{name}]使用两次，{label}[{id}]与{label2}[{id2}]内的[入量名称]相同 ";
            }
            if (_scriptDict.ContainsKey(name)) {
                var id2 = _scriptDict[name].Id;
                var label2 = _scriptDict[name].Label;
                return $"变量名[{name}]使用两次，{label}[{id}]与{label2}[{id2}]内的脚本内变量名称相同 ";
            }
            return $"变量名[{name}]使用两次，id:{id}";
        }
        private void RemoveSettingFormulaToDict(TreeNode node)
        {
            if (node.CurrWork is ISettingFormulaNodeWork work) {
                for (int i = 0; i < work.SettingFormula.Count; i++) {
                    var item = work.SettingFormula[i];
                    _tempNames.Remove(item.Name);
                    _progDict.Remove(item.Name);
                    _outputNumDict.Remove(item.Name);
                    _tempdict.Remove(item.Name);
                }
            }
            if (node.CurrWork is IInputNameNodeWork inputNameNodeWork) {
                _tempNames.Remove(inputNameNodeWork.InputName);
                _inputNumDict.Remove(inputNameNodeWork.InputName);
            }
            if (node.CurrWork is CustomFlowWork custom) {
                for (int i = 0; i < custom.Names.Count; i++) {
                    var name = custom.Names[i];
                    _tempNames.Remove(name);
                    _scriptDict.Remove(name);
                    _outputNumDict.Remove(name);
                }
            }
            if (_tempdictCount.ContainsKey(node.CurrWork.Id)) {
                var keys = _tempdict.Keys.ToList();
                while (keys.Count > _tempdictCount[node.CurrWork.Id]) {
                    var key = keys.Last();
                    keys.RemoveAt(keys.Count - 1);
                    _tempdict.Remove(key);
                }
            }
        }
		#endregion

		#region EvaluateInputNum 计算入量
		/// <summary>
		/// 计算入量
		/// </summary>
		public void EvaluateInputNum()
        {
            var keys = _inputNumDict.Keys.ToList();

            for (int i = keys.Count - 1; i >= 0; i--) {
                var key = keys[i];
                var treeNode = _inputNumDict[key];
                treeNode.EvaluateInputNum(this);
                if (treeNode.CurrWork is IInputNameNodeWork work) {
                    if (string.IsNullOrEmpty(work.InputName) == false) {
                        _tempdict[work.InputName] = Operand.Create(treeNode.InputNum.Value);
                    }
                }
            }
            Start.EvaluateInputNum(this);
        }
        internal Operand GetNum()
        {
            return _inputDict["数量"];
        }
        private Stack<Operand> _tempOutputs = new Stack<Operand>();
        internal void SetOutputNum(Operand operand)
        {
            if (_tempdict.TryGetValue("出量", out Operand op)) {
                _tempOutputs.Push(op);
            }
            _tempdict["出量"] = operand;
        }
        internal void ClearOutputNum()
        {
            if (_tempOutputs.TryPop(out Operand operand)) {
                _tempdict["出量"] = operand;
            } else {
                _tempdict["出量"] = _inputDict["数量"];
            }
        }
        internal Operand GetOutputNum()
        {
            if (_tempdict.TryGetValue("出量", out Operand op)) {
                return op;
            }
            return _inputDict["数量"];
        }

        #endregion

        #region Parse Evaluate
        /// <summary>
        /// 计算公式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public double TryEvaluate(string exp, double def)
        {
            try {
                var obj = Evaluate(exp);
                obj = obj.ToNumber();
                if (obj.IsError) {
                    return def;
                }
                return obj.NumberValue;
            } catch (Exception) {
            }
            return def;
        }
		/// <summary>
		/// 计算公式
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public bool TryEvaluate(string exp, bool def)
        {
            try {
                var obj = Evaluate(exp);
                obj = obj.ToBoolean();
                if (obj.IsError) {
                    return def;
                }
                return obj.BooleanValue;
            } catch (Exception) {
            }
            return def;
        }
		/// <summary>
		/// 计算公式
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public DateTime TryEvaluate(string exp, DateTime def)
        {
            try {
                var obj = Evaluate(exp);
                obj = obj.ToBoolean();
                if (obj.IsError) {
                    return def;
                }
                return obj.DateValue;
            } catch (Exception) {
            }
            return def;
        }
		/// <summary>
		/// 计算公式
		/// </summary>
		/// <param name="exp"></param>
		/// <param name="def"></param>
		/// <returns></returns>
		public string TryEvaluate(string exp, string def)
        {
            try {
                var obj = Evaluate(exp);
                obj = obj.ToText();
                if (obj.IsError) {
                    return def;
                }
                return obj.TextValue;
            } catch (Exception) {
            }
            return def;
        }
        internal Operand Evaluate(string exp)
        {
            if (string.IsNullOrWhiteSpace(exp)) {
                return null;
            }
            var stream = new AntlrCharStream(new AntlrInputStream(exp));
            var lexer = new mathLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new mathParser(tokens);
            var antlrErrorListener = new AntlrErrorListener();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(antlrErrorListener);

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

            var visitor = new MathVisitor();
            visitor.GetParameter += GetTempParameter;
            visitor.excelIndex = Project.ExcelIndex;
            var result = visitor.Visit(context);
            visitor = null;
            return result;
        }
        internal bool TryEvaluate(ProgContext exp, bool def)
        {
            try {
                var obj = Evaluate(exp);
                obj = obj.ToBoolean();
                if (obj.IsError) {
                    return def;
                }
                return obj.BooleanValue;
            } catch (Exception) {
            }
            return def;
        }
        private Operand Evaluate(ProgContext context)
        {
            var visitor = new MathVisitor();
            visitor.GetParameter += GetTempParameter;
            visitor.excelIndex = Project.ExcelIndex;
            var result = visitor.Visit(context);
            visitor = null;
            return result;
        }

        #endregion

        #region CheckMachine 检测机器是否可以使用
        internal bool CheckMachine(string categoryCode, string code)
        {
            if (MachineSetting == null || MachineSetting.Count == 0) { return true; }
            var key = categoryCode + '|' + code;
            if (MachineSetting.TryGetValue(key, out Setting_Machine machine)) {
                if (machine.IsStop) {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region EvaluateFormula
        internal Operand EvaluateFormula(ProgContext progContext, int inputType)
        {
            var operand = Evaluate(progContext);

            if (inputType == (int)InputType.Bool) {
                return operand.ToBoolean();
            } else if (inputType == (int)InputType.Number) {
                return operand.ToNumber();
            } else if (inputType == (int)InputType.String) {
                return operand.ToText();
            } else if (inputType == (int)InputType.List) {
                return operand.ToArray();
            } else if (inputType == (int)InputType.Date) {
                return operand.ToMyDate();
            }
            return null;
        }


        internal Operand GetTempParameter(string parameter)
        {
            if (_initDict.TryGetValue(parameter, out Operand operand4)) { // 初始值
                return operand4;
            }
            if (_inputDict.TryGetValue(parameter, out Operand operand)) { // 输入值
                return operand;
            }
            if (_tempdict.TryGetValue(parameter, out Operand operand2)) { // 临时变量
                return operand2;
            }
            if (_progDict.TryGetValue(parameter, out SettingFormulaWork work)) { // 临时公式
                Operand result;
                if (_outputNumDict.TryGetValue(parameter, out TreeNode treeNode)) {
                    SetOutputNum(treeNode.OutputNum); //设置出量
                    result = work.EvaluateFormula(this);
                    _tempdict[parameter] = result;
                    ClearOutputNum();// 清除出量
                } else {
                    result = work.EvaluateFormula(this);
                    _tempdict[parameter] = result;
                }
                return result;
            }
            if (Project.TryGetFormula(parameter, out ProgContext context)) { // 项目公式
                Operand result = Evaluate(context);
                _tempdict[parameter] = result;
                return result;
            }
            if (_scriptDict.TryGetValue(parameter, out CustomFlowWork script)) { //运行脚本
                try {
                    if (_outputNumDict.TryGetValue(parameter, out TreeNode treeNode)) {
                        SetOutputNum(treeNode.OutputNum); //设置出量
                        EvaluateJs(script.Script); //脚本运行后会保存到临时变量内
                        ClearOutputNum();// 清除出量
                    } else {
                        EvaluateJs(script.Script); //脚本运行后会保存到临时变量内
                    }
                    if (_tempdict.TryGetValue(parameter, out Operand operand3)) { return operand3; }
                } catch (Exception ex) {
                    return Operand.Error($"[{script.Label}]脚本:[{parameter}]" + ex.Message);
                }
            }

            if (_startFormulaDict.TryGetValue(parameter, out SettingFormulaWork startWork)) {
                Operand result = startWork.EvaluateFormula(this);
                _tempdict[parameter] = result;
                return result;
            }
            return Operand.Error($"{parameter}的公式未找到！");
        }

        private void EvaluateJs(string script)
        {
            var engine = new Engine()
                .SetValue("getDatas", new Func<object>(js_getDatas))
                .SetValue("hasKey", new Func<string, bool>(js_hasKey))
                .SetValue("setValue", new Action<string, object>(js_setValue))
                .SetValue("getValue", new Func<string, object>(js_getValue))
                .SetValue("error", new Action<string>(js_Error));

            engine.Evaluate(script);
            engine.Dispose();
            engine = null;
        }

        private void js_Error(string msg)
        {
            throw new Exception(msg);
        }

        private string js_getDatas()
        {
            return AttachData;
        }
        private void js_setValue(string name, object value)
        {
            if (value == null) {
                _tempdict.Add(name, Operand.CreateNull());
            } else if (value is int numInt) {
                _tempdict.Add(name, Operand.Create(numInt));
            } else if (value is double numDouble) {
                _tempdict.Add(name, Operand.Create(numDouble));
            } else if (value is bool numBool) {
                _tempdict.Add(name, Operand.Create(numBool));
            } else if (value is string str) {
                _tempdict.Add(name, Operand.Create(str));
            } else if (value is DateTime date) {
                _tempdict.Add(name, Operand.Create(date));
            } else {
                throw new Exception("setValue is error!");
            }
        }
        private object js_getValue(string name)
        {
            var value = GetTempParameter(name);
            if (value.Type == OperandType.BOOLEAN) {
                return value.BooleanValue;
            } else if (value.Type == OperandType.TEXT) {
                return value.TextValue;
            } else if (value.Type == OperandType.DATE) {
                return (DateTime)value.DateValue;
            } else if (value.Type == OperandType.NUMBER) {
                return value.NumberValue;
            } else if (value.Type == OperandType.NULL) {
                return null;
            } else if (value.Type == OperandType.ARRARY) {
                var list = new object[value.ArrayValue.Count];
                for (int i = 0; i < value.ArrayValue.Count; i++) {
                    var op = value.ArrayValue[i];
                    if (op.Type == OperandType.BOOLEAN) {
                        list[i] = value.BooleanValue;
                    } else if (op.Type == OperandType.TEXT) {
                        list[i] = value.TextValue;
                    } else if (op.Type == OperandType.DATE) {
                        list[i] = (DateTime)value.DateValue;
                    } else if (op.Type == OperandType.NUMBER) {
                        list[i] = value.NumberValue;
                    } else if (op.Type == OperandType.NULL) {
                        list[i] = null;
                    } else {
                        list[i] = value.TextValue;
                    }
                }
                return list;
            }
            throw new Exception("getValue is error!");
        }
        private bool js_hasKey(string name)
        {
            if (_initDict.ContainsKey(name)) {
                return true;
            } else if (_inputDict.ContainsKey(name)) {
                return true;
            } else if (_tempdict.ContainsKey(name)) {
                return true;
            } else if (_progDict.ContainsKey(name)) {
                return true;
            } else if (Project.TryGetFormula(name, out _)) {
                return true;
            }
            return false;
        }
        #endregion

        public void Dispose()
        {
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
}
