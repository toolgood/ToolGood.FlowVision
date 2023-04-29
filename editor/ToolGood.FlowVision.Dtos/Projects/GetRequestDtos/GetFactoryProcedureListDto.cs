using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetFactoryProcedureListDto : PageRequestDto
	{
		public int ProjectId { get; set; }
		public int Factory { get; set; }
		public string Category { get; set; }

		public string Name { get; set; }
	}
}