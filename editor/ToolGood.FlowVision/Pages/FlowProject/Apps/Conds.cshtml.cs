using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps
{
	[MemberMenu("FlowApp", "show")]
	public class CondsModel : MemberPageModel
	{
		private readonly IMemberApplication _application;
		private readonly IMemberFlowGraphApplication _memberFlowGraphApplication;

		public CondsModel(IMemberApplication application, IMemberFlowGraphApplication memberFlowGraphApplication)
		{
			_application = application;
			_memberFlowGraphApplication = memberFlowGraphApplication;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }

		public List<string> Conditions { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());
			Conditions = await _memberFlowGraphApplication.EnumerationConditions(MemberDto.MainMemberId, ProjectId);

			return Page();
		}


 
    }
}
