using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Dtos.Projects.Dtos
{
	public class ProjectDto : DbProject
	{
		[ToolGood.ReadyGo3.Attributes.Ignore]
		public int MainId { get; set; }

		[ToolGood.ReadyGo3.Attributes.Ignore]
		public string MemberName { get; set; }

		[ToolGood.ReadyGo3.Attributes.Ignore]
		public string MemberTrueName { get; set; }
	}
}