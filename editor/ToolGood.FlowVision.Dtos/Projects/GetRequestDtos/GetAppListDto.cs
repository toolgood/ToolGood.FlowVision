using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetAppListDto : PageRequestDto
	{
		public int ProjectId { get; set; }

		public string Name { get; set; }
	}
}