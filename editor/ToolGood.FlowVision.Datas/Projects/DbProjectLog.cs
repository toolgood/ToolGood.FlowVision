using ToolGood.ReadyGo3.Attributes;

namespace ToolGood.FlowVision.Datas.Projects
{
	[Table("flow_project_Log")]
	[Index("MainMemberId", "ProjectId")]
	public class DbProjectLog
	{
		public int Id { get; set; }

		[System.Text.Json.Serialization.JsonIgnore]
		public int MainMemberId { get; set; }

		public int ProjectId { get; set; }

		public int MemberId { get; set; }

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