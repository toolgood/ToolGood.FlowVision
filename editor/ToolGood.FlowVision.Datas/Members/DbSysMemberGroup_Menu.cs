using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberGroup_Menu")]
	[Index("GroupId")]
	[Index("MenuId")]
	[Index("MenuCode")]
	[Index("ButtonId")]
	[Index("ButtonCode")]
	public class DbSysMemberGroup_Menu
	{
		public int Id { get; set; }

		/// <summary>
		/// 管理组Id
		/// </summary>
		public int GroupId { get; set; }

		/// <summary>
		/// 菜单Id
		/// </summary>
		public int MenuId { get; set; }

		/// <summary>
		/// 操作名
		/// </summary>
		[ShortNameLength]
		public string MenuCode { get; set; }

		public int ButtonId { get; set; }

		[ShortNameLength]
		public string ButtonCode { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysMemberGroup_Menu CreateBy(Request<DbSysMemberGroup_Menu> request)
		{
			return new DbSysMemberGroup_Menu() {
				GroupId = request.Data.GroupId,
				MenuId = request.Data.MenuId,
				MenuCode = request.Data.MenuCode,
				ButtonId = request.Data.ButtonId,
				ButtonCode = request.Data.ButtonCode,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysMemberGroup_Menu> request)
		{
			GroupId = request.Data.GroupId;
			MenuId = request.Data.MenuId;
			MenuCode = request.Data.MenuCode;
			ButtonId = request.Data.ButtonId;
			ButtonCode = request.Data.ButtonCode;

			ModifyTime = DateTime.Now;
		}
	}
}