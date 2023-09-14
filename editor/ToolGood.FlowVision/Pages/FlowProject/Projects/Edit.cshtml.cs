using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Pages.Members.Projects
{
	[MemberMenu("FlowProject", "edit")]
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public EditModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public DbProject Dto { get; private set; }

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
			if (id < 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (id == 0) {
				Dto = new DbProject();
				Dto.Code = Guid.NewGuid().ToString();
				Dto.NumberRequired = 1;
				Dto.Distance = 4;
				Dto.Area = 14;
				Dto.Volume = 24;
				Dto.Mass = 32;
				ViewData["Title"] = "新增项目";
			} else {
				Dto = await _memberFlowApplication.GetProjectById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				ViewData["Title"] = "编辑项目";
			}
			return Page();
		}
	}
}