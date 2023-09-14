using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Dtos.Flow
{
	public class ProjectAppInfo
	{
		public DbApp App { get; set; }
		public List<DbAppInput> InputList { get; set; }
		public List<DbAppInitValue> InitValueList { get; set; }
		public List<CellDto> Cells { get; set; }

		public StartFlowDto Start { get; set; }

		public bool BuildFlow()
		{
			if (Cells == null) { return false; }
			var start = Cells.FirstOrDefault(q => q.NodeType == CellType.Start);
			if (start == null) return false;
			Start = (StartFlowDto)start;
			Start.IsUsed = true;

			var edges = Cells.Where(q => q.NodeType == CellType.Edge).ToList();
			Dictionary<string, List<EdgeDto>> edgeDict = new Dictionary<string, List<EdgeDto>>();
			foreach (EdgeDto edge in edges) {
				if (edgeDict.TryGetValue(edge.SourceCell, out List<EdgeDto> list)) {
					list.Add(edge);
				} else {
					edgeDict[edge.SourceCell] = new List<EdgeDto>() { edge };
				}
			}

			Dictionary<string, NodeDto> cellDict = Cells.Where(q => q.NodeType != CellType.Edge).ToDictionary(q => q.Id, q => (NodeDto)q);
			Stack<NodeDto> stack = new Stack<NodeDto>();
			stack.Push(Start);
			while (stack.TryPop(out NodeDto cell)) {
				if (edgeDict.TryGetValue(cell.Id, out List<EdgeDto> list)) {
					foreach (var edge in list) {
						if (edge.IsUsed) continue;
						if (cellDict.TryGetValue(edge.TargetCell, out NodeDto nodeDto)) {
							edge.IsUsed = true;
							nodeDto.IsUsed = true;
							stack.Push(nodeDto);

							if (cell.NextNodes.TryGetValue(edge.Label, out List<NodeDto> temp)) {
								temp.Add(nodeDto);
							} else {
								cell.NextNodes[edge.Label] = new List<NodeDto>() { nodeDto };
							}
						}
					}
				}
			}
			return true;
		}

		public bool CheckEndNodeOrErrorNode()
		{
			foreach (var item in Cells) {
				if (item.IsUsed == false) continue;
				if (item.NodeType == CellType.Edge) continue;
				if (item.NodeType == CellType.End) continue;
				if (item.NodeType == CellType.Error) continue;

				var node = item as NodeDto;
				if (node.NextNodes.Count == 0) {
					return false;
				}
			}
			return true;
		}

		public void SetProcedure(List<FactoryProcedureDto> factoryProcedures, List<FactoryMachineDto> machineList)
		{
			foreach (var item in Cells) {
				if (item is ProcedureFlowDto procedureFlow) {
					var procedureDto = factoryProcedures.FirstOrDefault(q => q.Code == procedureFlow.Procedure);
					if (procedureDto != null) {
						procedureFlow.FactoryProcedure = procedureDto;
					} else {
						item.IsUsed = false;
					}
					foreach (var machineInfo in procedureFlow.Machines) {
						var machineDto = machineList.FirstOrDefault(q => q.Name == machineInfo.Name);
						if (machineDto != null) {
							machineInfo.IsUsed = true;
							machineInfo.FactoryMachine = machineDto;
							machineInfo.FactoryCode = machineDto.FactoryCode;
						} else {
							machineInfo.IsUsed = false;
						}
					}
				}
			}
		}
	}
}