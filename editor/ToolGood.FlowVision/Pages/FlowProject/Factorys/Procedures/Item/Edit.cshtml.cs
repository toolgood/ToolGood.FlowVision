using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Technologys.FactoryProcedures.Item
{
	[MemberMenu("TechnologyProcedures", "show")]
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public EditModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public IButtonPass ButtonPass { get; private set; }
		public DbFactoryProcedureItem Dto { get; private set; }
		public List<DbFactory> FactoryList { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int procedureId, int id = 0)
		{
			if (id < 0 || pid <= 0 || procedureId < 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			ButtonPass = await _memberApplication.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			if (id == 0) {
				Dto = new DbFactoryProcedureItem();
				Dto.ProjectId = pid;
				Dto.ProcedureId = procedureId;
				ViewData["Title"] = "新增厂区标记";
			} else {
				Dto = await _memberFlowApplication.GetFactoryProcedureItemById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				ViewData["Title"] = "编辑厂区标记";
			}
			FactoryList = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);
			//var factoryProcedure = await _memberFlowApplication.GetFactoryProcedureById(MemberDto.MainMemberId, procedureId);
			//var fids = factoryProcedure.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
			//for (int i = FactoryList.Count - 1; i >= 0; i--) {
			//    if (fids.Contains(FactoryList[i].Id.ToString()) == false) {
			//        FactoryList.RemoveAt(i);
			//    }
			//}

			return Page();
		}
	}
}