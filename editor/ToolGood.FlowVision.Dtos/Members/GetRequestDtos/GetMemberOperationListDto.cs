using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos.Members
{
	public class GetMemberOperationListDto : PageRequestDto
	{
		public string Name { get; set; }

		public string Message { get; set; }
		public string Ip { get; set; }

		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}