using System.Text.Json.Serialization;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class OptionalMachine
	{
		public string NodeId { get; set; }
		public string MachineLabel { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CategoryCode { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Category { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }
	}
}