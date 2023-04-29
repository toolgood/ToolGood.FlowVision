using System.Text.Json.Serialization;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class ProcedureDetail
	{
		public List<ProcedureMachine> Procedures { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<FormulaDetailItem> FormulaItems { get; set; }
	}

	public sealed class ProcedureMachine
	{
		public string Id { get; set; }
		public string Type { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string ProcedureLabel { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public ProcedureItem Procedure { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public MachineItem Machine { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public decimal? InputNum { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public decimal? OutputNum { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Dictionary<string, List<FormulaDetailItem>> Exps { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<string> NextNodeIds { get; set; }

		public void AddNodeId(string id)
		{
			if (NextNodeIds == null) {
				NextNodeIds = new List<string>();
			}
			NextNodeIds.Add(id);
		}

		public void AddFormulaDetailItem(string name, FormulaDetailItem item)
		{
			if (Exps == null) {
				Exps = new Dictionary<string, List<FormulaDetailItem>>();
			}
			List<FormulaDetailItem> list;
			if (Exps.TryGetValue(name, out list) == false) {
				list = new List<FormulaDetailItem>();
				Exps[name] = list;
			}
			list.Add(item);
		}
	}

	public sealed class ProcedureItem
	{
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CategoryCode { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Category { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		public ProcedureItem(FactoryProcedureItemWork itemWork)
		{
			CategoryCode = itemWork.CategoryCode;
			Category = itemWork.Category;
			Code = itemWork.Code;
			Name = itemWork.Name;
		}
	}

	public sealed class MachineItem
	{
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CategoryCode { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Category { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		public MachineItem(FactoryMachineWork itemWork)
		{
			CategoryCode = itemWork.MachineCategoryCode;
			Category = itemWork.MachineCategoryCode;
			Code = itemWork.MachineCode;
			Name = itemWork.MachineName;
		}
	}
}