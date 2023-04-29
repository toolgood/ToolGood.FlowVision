using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetAppMinProcedureListDto : PageRequestDto
	{
		public int ProjectId { get; set; }
		public int AppId { get; set; }

		public string Name { get; set; }
	}
}