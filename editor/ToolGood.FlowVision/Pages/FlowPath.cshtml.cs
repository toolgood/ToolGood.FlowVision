using Microsoft.AspNetCore.Mvc.RazorPages;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowWork.Commons;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Pages
{
	public class FlowPathModel : PageModel
	{
		public ProjectWork ProjectWork { get; private set; }
		public List<AppWork> AppWorks { get; private set; }
		private readonly IMemberFlowApplication _memberFlowApplication;

		public FlowPathModel(IMemberFlowApplication memberFlowApplication)
		{
			_memberFlowApplication = memberFlowApplication;
		}

		public void OnGet(string projectCode = null)
		{
			ProjectWork = ProjectWorkCache.GetProjectWork(_memberFlowApplication, projectCode);
			if (ProjectWork != null) {
				AppWorks = ProjectWork.AppList.Values.ToList();
			} else {
				ProjectWork = new ProjectWork();
				ProjectWork.FactoryList = new Dictionary<string, FactoryWork>();
				AppWorks = new List<AppWork>();
			}
		}
	}
}