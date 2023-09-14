using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps.Flow
{
	[MemberMenu("FlowAppFlow", "show")]
	public class EdgeEditModel : MemberPageModel
	{
		private readonly IMemberApplication _application;

		public EdgeEditModel(IMemberApplication MemberApplication)
		{
			_application = MemberApplication;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }
		public int AppId { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			AppId = id;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			return Page();
		}
	}
}