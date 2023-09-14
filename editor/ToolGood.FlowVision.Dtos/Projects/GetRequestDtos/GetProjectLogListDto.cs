using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetProjectLogListDto : PageRequestDto
	{
		public int ProjectId { get; set; }

		public string Name { get; set; }
		public int MemberId { get; set; }

		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
	}
}