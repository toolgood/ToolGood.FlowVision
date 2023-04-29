using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 默认初始值
	/// </summary>
	[ReadyGo3.Attributes.Table("flow_app_initvalue")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbAppInitValue
	{
		public int Id { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[ReadyGo3.Attributes.FieldLength(250)]
		public string AppIds { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(20)]
		public string Name { get; set; }

		/// <summary>
		/// 输入类型
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(50)]
		public string InputType { get; set; }// 输入类型

		[ReadyGo3.Attributes.FieldLength(2000)]
		public string SettingFormula { get; set; }

		/// <summary>
		/// 抛出错误
		/// </summary>
		public bool IsThrowError { get; set; }

		/// <summary>
		/// 错误信息
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string ErrorMessage { get; set; }

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

		public static DbAppInitValue CreateBy(Request<DbAppInitValue> request)
		{
			return new DbAppInitValue() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,
				AppIds = "," + request.Data.AppIds?.Trim() + ",",

				Name = request.Data.Name?.Trim(),
				InputType = request.Data.InputType,
				SettingFormula = request.Data.SettingFormula?.Trim(),
				IsThrowError = request.Data.IsThrowError,
				ErrorMessage = request.Data.ErrorMessage?.Trim(),
				Comment = request.Data.Comment,

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbAppInitValue> request)
		{
			AppIds = "," + request.Data.AppIds?.Trim() + ",";
			Name = request.Data.Name?.Trim();
			InputType = request.Data.InputType;
			SettingFormula = request.Data.SettingFormula?.Trim();
			IsThrowError = request.Data.IsThrowError;
			ErrorMessage = request.Data.ErrorMessage?.Trim();
			Comment = request.Data.Comment;

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}