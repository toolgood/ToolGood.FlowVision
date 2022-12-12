using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Flows
{
	public sealed class FactoryWork
	{
		/// <summary>
		/// 编号
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		/// <summary>
		/// 简化名称
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		internal void Init(ProjectWork work)
		{
		}
	}
}