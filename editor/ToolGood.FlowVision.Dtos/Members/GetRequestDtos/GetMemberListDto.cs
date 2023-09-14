using ToolGood.Attributes;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos.Members
{
	public class GetMemberListDto : PageRequestDto
	{
		[VaShortNameLength]
		public string Name { get; set; }

		[VaShortNameLength]
		public string TrueName { get; set; }

		public string Department { get; set; }
		public string JobNo { get; set; }

		[VaPhoneLength]
		public string Phone { get; set; }

		[VaPhoneLength]
		public string Email { get; set; }

		public int? GroupId { get; set; }
	}
}