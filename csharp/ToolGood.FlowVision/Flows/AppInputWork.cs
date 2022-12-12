using System.Text.Json.Serialization;
using ToolGood.FlowVision.Engines;

namespace ToolGood.FlowVision.Flows
{
	public sealed class AppInputWork
	{
		private ProjectWork Project;

		/// <summary>
		/// 输入名
		/// </summary>
		public string InputName { get; set; } // 输入名

		/// <summary>
		/// 单位
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Unit { get; set; }

		/// <summary>
		/// 输入类型
		/// </summary>
		public InputType InputType { get; set; }// 输入类型

		[System.Text.Json.Serialization.JsonIgnore]
		public int InputTypeNum { get { return (int)InputType; } }

		/// <summary>
		/// 检查类型
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string CheckInput { get; set; } //检查类型

		/// <summary>
		/// 是否必填
		/// </summary>
		public bool IsRequired { get; set; } // 非空

		/// <summary>
		/// 默认值
		/// </summary>
		public bool UseDefaultValue { get; set; } // 默认值

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string DefaultValue { get; set; } // 默认值

		/// <summary>
		/// 抛出错误信息
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string ErrorMessage { get; set; } // 抛出错误信息

		internal bool Check(FlowEngine engine)
		{
			if (string.IsNullOrEmpty(CheckInput)) { return true; }
			var progContext = Project.CreateProgContext(CheckInput);
			var operand = engine.EvaluateFormula(progContext, (int)Flows.InputType.Bool);
			return operand.BooleanValue;
		}

		internal void Init(ProjectWork work)
		{
			Project = work;
		}
	}
}