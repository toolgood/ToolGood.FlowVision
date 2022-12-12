using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Flows
{
	public sealed class FactoryProcedureItemWork
	{
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