using System.Text.Json.Serialization;
using ToolGood.Algorithm2;
using ToolGood.FlowWork.Applications.Engines;

namespace ToolGood.FlowWork.Flows
{
	public sealed class SettingFormulaWork
	{
		private ProjectWork Project;// { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public NodeWork NodeWork { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Name { get; set; }

		public InputType Type { get; set; }
		public string DefaultFormula { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<SettingFormulaItemWork> Conditions { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Comment { get; set; }

		internal Operand EvaluateFormula(FlowEngine engine)
		{
			if (Conditions != null && Conditions.Count > 0) {
				for (int i = 0; i < Conditions.Count; i++) {
					var settingFormulaItem = Conditions[i];
					if (settingFormulaItem.Check(engine)) {
						return settingFormulaItem.EvaluateFormula(engine, Type);
					}
				}
			}
			var progContext = Project.CreateProgContext(DefaultFormula);
			return engine.EvaluateFormula(progContext, (int)Type);
		}

		internal void Init(ProjectWork work)
		{
			Project = work;
			if (Conditions != null && Conditions.Count > 0) {
				for (int i = 0; i < Conditions.Count; i++) {
					var settingFormulaItem = Conditions[i];
					settingFormulaItem.Init(work);
				}
			}
		}

		internal string GetMatchFormula(FlowEngine engine)
		{
			if (Conditions != null && Conditions.Count > 0) {
				for (int i = 0; i < Conditions.Count; i++) {
					var settingFormulaItem = Conditions[i];
					if (settingFormulaItem.Check(engine)) {
						return settingFormulaItem.Formula;
					}
				}
			}
			return DefaultFormula;
		}
	}
}