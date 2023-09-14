using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberGroup")]
	[Index("Name")]
	public class DbSysMemberGroup
	{
		public int Id { get; set; }

		/// <summary>
		/// 会员组名称
		/// </summary>
		[UserNameLength]
		public string Name { get; set; }

		/// <summary>
		/// 描述 200
		/// </summary>
		[CommentLength]
		public string Comment { get; set; }

		public int OrderNum { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsSystem { get; set; }

		public DateTime CreateTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public bool IsDelete { get; set; }

		public DateTime ModifyTime { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public DateTime? DeleteTime { get; set; }

		public static DbSysMemberGroup CreateBy(Request<DbSysMemberGroup> request)
		{
			return new DbSysMemberGroup() {
				Name = request.Data.Name,
				Comment = request.Data.Comment,
				OrderNum = request.Data.OrderNum,

				CreateTime = DateTime.Now,
				ModifyTime = DateTime.Now,
			};
		}

		public void EditBy(Request<DbSysMemberGroup> request)
		{
			Name = request.Data.Name;
			Comment = request.Data.Comment;
			OrderNum = request.Data.OrderNum;

			ModifyTime = DateTime.Now;
		}
	}
}