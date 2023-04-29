using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class TreeNode
	{
		public List<ChannelNode> PrevNodes { get; private set; }
		public Dictionary<string, TreeNode> NextNodes { get; private set; }

		public NodeWork CurrWork { get; private set; }
		public FactoryMachineWork MachineItem { get; private set; }
		public FactoryProcedureItemWork ProcedureItem { get; private set; }

		public string Id { get { return CurrWork.Id; } }
		public int Layer { get { return CurrWork.Layer; } }
		public bool IsSubsidiaryCount { get { if (CurrWork is IInputFormulaNodeWork work) { return work.IsSubsidiaryCount; } return false; } }
		public decimal? InputNum { get; set; }
		public decimal? OutputNum { get; set; }

		public TreeNode(NodeWork work, FactoryMachineWork factoryMachineItem, FactoryProcedureItemWork factoryProcedureItem)
		{
			CurrWork = work;
			MachineItem = factoryMachineItem;
			ProcedureItem = factoryProcedureItem;
			PrevNodes = new List<ChannelNode>();
			NextNodes = new Dictionary<string, TreeNode>();
		}

		public void AddNextNode(TreeNode node, string channel, int index)
		{
			NextNodes.Add(channel, node);
			node.PrevNodes.Add(new ChannelNode {
				Channel = channel,
				Index = index,
				Node = this
			});
		}

		public void SetError(string channel)
		{
			for (int i = 0; i < PrevNodes.Count; i++) {
				var prevNode = PrevNodes[i];
				prevNode.Node.NextNodes.Remove(channel);
			}
		}

		#region 计算入量

		public void EvaluateInputNum(FlowEngine engine)
		{
			if (InputNum != null) { return; }
			if (CurrWork.NodeType == CellType.End) {
				OutputNum = (decimal)engine.GetNum().NumberValue;
				if (CurrWork is ProcedureFlowWork procedure) {
					OutputNum = GetNumber(procedure.NumberType, OutputNum.Value);
				}
				InputNum = OutputNum;
				return;
			}
			if (OutputNum == null) {
				decimal num = 0;
				foreach (var nextNode in NextNodes) {
					nextNode.Value.EvaluateInputNum(engine);
					if (nextNode.Value.IsSubsidiaryCount == false) {
						if (num < nextNode.Value.InputNum.Value) {
							num = nextNode.Value.InputNum.Value;
						}
					}
				}
				OutputNum = num;
			}
			engine.SetOutputNum(OutputNum.Value);
			if (InputNum == null) {
				if (CurrWork is IInputFormulaNodeWork work) {
					InputNum = work.EvaluateInputNum(engine);
				} else {
					InputNum = OutputNum;
				}
				if (CurrWork is ProcedureFlowWork procedure) {
					InputNum = GetNumber(procedure.NumberType, InputNum.Value);
				}
			}
			engine.ClearOutputNum();
		}

		private decimal GetNumber(InputNumberType numberType, decimal n)
		{
			switch (numberType) {
				case InputNumberType.Original: return n;
				case InputNumberType.Ceil: return Math.Ceiling(n);
				case InputNumberType.Floor: return Math.Floor(n);
				case InputNumberType.Round0: return Math.Round(n, 0, MidpointRounding.AwayFromZero);
				case InputNumberType.Round1: return Math.Round(n, 1, MidpointRounding.AwayFromZero);
				case InputNumberType.Round2: return Math.Round(n, 2, MidpointRounding.AwayFromZero);
				case InputNumberType.Round3: return Math.Round(n, 3, MidpointRounding.AwayFromZero);
				case InputNumberType.Round4: return Math.Round(n, 4, MidpointRounding.AwayFromZero);
				case InputNumberType.Round5: return Math.Round(n, 5, MidpointRounding.AwayFromZero);
				case InputNumberType.Round6: return Math.Round(n, 6, MidpointRounding.AwayFromZero);
				case InputNumberType.Round7: return Math.Round(n, 7, MidpointRounding.AwayFromZero);
				case InputNumberType.Round8: return Math.Round(n, 8, MidpointRounding.AwayFromZero);
				default: return n;
			}
		}

		#endregion 计算入量
	}

	public sealed class StockNode
	{
		private List<ChannelNode> Nodes = new List<ChannelNode>();
		private HashSet<string> IdSet = new HashSet<string>();

		public void Push(TreeNode node, string channel, int index)
		{
			if (IdSet.Add(node.CurrWork.Id)) {// 防止合并
				Nodes.Add(new ChannelNode {
					Channel = channel,
					Index = index,
					Node = node
				});
			}
		}

		public bool TryPop(out ChannelNode node)
		{
			if (Nodes.Count == 0) { node = null; return false; }

			ChannelNode temp = Nodes[0];
			for (int i = 1; i < Nodes.Count; i++) {
				var channelNode = Nodes[i];
				if (temp != null) {
					if (channelNode.Node.Layer < temp.Node.Layer) {
						temp = channelNode;
					}
				} else {
					temp = channelNode;
				}
			}
			node = temp;
			if (temp != null) {
				Nodes.Remove(temp);
				IdSet.Remove(temp.Node.Id);
				return true;
			}
			return false;
		}
	}

	/// <summary>
	/// Channel，Index 为下个节点所在位置
	/// 如查将 Channel，Index 放在 TreeNode ，多条工艺渠道合并就会出错
	/// </summary>
	public sealed class ChannelNode
	{
		public TreeNode Node;
		public string Channel;
		public int Index;
	}
}