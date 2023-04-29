using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class FactoryProcedureDto : DbFactoryProcedure
	{
		[ReadyGo3.Attributes.Ignore]
		public string Factorys { get; set; }

		[ReadyGo3.Attributes.Ignore]
		public string FactoryCodes { get; set; }

		[ReadyGo3.Attributes.Ignore]
		public List<FactoryProcedureItemDto> Items { get; set; }
	}
}