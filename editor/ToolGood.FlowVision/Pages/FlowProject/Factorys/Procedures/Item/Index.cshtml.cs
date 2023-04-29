using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Technologys.FactoryProcedures.Item
{
	[MemberMenu("TechnologyProcedures", "show")]
	public class IndexModel : MemberPageModel
	{
		private readonly IMemberApplication _application;

		public IndexModel(IMemberApplication application)
		{
			_application = application;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }
		public int ProcedureId { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int procedureId)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (procedureId <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			ProcedureId = procedureId;
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			ViewData["Title"] = "厂区标记";
			return Page();
		}
	}
}