using System.Text.Json.Serialization;
using ToolGood.FlowWork.Applications.Engines;

namespace ToolGood.FlowWork.Flows
{
	public sealed class AppInitValueWork
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 输入类型
		/// </summary>
		public InputType InputType { get; set; }// 输入类型

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<SettingFormulaItemWork> Conditions { get; set; }

		/// <summary>
		/// 抛出错误
		/// </summary>
		public bool IsThrowError { get; set; }

		/// <summary>
		/// 错误信息
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string ErrorMessage { get; set; }

		internal void Init(ProjectWork work)
		{
			for (int i = 0; i < Conditions.Count; i++) {
				var item = Conditions[i];
				item.Init(work);
			}
		}

		internal string GetMatchFormula(FlowEngine engine)
		{
			for (int i = 0; i < Conditions.Count; i++) {
				var item = Conditions[i];
				if (item.Check(engine)) {
					return item.Formula;
				}
			}
			return null;
		}
	}
}