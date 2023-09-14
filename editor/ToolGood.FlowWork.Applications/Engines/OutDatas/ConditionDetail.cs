using System.Text.Json.Serialization;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class ConditionDetail
	{
		public string Name { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<ConditionDetailItem> Conditions { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string DefaultFormula { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Comment { get; set; }
	}

	public sealed class ConditionDetailItem
	{
		public List<ConditionItem> Condition { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Formula { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool? Check { get; set; }
	}

	public sealed class ConditionItem
	{
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public bool? IsError { get; set; }

		public string Text { get; set; }
	}
}