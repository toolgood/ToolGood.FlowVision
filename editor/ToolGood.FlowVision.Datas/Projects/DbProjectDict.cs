using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	[ReadyGo3.Attributes.Table("flow_project_dict")]
	[Index("MainMemberId", "ProjectId")]
	public class DbProjectDict
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[FieldLength(200)]
		public string Category { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[FieldLength(50)]
		public string Name { get; set; }

		[FieldLength(50)]
		public string Unit { get; set; }

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

		public static DbProjectDict CreateBy(Request<DbProjectDict> request)
		{
			return new DbProjectDict() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,

				Category = request.Data.Category?.Trim(),
				Name = request.Data.Name?.Trim(),
				Comment = request.Data.Comment?.Trim(),
				Unit = request.Data.Unit?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbProjectDict> request)
		{
			Category = request.Data.Category?.Trim();
			Name = request.Data.Name?.Trim();
			Comment = request.Data.Comment?.Trim();
			Unit = request.Data.Unit?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}