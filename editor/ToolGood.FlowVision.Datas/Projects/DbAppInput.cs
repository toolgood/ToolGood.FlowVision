using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 输入信息
	/// </summary>
	[ReadyGo3.Attributes.Table("flow_app_input")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbAppInput // 输入
	{
		public int Id { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[ReadyGo3.Attributes.FieldLength(250)]
		public string AppIds { get; set; }

		/// <summary>
		/// 输入名
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(100)]
		public string InputName { get; set; } // 输入名

		/// <summary>
		/// 输入类型
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(50)]
		public string InputType { get; set; }// 输入类型

		[FieldLength(50)]
		public string Unit { get; set; }

		/// <summary>
		/// 检查类型
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(2000)]
		public string CheckInput { get; set; } //检查类型

		/// <summary>
		/// 是否必填
		/// </summary>
		public bool IsRequired { get; set; } // 非空

		/// <summary>
		/// 默认值
		/// </summary>
		public bool UseDefaultValue { get; set; } // 默认值

		[ReadyGo3.Attributes.FieldLength(200)]
		public string DefaultValue { get; set; } // 默认值

		/// <summary>
		/// 抛出错误信息
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string ErrorMessage { get; set; } // 抛出错误信息

		[ReadyGo3.Attributes.FieldLength(500)]
		public string Comment { get; set; }

		public DateTime CreateTime { get; set; }

		[JsonIgnore]
		public int CreateUserId { get; set; }

		public DateTime ModifyTime { get; set; }

		[JsonIgnore]
		public int ModifyUserId { get; set; }

		[JsonIgnore]
		public bool IsDelete { get; set; }

		[JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		[JsonIgnore]
		public int DeleteUserId { get; set; }

		public static DbAppInput CreateBy(Request<DbAppInput> request)
		{
			return new DbAppInput() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,
				AppIds = "," + request.Data.AppIds?.Trim() + ",",

				InputName = request.Data.InputName?.Trim(),
				InputType = request.Data.InputType,
				CheckInput = request.Data.CheckInput?.Trim(),
				IsRequired = request.Data.IsRequired,
				UseDefaultValue = request.Data.UseDefaultValue,
				DefaultValue = request.Data.DefaultValue?.Trim(),
				ErrorMessage = request.Data.ErrorMessage?.Trim(),
				Comment = request.Data.Comment?.Trim(),
				Unit = request.Data.Unit?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbAppInput> request)
		{
			AppIds = "," + request.Data.AppIds?.Trim() + ",";
			InputName = request.Data.InputName?.Trim();
			InputType = request.Data.InputType;
			CheckInput = request.Data.CheckInput?.Trim();
			IsRequired = request.Data.IsRequired;
			UseDefaultValue = request.Data.UseDefaultValue;
			DefaultValue = request.Data.DefaultValue?.Trim();
			ErrorMessage = request.Data.ErrorMessage?.Trim();
			Comment = request.Data.Comment?.Trim();
			Unit = request.Data.Unit?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}