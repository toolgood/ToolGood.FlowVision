using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberMenu")]
	[Index("MenuName")]
	public class DbSysMemberMenu
	{
		public int Id { get; set; }

		[Ignore]
		public string Ids {
			get
			{
				return ParentIds + Id + "-";
			}
		}

		/// <summary>
		/// 父Id的完整
		/// </summary>
		[FieldLength(200)]
		public string ParentIds { get; set; }

		/// <summary>
		/// 父Id
		/// </summary>
		public int ParentId { get; set; }

		/// <summary>
		/// Code
		/// </summary>
		[ShortNameLength]
		public string MenuCode { get; set; }

		/// <summary>
		/// 名称
		/// </summary>
		[ShortNameLength]
		public string MenuName { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		[CommentLength]
		public string Comment { get; set; }

		/// <summary>
		/// Url
		/// </summary>
		[UrlLength]
		public string Url { get; set; }

		/// <summary>
		/// 操作
		/// </summary>
		[ShortNameLength]
		public string Buttons { get; set; }

		public int OrderNum { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysMemberMenu CreateBy(Request<DbSysMemberMenu> request)
		{
			return new DbSysMemberMenu() {
				ParentIds = request.Data.ParentIds,
				ParentId = request.Data.ParentId,
				MenuCode = request.Data.MenuCode,
				MenuName = request.Data.MenuName,

				Comment = request.Data.Comment,
				Url = request.Data.Url,
				Buttons = request.Data.Buttons ?? "",
				OrderNum = request.Data.OrderNum,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysMemberMenu> request)
		{
			MenuName = request.Data.MenuName;

			Comment = request.Data.Comment;
			Url = request.Data.Url;
			Buttons = request.Data.Buttons ?? "";
			OrderNum = request.Data.OrderNum;

			ModifyTime = DateTime.Now;
		}
	}
}