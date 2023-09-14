using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.Datas
{
	[Table("Sys_MemberOperationLog")]
	[Index("Name")]
	[Index("MainMemberId")]
	public class DbSysMemberOperationLog
	{
		public int Id { get; set; }

		public int MainMemberId { get; set; }

		public int MemberId { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[UserAgentLength]
		public string Name { get; set; }

		[UserNameLength]
		public string TrueName { get; set; }

		[FieldLength(2000)]
		public string Message { get; set; }

		/// <summary>
		/// IP
		/// </summary>
		[IpLength]
		public string Ip { get; set; }

		[ShortNameLength]
		public string SessionID { get; set; }

		[UserAgentLength]
		public string UserAgent { get; set; }

		[FieldLength(32)]
		public string Fingerprint { get; set; }

		public DateTime CreateTime { get; set; }
	}
}