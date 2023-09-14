using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Dtos
{
	public class GetFactoryMachineItemListDto : PageRequestDto
	{
		public int ProjectId { get; set; }
		public int MachineId { get; set; }
		public string Name { get; set; }
	}
}