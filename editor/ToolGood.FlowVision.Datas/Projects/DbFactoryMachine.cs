using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Datas.Projects
{
	/// <summary>
	/// 工厂机器
	/// </summary>
	[ReadyGo3.Attributes.Table("flow_factory_machine")]
	[ReadyGo3.Attributes.Index("MainMemberId", "ProjectId")]
	public class DbFactoryMachine
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		/// <summary>
		/// 分类
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(50)]
		public string Category { get; set; }

		/// <summary>
		/// 子分类
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(50)]
		public string SubCategory { get; set; }

		[ReadyGo3.Attributes.FieldLength(200)]
		public string Name { get; set; }

		public int FactoryId { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(100)]
		public string MachineCode { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string MachineName { get; set; }

		/// <summary>
		/// 编号
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(100)]
		public string MachineCategoryCode { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(200)]
		public string MachineCategoryName { get; set; }

		/// <summary>
		/// 检测公式
		/// </summary>
		[ReadyGo3.Attributes.FieldLength(2000)]
		public string CheckFormula { get; set; }

		public int OrderNum { get; set; }

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

		public static DbFactoryMachine CreateBy(Request<DbFactoryMachine> request)
		{
			return new DbFactoryMachine() {
				MainMemberId = request.MainMemberId,
				ProjectId = request.Data.ProjectId,

				Category = request.Data.Category?.Trim(),
				SubCategory = request.Data.SubCategory?.Trim(),
				Name = request.Data.Name?.Trim(),

				FactoryId = request.Data.FactoryId,
				MachineCode = request.Data.MachineCode?.Trim(),
				MachineName = request.Data.MachineName?.Trim(),
				MachineCategoryCode = request.Data.MachineCategoryCode?.Trim(),
				MachineCategoryName = request.Data.MachineCategoryName?.Trim(),

				CheckFormula = request.Data.CheckFormula?.Trim(),
				Comment = request.Data.Comment?.Trim(),
				OrderNum = request.Data.OrderNum,

				CreateTime = DateTime.Now,
				CreateUserId = request.OperatorId,
				ModifyTime = DateTime.Now,
				ModifyUserId = request.OperatorId,
			};
		}

		public void EditBy(Request<DbFactoryMachine> request)
		{
			Category = request.Data.Category?.Trim();
			SubCategory = request.Data.SubCategory?.Trim();
			Name = request.Data.Name?.Trim();

			FactoryId = request.Data.FactoryId;
			MachineCode = request.Data.MachineCode?.Trim();
			MachineName = request.Data.MachineName?.Trim();
			MachineCategoryCode = request.Data.MachineCategoryCode?.Trim();
			MachineCategoryName = request.Data.MachineCategoryName?.Trim();

			CheckFormula = request.Data.CheckFormula?.Trim();
			Comment = request.Data.Comment?.Trim();
			OrderNum = request.Data.OrderNum;

			ModifyTime = DateTime.Now;
			ModifyUserId = request.OperatorId;
		}
	}
}