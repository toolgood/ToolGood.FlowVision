using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetFactoryProcedureItemListDto : PageRequestDto
	{
		public int ProjectId { get; set; }
		public int ProcedureId { get; set; }

		public string Name { get; set; }
	}
}