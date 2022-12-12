using System.Text.Json.Serialization;
using ToolGood.FlowVision.Engines;

namespace ToolGood.FlowVision.Flows
{
	public sealed class FactoryMachineWork
	{
		private ProjectWork Project;// { get; set; }

		public string Factory { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		public string MachineCode { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		public string MachineName { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string MachineCategoryCode { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string MachineCategoryName { get; set; }

		/// <summary>
		/// 检测公式
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CheckFormula { get; set; }

		internal void Init(ProjectWork work)
		{
			Project = work;
		}

		internal bool Check(FlowEngine engine)
		{
			if (string.IsNullOrEmpty(CheckFormula)) { return true; }
			var progContext = Project.CreateProgContext(CheckFormula);
			return engine.TryEvaluate(progContext, false);
		}
	}
}