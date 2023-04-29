using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	///  工厂信息
	/// </summary>
	[Table("flow_factory")]
	[Index("MainMemberId", "ProjectId")]
	public class DbFactory
	{
		public int Id { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[FieldLength(200)]
		public string Category { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[FieldLength(100)]
		public string Code { get; set; }

		/// <summary>
		/// 简化名称
		/// </summary>
		public string SimplifyName { get; set; }

		[FieldLength(500)]
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

		public static DbFactory CreateBy(Request<DbFactory> request)
		{
			return new DbFactory() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,

				Category = request.Data.Category?.Trim(),
				Code = request.Data.Code?.Trim(),
				SimplifyName = request.Data.SimplifyName?.Trim(),
				Comment = request.Data.Comment?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbFactory> request)
		{
			Category = request.Data.Category?.Trim();
			Code = request.Data.Code?.Trim();
			SimplifyName = request.Data.SimplifyName?.Trim();
			Comment = request.Data.Comment?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}