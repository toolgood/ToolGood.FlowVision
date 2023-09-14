using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	[ReadyGo3.Attributes.Table("flow_project")]
	[ReadyGo3.Attributes.Index("MainMemberId")]
	public class DbProject
	{
		public int Id { get; set; }
		public int MainMemberId { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string Name { get; set; }

		[ReadyGo3.Attributes.FieldLength(500)]
		public string Comment { get; set; }

		public int ExcelIndex { get; set; }

		public int NumberRequired { get; set; }
		public int Distance { get; set; }
		public int Area { get; set; }
		public int Volume { get; set; }
		public int Mass { get; set; }

		[FieldLength(60)]
		public string Code { get; set; }

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

		public DbProject()
		{ }

		public static DbProject CreateBy(Request<DbProject> request)
		{
			return new DbProject() {
				MainMemberId = request.Data.MainMemberId,

				Name = request.Data.Name?.Trim(),
				Comment = request.Data.Comment?.Trim(),
				Code = request.Data.Code?.Trim(),
				ExcelIndex = request.Data.ExcelIndex,
				NumberRequired = request.Data.NumberRequired,
				Distance = request.Data.Distance,
				Area = request.Data.Area,
				Volume = request.Data.Volume,
				Mass = request.Data.Mass,

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbProject> request)
		{
			Code = request.Data.Code?.Trim();
			Name = request.Data.Name?.Trim();
			Comment = request.Data.Comment?.Trim();
			ExcelIndex = request.Data.ExcelIndex;
			NumberRequired = request.Data.NumberRequired;
			Distance = request.Data.Distance;
			Area = request.Data.Area;
			Volume = request.Data.Volume;
			Mass = request.Data.Mass;

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}