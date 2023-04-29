using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class ProjectLogDto : DbProjectLog
	{
		[ReadyGo3.Attributes.Ignore]
		public string ProjectName { get; set; }
	}
}