using System.Text.Json.Serialization;
using ToolGood.FlowWork.Applications.Engines;

namespace ToolGood.FlowWork.Flows
{
	public sealed class FactoryProcedureWork
	{
		private ProjectWork Project;// { get; set; }

		/// <summary>
		///
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Category { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		/// <summary>
		/// 检测公式
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CheckFormula { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Dictionary<string, FactoryProcedureItemWork> Items { get; set; }

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

		internal string GetErrorMessage(FlowEngineEx engine, string factoryCode)
		{
			var result = engine.FindErrorCondition(CheckFormula);
			if (result == null) {
				if (Items.ContainsKey(factoryCode) == false) {
					return "该厂区未找到相关工艺！";
				}
			}
			return result;
		}
	}
}