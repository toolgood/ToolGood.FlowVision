using Microsoft.AspNetCore.Mvc;
using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Pages.Members.SubAccounts
{
	[MemberMenu("SubAccount", "edit")]
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _application;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public EditModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_application = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public DbSysMember Dto { get; private set; }
		public List<DbSysMemberGroup> Groups { get; private set; }
		public List<DbProject> Projects { get; private set; }
		public List<int> ProjectIds { get; private set; }

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
			if (id <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			Dto = await _application.GetSubAccountById(MemberDto.MainMemberId, id);
			if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectIds = new List<int>();
			if (string.IsNullOrEmpty(Dto.ProjectIds) == false) {
				foreach (var item in Dto.ProjectIds.Split(',', StringSplitOptions.RemoveEmptyEntries)) {
					ProjectIds.Add(int.Parse(item));
				}
			}

			Groups = await _application.GetMemberGroupAll();
			Projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);

			ViewData["Title"] = "±à¼­×ÓÕËºÅ";
			return Page();
		}
	}
}