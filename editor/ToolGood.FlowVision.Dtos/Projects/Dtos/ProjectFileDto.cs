using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class ProjectFileDto : DbProjectFile
	{
		[ReadyGo3.Attributes.Ignore]
		public string ProjectName { get; set; }

		[ReadyGo3.Attributes.Ignore]
		public string TrueName { get; set; }
	}
}