using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps
{
	[MemberMenu("FlowApp", "edit")]
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public EditModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public DbApp Dto { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id = 0)
		{
			if (id < 0 || pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			if (id == 0) {
				Dto = new DbApp();
				Dto.ProjectId = pid;
				ViewData["Title"] = "新增流程信息";
			} else {
				Dto = await _memberFlowApplication.GetAppById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				ViewData["Title"] = "编辑流程信息";
			}
			return Page();
		}
	}
}