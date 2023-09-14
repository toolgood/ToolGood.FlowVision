using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Datas.Projects
{
	[ReadyGo3.Attributes.Table("flow_project_formula")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbProjectFormula
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		/// <summary>
		///
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string Category { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string Name { get; set; }

		[ReadyGo3.Attributes.FieldLength(1000)]
		public string Formula { get; set; }

		[ReadyGo3.Attributes.FieldLength(500)]
		public string Comment { get; set; }

		public bool IsReferenceCode { get; set; }

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

		public static DbProjectFormula CreateBy(Request<DbProjectFormula> request)
		{
			return new DbProjectFormula() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,

				Category = request.Data.Category?.Trim(),
				Name = request.Data.Name?.Trim(),
				Formula = request.Data.Formula?.Trim(),
				Comment = request.Data.Comment?.Trim(),
				IsReferenceCode = request.Data.IsReferenceCode,

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbProjectFormula> request)
		{
			Category = request.Data.Category?.Trim();
			Name = request.Data.Name?.Trim();
			Formula = request.Data.Formula?.Trim();
			Comment = request.Data.Comment?.Trim();
			IsReferenceCode = request.Data.IsReferenceCode;

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}