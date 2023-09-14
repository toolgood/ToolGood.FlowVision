using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberLoginLog")]
	[Index("Name")]
	public class DbSysMemberLoginLog
	{
		public int Id { get; set; }
		public int MainMemberId { get; set; }

		public int MemberId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[UserNameLength]
		public string Name { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[UserNameLength]
		public string TrueName { get; set; }

		/// <summary>
		/// 返回信息
		/// </summary>
		[CommentLength]
		public string Message { get; set; }

		[FieldLength(250)]
		public string SessionID { get; set; }

		/// <summary>
		/// IP
		/// </summary>
		[IpLength]
		public string Ip { get; set; }

		public bool Success { get; set; }

		[UserAgentLength]
		public string UserAgent { get; set; }

		/// <summary>
		///
		/// </summary>
		[FieldLength(32)]
		public string Fingerprint { get; set; }

		/// <summary>
		/// 添加日期
		/// </summary>
		public DateTime CreateTime { get; set; }
	}
}