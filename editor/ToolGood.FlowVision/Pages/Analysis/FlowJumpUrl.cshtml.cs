using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToolGood.FlowWork.Pages.Analysis
{
	public class FlowJumpUrlModel : PageModel
	{
		public string URL { get; set; }

		public void OnGet(string url)
		{
			URL = url;
		}

		public void OnPost(string url)
		{
			URL = url;
		}
	}
}