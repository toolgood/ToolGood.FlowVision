using System.Text.Json.Serialization;

namespace ToolGood.FlowWork.Flows
{
	public sealed class AppWork
	{
		/// <summary>
		/// 编码
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		public List<AppInitValueWork> InitValueList { get; set; }
		public List<AppInputWork> InputList { get; set; }
		public Dictionary<string, NodeWork> AllNodeWork { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public Dictionary<string, List<NodeWork>> ParentNodeWorks { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public StartFlowWork Start { get; set; }

		internal void Init(ProjectWork work)
		{
			for (int i = 0; i < InitValueList.Count; i++) {
				var item = InitValueList[i];
				item.Init(work);
			}
			for (int i = 0; i < InputList.Count; i++) {
				var item = InputList[i];
				item.Init(work);
			}
			foreach (var item in AllNodeWork) {
				item.Value.Init(work, this);
			}
			Start = AllNodeWork.FirstOrDefault(q => q.Value.NodeType == CellType.Start).Value as StartFlowWork;

			ParentNodeWorks = new Dictionary<string, List<NodeWork>>();
			foreach (var item in AllNodeWork) {
				foreach (var nextNode in item.Value.NextNodes) {
					for (int i = 0; i < nextNode.Value.Count; i++) {
						var node = nextNode.Value[i];
						if (node is ProcedureFlowWork) {
							List<NodeWork> list;
							if (ParentNodeWorks.TryGetValue(node.Id, out list) == false) {
								list = new List<NodeWork>();
								ParentNodeWorks[node.Id] = list;
							}
							list.Add(item.Value);
						}
					}
				}
			}
		}
	}
}