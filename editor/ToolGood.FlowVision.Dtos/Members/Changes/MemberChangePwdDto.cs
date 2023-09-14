namespace ToolGood.FlowVision.Dtos.Members.Changes
{
	public class MemberChangePwdDto
	{
		public int MemberId { get; set; }

		public string OldPassword { get; set; }

		public string NewPassword { get; set; }

		public string ConfirmPassword { get; set; }
	}
}