using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowWork.Commons;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Pages
{
	public class FlowFormulaModel : PageModel
	{
		public ProjectWork ProjectWork { get; private set; }
		public List<AppWork> AppWorks { get; private set; }
		private readonly IMemberFlowApplication _memberFlowApplication;

		public FlowFormulaModel(IMemberFlowApplication memberFlowApplication)
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

		public bool IsList(AppInputWork appInput)
		{
			if (string.IsNullOrEmpty(appInput.CheckInput)) {
				return false;
			}
			return Regex.IsMatch(appInput.CheckInput, @"^\{.*?\}\.has\(", RegexOptions.IgnoreCase);
		}

		public List<string> GetListItem(AppInputWork appInput)
		{
			var ms = Regex.Match(appInput.CheckInput, @"^\{(.*?)\}\.has\(", RegexOptions.IgnoreCase);
			List<string> result = new List<string>();
			var t = ms.Groups[1].Value;
			var sp = t.Split(',', StringSplitOptions.RemoveEmptyEntries);
			foreach (var s in sp) {
				var temp = s.Replace("\"", "").Replace("'", "").Replace("¡°", "").Replace("¡±", "").Replace("¡®", "").Replace("¡¯", "").Trim();
				result.Add(temp);
			}
			return result;
		}
	}
}