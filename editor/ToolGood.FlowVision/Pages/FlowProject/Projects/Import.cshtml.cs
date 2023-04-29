using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Commons.Controllers;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Dicts
{
	[MemberMenu("FlowProject", "Import")]
	public class ImportModel : MemberPageModel
	{
		public int ProjectId { get; private set; }

		public IActionResult OnGetAsync(int id = 0)
		{
			if (id <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			ProjectId = id;
			ViewData["Title"] = "项目导入";
			return Page();
		}
	}
}