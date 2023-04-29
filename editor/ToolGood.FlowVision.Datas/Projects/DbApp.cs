using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 应用信息
	/// </summary>
	[Table("flow_app")]
	[Index("MainMemberId", "ProjectId")]
	public class DbApp
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		/// <summary>
		/// 编码
		/// </summary>
		[FieldLength(64)]
		public string Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[FieldLength(100)]
		public string Name { get; set; }

		[FieldLength(500)]
		public string Comment { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int CreateUserId { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int ModifyUserId { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int DeleteUserId { get; set; }

		public static DbApp CreateBy(Request<DbApp> request)
		{
			return new DbApp() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,

				Code = request.Data.Code?.Trim(),
				Name = request.Data.Name?.Trim(),
				Comment = request.Data.Comment?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbApp> request)
		{
			Code = request.Data.Code.Trim();
			Name = request.Data.Name.Trim();
			Comment = request.Data.Comment?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}