using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps
{
	[MemberMenu("FlowApp", "export")]
	public class ExportsModel : MemberPageModel
	{
		private readonly IMemberApplication _application;

		public ExportsModel(IMemberApplication application)
		{
			_application = application;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			return Page();
		}
	}
}