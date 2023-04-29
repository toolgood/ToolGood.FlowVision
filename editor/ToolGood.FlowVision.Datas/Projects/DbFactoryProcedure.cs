using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 厂区工艺
	/// </summary>
	[ReadyGo3.Attributes.Table("flow_factory_procedure")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbFactoryProcedure
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		[ReadyGo3.Attributes.FieldLength(250)]
		public string FactoryIds { get; set; }

		/// <summary>
		///
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(100)]
		public string Category { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(100)]
		public string Code { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string Name { get; set; }

		/// <summary>
		/// 检测公式
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(2000)]
		public string CheckFormula { get; set; }

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

		public static DbFactoryProcedure CreateBy(Request<DbFactoryProcedure> request)
		{
			return new DbFactoryProcedure() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,
				FactoryIds = "," + request.Data.FactoryIds + ",",

				Category = request.Data.Category?.Trim(),
				Code = request.Data.Code?.Trim(),
				Name = request.Data.Name?.Trim(),
				CheckFormula = request.Data.CheckFormula?.Trim(),
				Comment = request.Data.Comment?.Trim(),

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbFactoryProcedure> request)
		{
			FactoryIds = "," + request.Data.FactoryIds + ",";

			Category = request.Data.Category?.Trim();
			Code = request.Data.Code?.Trim();
			Name = request.Data.Name?.Trim();
			CheckFormula = request.Data.CheckFormula?.Trim();
			Comment = request.Data.Comment?.Trim();

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}