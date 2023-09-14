using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.Members.Factorys.Machines
{
	[MemberMenu("FlowFactoryMachines", "show")]
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
		public List<DbFactory> FactoryList { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());
			CategoryList = await _memberFlowApplication.GetCategoryInFactoryMachine(MemberDto.MainMemberId, pid);
			FactoryList = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);
			FactoryList = FactoryList.OrderBy(q => q.SimplifyName).ToList();

			ViewData["Title"] = "³§Çø»úÐµ";
			return Page();
		}
	}
}