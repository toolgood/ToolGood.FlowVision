using System.Text.Json.Serialization;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class ErrorNodeInfo
	{
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool? IsError { get; set; }

		public List<TreeErrorNode> Nodes { get; set; }
		public string Start { get; set; }
		public HashSet<string> Parameters { get; set; }
	}

	public sealed class TreeErrorNode
	{
		[JsonIgnore] public List<ChannelErrorNode> PrevNodes;// { get; private set; }
		[JsonIgnore] public Dictionary<string, List<TreeErrorNode>> NextNodes;// { get; private set; }
		[JsonIgnore] public NodeWork CurrWork;// { get; private set; }
		[JsonIgnore] public bool IsErrorNode;// { get; private set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Id { get; set; }

		public string Label { get; set; }
		public string NodeType { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool? IsError { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string ErrorMessage { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Dictionary<string, List<string>> NextNodeIds { get; set; }

		public int Layer { get; set; }
		public int Order { get; set; }

		public TreeErrorNode(NodeWork work)
		{
			CurrWork = work;
			NodeType = work.NodeType.ToString().ToLower();
			Id = work.Id;
			Label = work.Label;
			//Layer = work.Layer;
			PrevNodes = new List<ChannelErrorNode>();
		}

		public TreeErrorNode(NodeWork work, string nodeType, string label, string id)
		{
			Id = id;
			CurrWork = work;
			Label = label;
			NodeType = nodeType;
			//Layer = work.Layer;
			PrevNodes = new List<ChannelErrorNode>();
		}

		public void AddNextNode(TreeErrorNode node, string channel)
		{
			if (NextNodes == null) {
				NextNodes = new Dictionary<string, List<TreeErrorNode>>();
				NextNodeIds = new Dictionary<string, List<string>>();
			}
			List<TreeErrorNode> list;
			List<string> list2;
			if (NextNodes.TryGetValue(channel, out list) == false) {
				list = new List<TreeErrorNode>();
				NextNodes[channel] = list;
				list2 = new List<string>();
				NextNodeIds[channel] = list2;
			} else {
				list2 = NextNodeIds[channel];
			}
			if (list2.Contains(node.Id) == false) {
				list.Add(node);
				list2.Add(node.Id);
			}
		}

		public void SetPrevNode(TreeErrorNode node, string channel, int index)
		{
			if (PrevNodes.Any(q => q.Node.Id == node.Id)) {
				return;
			}
			PrevNodes.Add(new ChannelErrorNode { Channel = channel, Index = index, Node = node });
		}

		public void SetError()
		{
			IsError = true;
		}

		public void SetError(string errorMessage)
		{
			IsError = true;
			ErrorMessage = errorMessage;
		}

		public void SetError(string errorMessage, bool isErrorNode)
		{
			IsError = true;
			ErrorMessage = errorMessage;
			IsErrorNode = isErrorNode;
		}

		public HashSet<TreeErrorNode> GetTreeNodeAll()
		{
			HashSet<TreeErrorNode> treeNodes = new HashSet<TreeErrorNode>();
			GetTreeNodeAll(treeNodes);
			return treeNodes;
		}

		private void GetTreeNodeAll(HashSet<TreeErrorNode> treeNodes)
		{
			treeNodes.Add(this);
			if (NextNodes == null) { return; }
			foreach (var node in NextNodes) {
				var list = node.Value;
				for (int i = 0; i < list.Count; i++) {
					var item = list[i];
					item.GetTreeNodeAll(treeNodes);
				}
			}
		}

		public void SetMaxLayer(int layer)
		{
			if (Layer < layer) {
				Layer = layer;
			}
		}
	}

	public sealed class StockErrorNode
	{
		private List<ChannelErrorNode> Nodes = new List<ChannelErrorNode>();
		private HashSet<string> IdSet = new HashSet<string>();

		public void Push(TreeErrorNode node, string channel, int index)
		{
			if (IdSet.Add(node.Id)) {// 防止合并
				Nodes.Add(new ChannelErrorNode {
					Channel = channel,
					Index = index,
					Node = node
				});
			}
		}

		public bool TryPop(out ChannelErrorNode node)
		{
			if (Nodes.Count == 0) { node = null; return false; }

			ChannelErrorNode temp = Nodes[0];
			for (int i = 1; i < Nodes.Count; i++) {
				var channelNode = Nodes[i];
				if (temp != null) {
					if (channelNode.Node.CurrWork.Layer < temp.Node.CurrWork.Layer) {
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
	public class ChannelErrorNode
	{
		public TreeErrorNode Node;
		public string Channel;
		public int Index;
	}
}