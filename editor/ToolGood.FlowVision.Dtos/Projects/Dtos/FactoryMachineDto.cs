using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class FactoryMachineDto : DbFactoryMachine
	{
		[ReadyGo3.Attributes.Ignore]
		public string Factory { get; set; }

		[ReadyGo3.Attributes.Ignore]
		public string FactoryCode { get; set; }
	}
}