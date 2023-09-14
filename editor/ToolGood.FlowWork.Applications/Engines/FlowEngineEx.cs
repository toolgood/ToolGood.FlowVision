using ToolGood.Algorithm2;
using ToolGood.Algorithm2.Enums;
using ToolGood.Algorithm2.Internals;
using ToolGood.FlowWork.Applications.Engines.OutDatas;
using ToolGood.FlowWork.Applications.Engines.Parameters;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Applications.Engines
{
	public sealed class FlowEngineEx : FlowEngine
	{
		public FlowEngineEx(ProjectWork project, string attachData = null, List<Setting_Machine> machines = null) : base(project, attachData, machines)
		{
		}

		#region GetFormulaDetails 获取公式计算详情

		public FormulaDetail GetFormulaDetails(string ps = null)
		{
			FormulaDetail result = new FormulaDetail();
			result.Exps = new List<FormulaDetailItem>();

			Dictionary<string, FormulaDetailItem> temp = new Dictionary<string, FormulaDetailItem>();
			if (string.IsNullOrWhiteSpace(ps) == false) {
				List<IFormulaItem> list2 = new List<IFormulaItem>();
				var pps = ps.Split(',', StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < pps.Length; i++) {
					var p = pps[i];
					FormulaItem formulaItem = new FormulaItem();
					formulaItem.Name = p;
					var operand2 = GetTempParameter(p);
					if (operand2.IsError) {
						formulaItem.Value = $"error(\"{operand2.ErrorMsg}\")";
					} else if (operand2.Type == OperandType.TEXT) {
						formulaItem.Value = $"\"{operand2.TextValue}\"";
					} else {
						formulaItem.Value = operand2.ToText().TextValue;
					}
					list2.Add(formulaItem);
				}
				GetFormulaItems2(list2, temp, 1);
			}

			EvalLayer(temp);
			result.Items = temp.Values.OrderBy(q => q.Layer).ToList();
			return result;
		}

		public FormulaDetail GetFormulaDetails(string name, string formula, string ps = null)
		{
			FormulaDetail result = new FormulaDetail();
			result.Exps = new List<FormulaDetailItem>();
			FormulaDetailItem item = new FormulaDetailItem();
			result.Exps.Add(item);

			item.Name = name;
			var operand = Evaluate(formula);
			item.Value = operand.IsError ? operand.ErrorMsg : operand.ToText().TextValue;
			item.Formulas = GetFormulaItems(formula);

			Dictionary<string, FormulaDetailItem> temp = new Dictionary<string, FormulaDetailItem>();
			if (string.IsNullOrWhiteSpace(ps) == false) {
				List<IFormulaItem> list2 = new List<IFormulaItem>();
				list2.AddRange(item.Formulas);
				var pps = ps.Split(',', StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < pps.Length; i++) {
					var p = pps[i];
					FormulaItem formulaItem = new FormulaItem();
					formulaItem.Name = p;
					var operand2 = GetTempParameter(p);
					if (operand2.IsError) {
						formulaItem.Value = $"error(\"{operand2.ErrorMsg}\")";
					} else if (operand2.Type == OperandType.TEXT) {
						formulaItem.Value = $"\"{operand2.TextValue}\"";
					} else {
						formulaItem.Value = operand2.ToText().TextValue;
					}
					list2.Add(formulaItem);
				}
				GetFormulaItems2(list2, temp, 1);
			} else {
				GetFormulaItems2(item.Formulas, temp, 1);
			}
			EvalLayer(temp);
			result.Items = temp.Values.OrderBy(q => q.Layer).ToList();
			return result;
		}

		public FormulaDetail GetFormulaDetails(Dictionary<string, string> dict)
		{
			FormulaDetail result = new FormulaDetail();
			result.Exps = new List<FormulaDetailItem>();

			List<IFormulaItem> findFormulas = new List<IFormulaItem>();
			foreach (var keyValue in dict) {
				FormulaDetailItem item = new FormulaDetailItem();
				item.Name = keyValue.Key;
				var operand = Evaluate(keyValue.Value);
				if (operand.IsError) {
					item.Value = operand.ErrorMsg;
				} else if (operand.Type == OperandType.TEXT) {
					item.Value = $"\"{operand.TextValue}\"";
				} else {
					item.Value = operand.ToText().TextValue;
				}
				item.Formulas = GetFormulaItems(keyValue.Value);
				result.Exps.Add(item);
				findFormulas.AddRange(item.Formulas);
			}

			Dictionary<string, FormulaDetailItem> temp = new Dictionary<string, FormulaDetailItem>();
			GetFormulaItems2(findFormulas, temp, 1);
			EvalLayer(temp);
			result.Items = temp.Values.OrderBy(q => q.Layer).ToList();
			return result;
		}

		private void EvalLayer(Dictionary<string, FormulaDetailItem> temp)
		{
			HashSet<FormulaDetailItem> items = temp.Select(q => q.Value).ToHashSet();

			while (items.Count > 0) {
				HashSet<FormulaDetailItem> nextItems = new HashSet<FormulaDetailItem>();
				foreach (var item in items) {
					var formulas = item.Formulas;
					if (formulas == null) { continue; }
					for (int i = 0; i < formulas.Count; i++) {
						if (formulas[i] is FormulaItem formula) {
							if (temp.TryGetValue(formula.Name, out FormulaDetailItem formulaDetailItem)) {
								if (formulaDetailItem.Layer < item.Layer + 1) {
									formulaDetailItem.Layer = item.Layer + 1;
									nextItems.Add(formulaDetailItem);
								}
							}
						}
					}
				}
				items = nextItems;
			}
		}

		#endregion GetFormulaDetails 获取公式计算详情

		#region GetErrorMessage 获取程序完整错误信息

		public ErrorNodeInfo GetErrorMessage(string appCode, string factoryCode, string json, string previous)
		{
			var app = BindingJson(appCode, factoryCode, json);
			BindingPreviousJson(previous);

			_tempNames = new HashSet<string>();
			_tempdictCount = new Dictionary<string, int>();

			StockErrorNode stock = new StockErrorNode();
			var start = new TreeErrorNode(app.Start);
			AddSettingFormulaToDict(start);
			stock.Push(start, null, 0);
			string errorMessage = null;
			Dictionary<string, TreeErrorNode> temp = new Dictionary<string, TreeErrorNode>();
			Dictionary<string, TreeErrorNode> errors = new Dictionary<string, TreeErrorNode>();

			bool isError = true;
			while (stock.TryPop(out ChannelErrorNode channelNode)) {
				var currTreeNode = channelNode.Node;
				var work = currTreeNode.CurrWork;
				var channel = channelNode.Channel;

				// 初始 渠道
				if (channel == null) { var channels = work.GetChannels(); for (int i = 0; i < channels.Count; i++) { stock.Push(currTreeNode, channels[i], 0); } continue; }

				//没有下一个节点   work.NextNodes == null || work.NextNodes.Count == 0 || work.NextNodes[channel].Count == 0 ||
				if (channelNode.Index >= work.NextNodes[channel].Count) {
					currTreeNode.SetError();
					for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); }
					RemoveSettingFormulaToDict(currTreeNode);
					errors[currTreeNode.Id] = currTreeNode;
					continue;
				}
				var arr = work.NextNodes[channel];
				var node = arr[channelNode.Index];
				if (errors.TryGetValue(node.Id, out TreeErrorNode treeError)) {
					if (currTreeNode.CurrWork is ErrorFlowWork) { // 错误节点
						currTreeNode.SetError();
						currTreeNode.AddNextNode(treeError, channel);
						for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); errors[prev.Node.Id] = prev.Node; }
						RemoveSettingFormulaToDict(currTreeNode);
					} else { // 普通节点未匹配
						stock.Push(currTreeNode, channel, channelNode.Index + 1);
					}
					continue;
				}
				if (node is ProcedureFlowWork procedureWork) { // 工艺
					if (procedureWork.CheckWithoutItems(this, factoryCode)) {
						TreeErrorNode procedureWorkNode;
						if (temp.TryGetValue(node.Id, out procedureWorkNode) == false) {
							procedureWorkNode = new TreeErrorNode(node);
							temp[node.Id] = procedureWorkNode;
						}
						currTreeNode.AddNextNode(procedureWorkNode, channel);
						if (procedureWork.FactoryProcedure.Items.TryGetValue(factoryCode, out FactoryProcedureItemWork factoryProcedureItemWork)) {
							if (procedureWork.Machines.TryGetValue(factoryCode, out List<ProcedureFlowMachineInfo> machines)) {
								bool match = false;
								for (int i = 0; i < machines.Count; i++) {
									var machine = machines[i];

									TreeErrorNode procedureWorkItemNode;
									if (temp.TryGetValue(node.Id + i.ToString(), out procedureWorkItemNode) == false) {
										var label = machine.FactoryMachine.MachineName;
										if (string.IsNullOrEmpty(machine.FactoryMachine.MachineCategoryName) == false) {
											label += '\n' + machine.FactoryMachine.MachineCategoryName;
										}
										procedureWorkItemNode = new TreeErrorNode(node, "procedureItem", label, node.Id + i.ToString());
										temp[node.Id + i.ToString()] = procedureWorkItemNode;
									}
									procedureWorkNode.AddNextNode(procedureWorkItemNode, channel);
									if (CheckMachine(machine.FactoryMachine.MachineCategoryCode, machine.FactoryMachine.MachineCode) && machine.Check(this)) {
										match = true;
										procedureWorkItemNode.SetPrevNode(currTreeNode, channel, channelNode.Index);
										AddSettingFormulaToDict(procedureWorkItemNode);
										stock.Push(procedureWorkItemNode, null, -1);// 下一个节点
										break;
									} else {
										procedureWorkItemNode.SetError(machine.GetErrorMessage(this));
									}
								}
								if (match) { continue; }
							}
							if (procedureWork.MachineRequired == false) {
								procedureWorkNode.SetPrevNode(currTreeNode, channel, channelNode.Index);
								AddSettingFormulaToDict(procedureWorkNode);
								stock.Push(procedureWorkNode, null, -1);// 下一个节点
								continue;
							}
							procedureWorkNode.SetError("厂区没有对应机械", true);
						} else {
							procedureWorkNode.SetError("厂区没有对应工艺", true);
						}
					}
				} else if (node is ErrorFlowWork error) {
					if (error.Check(this, factoryCode)) {
						if (errorMessage == null) { errorMessage = error.ErrorMessage; }
						TreeErrorNode errorNode = new TreeErrorNode(node);
						errorNode.SetError(error.ErrorMessage, true);
						currTreeNode.AddNextNode(errorNode, channel);
						for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); errors[prev.Node.Id] = prev.Node; }
						RemoveSettingFormulaToDict(currTreeNode);
						errors[error.Id] = errorNode;
						continue;
					}
					stock.Push(currTreeNode, channel, channelNode.Index + 1);
					continue;
				} else if (node.Check(this, factoryCode)) {
					// 防合并 bug
					TreeErrorNode treeNode;
					if (temp.TryGetValue(node.Id, out treeNode) == false) {
						treeNode = new TreeErrorNode(node);
						temp.Add(node.Id, treeNode);
					}
					currTreeNode.AddNextNode(treeNode, channel);
					treeNode.SetPrevNode(currTreeNode, channel, channelNode.Index);
					AddSettingFormulaToDict(treeNode);
					if (node.NodeType == CellType.End) { isError = false; continue; }// 结束
					stock.Push(treeNode, null, -1);// 下一个节点
					continue;
				} else if (node is StatusFlowWork status) { // 必须在 nextNode.Check(this, factoryCode) 之后
					if (status.CheckStatus(this)) {
						TreeErrorNode treeNode;
						if (temp.TryGetValue(node.Id, out treeNode) == false) {
							treeNode = new TreeErrorNode(node);
							temp.Add(node.Id, treeNode);
						}
						currTreeNode.AddNextNode(treeNode, channel);
						treeNode.SetPrevNode(currTreeNode, channel, channelNode.Index);
						//AddSettingFormulaToDict(treeNode); //通过之后
						continue;
					}
				}
				TreeErrorNode errorNode2 = new TreeErrorNode(node);
				errorNode2.SetError(node.GetErrorMessage(this, factoryCode));
				currTreeNode.AddNextNode(errorNode2, channel);
				errors[errorNode2.Id] = errorNode2;
				// 未匹配 匹配下一个
				stock.Push(currTreeNode, channel, channelNode.Index + 1);
			}
			stock = null;
			temp = null;
			errors = null;
			_tempNames.Clear();
			_tempNames = null;
			_tempdictCount.Clear();
			_tempdictCount = null;

			ErrorNodeInfo errorNodeInfo = new ErrorNodeInfo();
			if (isError) { errorNodeInfo.IsError = true; }
			errorNodeInfo.Nodes = start.GetTreeNodeAll().ToList();
			errorNodeInfo.Parameters = new HashSet<string>();
			if (errorNodeInfo.Nodes.Count > 0) {
				errorNodeInfo.Start = errorNodeInfo.Nodes[0].Id;
				for (int i = 0; i < errorNodeInfo.Nodes.Count; i++) {
					var node = errorNodeInfo.Nodes[i];
					if (node.NextNodes == null) { continue; }
					var nextLayer = node.Layer + 1;
					foreach (var next in node.NextNodes) {
						for (int j = 0; j < next.Value.Count; j++) {
							var item = next.Value[j];
							item.SetMaxLayer(nextLayer);
						}
					}
				}
				for (int i = 0; i < errorNodeInfo.Nodes.Count; i++) {
					var node = errorNodeInfo.Nodes[i];
					if (node.ErrorMessage != null && node.IsErrorNode == false) {
						var diyNameInfo = AlgorithmEngineHelper.GetDiyNames(node.ErrorMessage);
						for (int j = 0; j < diyNameInfo.Parameters.Count; j++) {
							var item = diyNameInfo.Parameters[j];
							errorNodeInfo.Parameters.Add(item.Name);
						}
					}
				}

				int[] orders = new int[errorNodeInfo.Nodes.Count];
				for (int i = 0; i < errorNodeInfo.Nodes.Count; i++) {
					var node = errorNodeInfo.Nodes[i];
					node.Order = orders[node.Layer]++;
				}
				orders = null;
			}
			return errorNodeInfo;
		}

		internal string FindErrorCondition(string exp)
		{
			if (string.IsNullOrEmpty(exp)) { return null; }
			var tree = ConditionTree.Parse(exp);
			return FindErrorCondition(tree);
		}

		private string FindErrorCondition(ConditionTree tree)
		{
			if (tree.Type == ConditionTreeType.String) {
				var prog = Project.CreateProgContext(tree.ConditionString);
				if (TryEvaluate(prog, false) == false) {
					return tree.ConditionString;
				}
				return null;
			} else if (tree.Type == ConditionTreeType.And) {
				var left = FindErrorCondition(tree.Nodes[0]);
				if (left != null) { return left; }
				return FindErrorCondition(tree.Nodes[1]);
			} else if (tree.Type == ConditionTreeType.Or) {
				var left = FindErrorCondition(tree.Nodes[0]);
				if (left == null) { return null; }
				var right = FindErrorCondition(tree.Nodes[1]);
				if (right == null) { return null; }
				return left + " || " + right;
			}
			return "Condition is error!";
		}

		private void AddSettingFormulaToDict(TreeErrorNode node)
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
					} else {
						throw new Exception(GetDuplicateNameErrorMessage(item.Name, node.Id, node.CurrWork.Label));
					}
				}
			}
			if (node.CurrWork is IInputNameNodeWork inputNameNodeWork) {
				if (string.IsNullOrEmpty(inputNameNodeWork.InputName)) {
				} else if (_tempNames.Add(inputNameNodeWork.InputName)) {
					// 错误节点不用计算数量
					// _inputNumDict.Add(inputNameNodeWork.InputName, node.CurrWork);
				} else {
					throw new Exception(GetDuplicateNameErrorMessage(inputNameNodeWork.InputName, node.Id, node.CurrWork.Label));
				}
			}
			if (node.CurrWork is CustomFlowWork custom) {
				for (int i = 0; i < custom.Names.Count; i++) {
					var name = custom.Names[i];
					if (_tempNames.Add(name)) {
						_scriptDict.Add(name, custom);
					} else {
						throw new Exception(GetDuplicateNameErrorMessage(name, node.Id, node.CurrWork.Label));
					}
				}
			}
			_tempdictCount[node.CurrWork.Id] = _tempdict.Count;
		}

		private void RemoveSettingFormulaToDict(TreeErrorNode node)
		{
			if (node.CurrWork is ISettingFormulaNodeWork work) {
				for (int i = 0; i < work.SettingFormula.Count; i++) {
					var item = work.SettingFormula[i];
					_tempNames.Remove(item.Name);
					_progDict.Remove(item.Name);
					_tempdict.Remove(item.Name);
				}
			}
			if (node.CurrWork is IInputNameNodeWork inputNameNodeWork) {
				_tempNames.Remove(inputNameNodeWork.InputName);
				//_inputNumDict.Remove(inputNameNodeWork.InputName);
			}
			if (node.CurrWork is CustomFlowWork custom) {
				for (int i = 0; i < custom.Names.Count; i++) {
					var name = custom.Names[i];
					_tempNames.Remove(name);
					_scriptDict.Remove(name);
				}
			}
			var keys = _tempdict.Keys.ToList();
			if (_tempdictCount.ContainsKey(node.CurrWork.Id)) {
				while (keys.Count > _tempdictCount[node.CurrWork.Id]) {
					var key = keys.Last();
					keys.RemoveAt(keys.Count - 1);
					_tempdict.Remove(key);
				}
			}
		}

		#endregion GetErrorMessage 获取程序完整错误信息

		#region BuildTreeNodeWithError

		public void BuildTreeNodeWithError(string appCode, string factoryCode, string json, string previous)
		{
			var app = BindingJson(appCode, factoryCode, json);
			BindingPreviousJson(previous);

			_tempNames = new HashSet<string>();

			StockNode stock = new StockNode();
			Start = new TreeNode(app.Start, null, null);
			AddSettingFormulaToDictWithError(Start);
			stock.Push(Start, null, 0);
			int errorLayer = -1;
			string errorMessage = null;
			HashSet<string> errorIds = new HashSet<string>();
			Dictionary<string, TreeNode> temp = new Dictionary<string, TreeNode>();

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
					errorIds.Add(work.Id);
					continue;
				}
				var arr = work.NextNodes[channel];
				var nextNode = arr[channelNode.Index];
				if (errorIds.Contains(nextNode.Id)) {
					if (nextNode is ErrorFlowWork) { // 错误，当前设置错误
						currTreeNode.SetError(channel);
						for (int i = 0; i < currTreeNode.PrevNodes.Count; i++) { var prev = currTreeNode.PrevNodes[i]; stock.Push(prev.Node, prev.Channel, prev.Index + 1); errorIds.Add(prev.Node.Id); }
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
										AddSettingFormulaToDictWithError(treeNode);
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
								AddSettingFormulaToDictWithError(treeNode);
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
					AddSettingFormulaToDictWithError(treeNode);
					currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
					if (nextNode.NodeType == CellType.End) { continue; }// 结束
					stock.Push(treeNode, null, -1);// 下一个节点
					continue;
				} else if (nextNode is StatusFlowWork status) { // 必须在 nextNode.Check(this, factoryCode) 之后
					if (status.CheckStatus(this)) {
						if (temp.TryGetValue(nextNode.Id, out treeNode) == false) {
							treeNode = new TreeNode(nextNode, null, null);
							temp[nextNode.Id] = treeNode;
						}
						//AddSettingFormulaToDictWithError(treeNode);
						currTreeNode.AddNextNode(treeNode, channel, channelNode.Index);
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
		}

		private void AddSettingFormulaToDictWithError(TreeNode node)
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
					_progDict[item.Name] = item;
				}
			}
			if (node.CurrWork is IInputNameNodeWork inputNameNodeWork) {
				if (string.IsNullOrEmpty(inputNameNodeWork.InputName)) {
				} else {
					_inputNumDict[inputNameNodeWork.InputName] = node;
				}
			}
			if (node.CurrWork is CustomFlowWork custom) {
				for (int i = 0; i < custom.Names.Count; i++) {
					var name = custom.Names[i];
					_scriptDict[name] = custom;
				}
			}
		}

		#endregion BuildTreeNodeWithError

		#region GetConditionDetails 获取 公式 条件

		public NodeConditionDetail GetNodeConditionDetails(string nodeId, string factoryCode)
		{
			if (AppWork.AllNodeWork.TryGetValue(nodeId, out NodeWork node)) {
				NodeConditionDetail result = new NodeConditionDetail();
				result.Label = node.Label;
				if (node is JumpFlowWork jump) {
					result.Condition = GetConditionItems(jump.CheckFormula);
				} else if (node is CustomFlowWork custom) {
					result.Condition = GetConditionItems(custom.CheckFormula);
				} else if (node is ErrorFlowWork error) {
					result.Condition = GetConditionItems(error.CheckFormula);
				} else if (node is ProcedureFlowWork procedure) {
					if (procedure.FactoryProcedure.Items.TryGetValue(factoryCode, out FactoryProcedureItemWork itemWork)) {
						result.Procedure = new NodeProcedureConditionItem();
						result.Procedure.Name = itemWork.Name;
						result.Procedure.Code = itemWork.Code;
						result.Procedure.CategoryCode = itemWork.Category;
						result.Procedure.Category = itemWork.Category;
					}
					if (procedure.CheckType == CheckType.Add) {
						result.ProcedureCondition = GetConditionItems(procedure.FactoryProcedure.CheckFormula);
					}
					if (procedure.Machines.TryGetValue(factoryCode, out List<ProcedureFlowMachineInfo> list)) {
						if (list.Count == 0) { return result; }
						result.Machines = new List<NodeMachineConditionItem>();
						foreach (var machineInfo in list) {
							NodeMachineConditionItem machineConditionItem = new NodeMachineConditionItem();
							machineConditionItem.Name = machineInfo.FactoryMachine.MachineName;
							machineConditionItem.Code = machineInfo.FactoryMachine.MachineCode;
							machineConditionItem.CategoryCode = machineInfo.FactoryMachine.MachineCategoryCode;
							machineConditionItem.Category = machineInfo.FactoryMachine.MachineCategoryCode;
							machineConditionItem.MachineCondition = GetConditionItems(machineInfo.Condition);
							if (machineInfo.CheckType == CheckType.Add) {
								machineConditionItem.MachineCondition2 = GetConditionItems(machineInfo.FactoryMachine.CheckFormula);
							}
							result.Machines.Add(machineConditionItem);
						}
					}
				}
				return result;
			}
			return null;
		}

		public ConditionDetail GetConditionDetails(string nodeId, string name)
		{
			if (AppWork.AllNodeWork.TryGetValue(nodeId, out NodeWork node)) {
				ConditionDetail result = new ConditionDetail();
				result.Name = name;
				if (node is ISettingFormulaNodeWork formulaNodeWork) {
					for (int i = 0; i < formulaNodeWork.SettingFormula.Count; i++) {
						var settingFormula = formulaNodeWork.SettingFormula[i];
						if (settingFormula.Name == name) {
							result.DefaultFormula = settingFormula.DefaultFormula;
							result.Comment = settingFormula.Comment;
							if (settingFormula.Conditions != null && settingFormula.Conditions.Count > 0) {
								result.Conditions = new List<ConditionDetailItem>();
								for (int j = 0; j < settingFormula.Conditions.Count; j++) {
									var condition = settingFormula.Conditions[j];
									ConditionDetailItem conditionDetailItem = new ConditionDetailItem();
									conditionDetailItem.Formula = condition.Formula;
									conditionDetailItem.Condition = GetConditionItems(condition.Condition);

									if (condition.Check(this)) { conditionDetailItem.Check = true; }
									result.Conditions.Add(conditionDetailItem);
								}
							}
							return result;
						}
					}
				}
				if (node is ProcedureFlowWork procedure) {
					if (name == procedure.InputName) {
						result.Comment = procedure.Comment;
						result.Conditions = new List<ConditionDetailItem>();
						if (procedure.InputFormula != null && procedure.InputFormula.Count > 0) {
							for (int i = 0; i < procedure.InputFormula.Count; i++) {
								var condition = procedure.InputFormula[i];
								ConditionDetailItem conditionDetailItem = new ConditionDetailItem();
								conditionDetailItem.Formula = condition.Formula;
								conditionDetailItem.Condition = GetConditionItems(condition.Condition);
								if (condition.Check(this)) { conditionDetailItem.Check = true; }
								result.Conditions.Add(conditionDetailItem);
							}
						}
						return result;
					}
				}
			}
			return null;
		}

		private List<ConditionItem> GetConditionItems(string formula)
		{
			if (string.IsNullOrEmpty(formula)) { return null; }

			var tree = ConditionTree.Parse(formula);
			List<ConditionTree> conditionTrees = new List<ConditionTree>();
			GetConditionTreeAll(tree, conditionTrees);

			var formulaSpan = formula.AsSpan();
			List<ConditionItem> result = new List<ConditionItem>();
			var index = 0;
			for (int i = 0; i < conditionTrees.Count; i++) {
				var parameter = conditionTrees[i];
				if (index < parameter.Start) {
					ConditionItem textItem = new ConditionItem();
					textItem.Text = formulaSpan.Slice(index, parameter.Start - index).ToString();
					result.Add(textItem);
				}
				ConditionItem formulaItem = new ConditionItem();
				formulaItem.Text = parameter.ConditionString;
				var prog = Project.CreateProgContext(parameter.ConditionString);
				if (TryEvaluate(prog, false) == false) {
					formulaItem.IsError = true;
				}
				result.Add(formulaItem);
				index = parameter.End + 1;
			}
			if (index < formula.Length) {
				ConditionItem textItem = new ConditionItem();
				textItem.Text = formulaSpan.Slice(index).ToString();
				result.Add(textItem);
			}
			return result;
		}

		private void GetConditionTreeAll(ConditionTree tree, List<ConditionTree> conditionTrees)
		{
			if (tree.Type == ConditionTreeType.String) {
				conditionTrees.Add(tree);
			} else if (tree.Type == ConditionTreeType.And || tree.Type == ConditionTreeType.Or) {
				GetConditionTreeAll(tree.Nodes[0], conditionTrees);
				GetConditionTreeAll(tree.Nodes[1], conditionTrees);
			}
		}

		#endregion GetConditionDetails 获取 公式 条件

		#region GetProcedureDetail 获取 机械工艺 及 公式

		public ProcedureDetail GetProcedureDetail(List<FormulaMatch> formulaMatches, bool allFormulas)
		{
			ProcedureDetail result = new ProcedureDetail();
			result.Procedures = new List<ProcedureMachine>();

			Stack<(TreeNode, ProcedureMachine)> stack = new Stack<(TreeNode, ProcedureMachine)>();
			HashSet<string> idSet = new HashSet<string>();
			ProcedureMachine root = new ProcedureMachine();
			result.Procedures.Add(root);
			root.Id = Start.Id;
			root.Type = "start";
			stack.Push((Start, root));

			while (stack.TryPop(out (TreeNode, ProcedureMachine) p)) {
				var treeNode = p.Item1;
				var pm = p.Item2;

				if (treeNode.MachineItem != null) {

					#region 机械->工艺

					ProcedureMachine procedureMachine = new ProcedureMachine();
					result.Procedures.Add(procedureMachine);
					procedureMachine.Type = "procedure";
					procedureMachine.Id = treeNode.Id;
					procedureMachine.Machine = new MachineItem(treeNode.MachineItem);
					pm.AddNodeId(procedureMachine.Id);

					AddFormulaMatches(treeNode, procedureMachine, formulaMatches);
					procedureMachine.ProcedureLabel = treeNode.CurrWork.Label;
					procedureMachine.Procedure = new ProcedureItem(treeNode.ProcedureItem);
					procedureMachine.InputNum = treeNode.InputNum;
					procedureMachine.OutputNum = treeNode.OutputNum;

					if (treeNode.NextNodes != null && treeNode.NextNodes.Count > 0) {
						foreach (var nextNode in treeNode.NextNodes) {
							if (idSet.Add(nextNode.Value.Id)) {//防重复
								stack.Push((nextNode.Value, procedureMachine));
							}
						}
					}

					#endregion 机械->工艺
				} else if (treeNode.ProcedureItem != null) {

					#region 工艺

					ProcedureMachine procedureMachine = new ProcedureMachine();
					result.Procedures.Add(procedureMachine);
					procedureMachine.Type = "procedure";
					procedureMachine.Id = treeNode.Id;
					procedureMachine.ProcedureLabel = treeNode.CurrWork.Label;
					procedureMachine.Procedure = new ProcedureItem(treeNode.ProcedureItem);
					procedureMachine.InputNum = treeNode.InputNum;
					procedureMachine.OutputNum = treeNode.OutputNum;
					pm.AddNodeId(procedureMachine.Id);

					AddFormulaMatches(treeNode, procedureMachine, formulaMatches);

					if (treeNode.NextNodes != null && treeNode.NextNodes.Count > 0) {
						foreach (var nextNode in treeNode.NextNodes) {
							if (idSet.Add(nextNode.Value.Id)) {//防重复
								stack.Push((nextNode.Value, procedureMachine));
							}
						}
					}

					#endregion 工艺
				} else if (treeNode.CurrWork is EndFlowWork) {

					#region 结束节点

					ProcedureMachine procedureMachine = new ProcedureMachine();
					result.Procedures.Add(procedureMachine);
					procedureMachine.Id = Start.Id;
					procedureMachine.Type = "end";
					if (pm.NextNodeIds == null) { pm.NextNodeIds = new List<string>(); }
					pm.NextNodeIds.Add(procedureMachine.Id);

					#endregion 结束节点
				} else {
					if (treeNode.NextNodes != null && treeNode.NextNodes.Count > 0) {
						foreach (var nextNode in treeNode.NextNodes) {
							if (idSet.Add(nextNode.Value.Id)) {//防重复
								stack.Push((nextNode.Value, pm));
							}
						}
					}
				}
			}
			if (formulaMatches == null || formulaMatches.Count == 0 || allFormulas == false) { return result; }

			List<IFormulaItem> formulaItems = new List<IFormulaItem>();
			for (int i = 0; i < result.Procedures.Count; i++) {
				var procedure = result.Procedures[i];
				if (procedure.Exps == null) { continue; }
				foreach (var exp in procedure.Exps) {
					for (int j = 0; j < exp.Value.Count; j++) {
						var item = exp.Value[j];
						formulaItems.AddRange(item.Formulas);
					}
				}
			}

			Dictionary<string, FormulaDetailItem> temp = new Dictionary<string, FormulaDetailItem>();
			GetFormulaItems2(formulaItems, temp, 1);
			result.FormulaItems = temp.Values.OrderBy(q => q.Layer).ToList();
			return result;
		}

		private void AddFormulaMatches(TreeNode treeNode, ProcedureMachine procedureMachine, List<FormulaMatch> formulaMatches)
		{
			if (formulaMatches == null && formulaMatches.Count == 0) { return; }

			var nodework = treeNode.CurrWork;
			if (nodework is ISettingFormulaNodeWork settingFormulaNodeWork) {
				for (int i = 0; i < settingFormulaNodeWork.SettingFormula.Count; i++) {
					var settingFormulaWork = settingFormulaNodeWork.SettingFormula[i];
					for (int j = 0; j < formulaMatches.Count; j++) {
						var formulaMatch = formulaMatches[j];
						if (formulaMatch.IsMatch(settingFormulaWork.Name)) {
							var formula = settingFormulaWork.GetMatchFormula(this);
							FormulaDetailItem formulaDetailItem = new FormulaDetailItem();
							formulaDetailItem.Name = settingFormulaWork.Name;
							formulaDetailItem.Formulas = GetFormulaItems(formula);
							formulaDetailItem.Value = GetTempParameter(settingFormulaWork.Name).ToText().TextValue;
							procedureMachine.AddFormulaDetailItem(formulaMatch.Name, formulaDetailItem);
						}
					}
				}
			}
			if (nodework is IInputFormulaNodeWork inputFormulaNodeWork) {
				if (string.IsNullOrEmpty(inputFormulaNodeWork.InputName) == false) {
					for (int j = 0; j < formulaMatches.Count; j++) {
						var formulaMatch = formulaMatches[j];
						if (formulaMatch.IsMatch(inputFormulaNodeWork.InputName)) {
							var formula = inputFormulaNodeWork.GetMatchFormula(this);
							if (formula == null) { continue; }

							FormulaDetailItem formulaDetailItem = new FormulaDetailItem();
							formulaDetailItem.Name = inputFormulaNodeWork.InputName;
							formulaDetailItem.Formulas = GetFormulaItems(formula);
							formulaDetailItem.Value = GetTempParameter(inputFormulaNodeWork.InputName).ToText().TextValue;
							procedureMachine.AddFormulaDetailItem(formulaMatch.Name, formulaDetailItem);
						}
					}
				}
			}
		}

		#endregion GetProcedureDetail 获取 机械工艺 及 公式

		#region GetOptionalMachine 获取 可选机械

		public List<OptionalMachine> GetOptionalMachine(string factoryCode, string nodeId)
		{
			List<OptionalMachine> result = new List<OptionalMachine>();
			HashSet<NodeWork> temps = new HashSet<NodeWork>();

			if (AppWork.ParentNodeWorks.TryGetValue(nodeId, out List<NodeWork> list)) {
				for (int i = 0; i < list.Count; i++) {
					var item = list[i];
					if (temps.Contains(item) == false) { continue; }
					if (item is ProcedureFlowWork procedure) {
						if (procedure.CheckWithoutFactory(this)) {
							if (procedure.FactoryProcedure.Items.ContainsKey(factoryCode) == false) { continue; }
							if (procedure.Machines.TryGetValue(factoryCode, out List<ProcedureFlowMachineInfo> machineList)) {
								for (int j = 0; j < machineList.Count; j++) {
									var machine = machineList[j];
									if (machine.Check(this)) {
										OptionalMachine optionalMachine = new OptionalMachine() {
											NodeId = item.Id,
											MachineLabel = item.Label,
											Category = machine.FactoryMachine.MachineCategoryName,
											CategoryCode = machine.FactoryMachine.MachineCategoryCode,
											Code = machine.FactoryMachine.MachineCode,
											Name = machine.FactoryMachine.MachineName,
										};
										result.Add(optionalMachine);
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		#endregion GetOptionalMachine 获取 可选机械
	}
}