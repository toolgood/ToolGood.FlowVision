using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberMenuButton")]
	[Index("ButtonCode")]
	public class DbSysMemberMenuButton
	{
		public int Id { get; set; }

		[ShortNameLength]
		public string ButtonCode { get; set; }

		[ShortNameLength]
		public string ButtonName { get; set; }

		public int OrderNum { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysMemberMenuButton CreateBy(Request<DbSysMemberMenuButton> request)
		{
			return new DbSysMemberMenuButton() {
				ButtonCode = request.Data.ButtonCode,
				ButtonName = request.Data.ButtonName,
				OrderNum = request.Data.OrderNum,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysMemberMenuButton> request)
		{
			ButtonCode = request.Data.ButtonCode;
			ButtonName = request.Data.ButtonName;
			OrderNum = request.Data.OrderNum;

			ModifyTime = DateTime.Now;
		}
	}
}