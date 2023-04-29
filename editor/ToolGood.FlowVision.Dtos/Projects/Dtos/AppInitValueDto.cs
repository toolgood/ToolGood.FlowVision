using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class AppInitValueDto : DbAppInitValue
	{
		[ReadyGo3.Attributes.Ignore]
		public string Apps { get; set; }
	}
}