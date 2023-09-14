using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetFactoryMachineProcedureListDto : PageRequestDto
	{
		public int ProjectId { get; set; }
		public int FactoryId { get; set; }

		public string Name { get; set; }
	}
}