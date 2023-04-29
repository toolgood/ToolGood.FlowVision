using ToolGood.Attributes;

namespace ToolGood.FlowVision.Dtos.Members.Changes
{
	public class MemberLoginDto
	{
		[VaUserName]
		[VaRequired]
		public string TName { get; set; }

		[VaPasswordLength]
		[VaRequired]
		public string TPwd { get; set; }

		[VaRequired]
		public string Vcode { get; set; }

		public string Fingerprint { get; set; }

		public string Mac { get; set; }

		public string CheckHash { get; set; }
	}
}