using Microsoft.AspNetCore.Mvc.RazorPages;
using ToolGood.FlowWork.Commons;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Pages
{
	public class FlowProcedureModel : PageModel
	{
		public ProjectWork ProjectWork { get; private set; }
		public List<AppWork> AppWorks { get; private set; }

		public void OnGet()
		{
			ProjectWork = ProjectWorkCache.GetProjectWork();
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