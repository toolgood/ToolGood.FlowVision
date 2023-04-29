using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 厂区工艺 子项
	/// </summary>
	[ReadyGo3.Attributes.Table("flow_factory_procedure_item")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId", "ProcedureId")]
	public class DbFactoryProcedureItem
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		public int ProcedureId { get; set; }
		public int FactoryId { get; set; }

		[ReadyGo3.Attributes.FieldLength(100)]
		public string CategoryCode { get; set; }

		[ReadyGo3.Attributes.FieldLength(100)]
		public string Category { get; set; }

		[ReadyGo3.Attributes.FieldLength(100)]
		public string Code { get; set; }

		[ReadyGo3.Attributes.FieldLength(100)]
		public string Name { get; set; }

		[ReadyGo3.Attributes.FieldLength(500)]
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

		public static DbFactoryProcedureItem CreateBy(Request<DbFactoryProcedureItem> request)
		{
			return new DbFactoryProcedureItem() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,
				ProcedureId = request.Data.ProcedureId,
				FactoryId = request.Data.FactoryId,

				CategoryCode = request.Data.CategoryCode?.Trim(),
				Category = request.Data.Category?.Trim(),
				Code = request.Data.Code?.Trim(),
				Name = request.Data.Name?.Trim(),
				Comment = request.Data.Comment?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbFactoryProcedureItem> request)
		{
			CategoryCode = request.Data.CategoryCode?.Trim();
			Category = request.Data.Category?.Trim();
			Code = request.Data.Code?.Trim();
			Name = request.Data.Name?.Trim();
			Comment = request.Data.Comment?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}