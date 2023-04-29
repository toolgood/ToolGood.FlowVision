using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.Members.Factorys
{
	[MemberMenu("FlowFactorys", "show")]
	public class IndexModel : MemberPageModel
	{
		private readonly IMemberApplication _application;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public IndexModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_application = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }
		public List<string> CategoryList { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());
			CategoryList = await _memberFlowApplication.GetCategoryInFactory(MemberDto.MainMemberId, pid);

			ViewData["Title"] = "³§Çø";
			return Page();
		}
	}
}