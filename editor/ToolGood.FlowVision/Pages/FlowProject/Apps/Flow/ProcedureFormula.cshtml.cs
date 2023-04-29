using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps.Flow
{
	[MemberMenu("FlowAppFlow", "show")]
	public class ProcedureFormulaModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public ProcedureFormulaModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public IButtonPass ButtonPass { get; private set; }
		public int ProjectId { get; private set; }
		public string Code { get; private set; }

		public DbFactoryProcedure FactoryProcedure { get; private set; }
		public List<FactoryProcedureItemDto> FactoryProcedureItems { get; private set; }
		public List<FactoryMachineDto> FactoryMachines { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, string code, string machines)
		{
			if (pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (string.IsNullOrEmpty(code)) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			ProjectId = pid;
			Code = code;
			ButtonPass = await _memberApplication.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			FactoryProcedure = await _memberFlowApplication.GetFactoryProcedureByCode(MemberDto.MainMemberId, pid, Code);
			if (FactoryProcedure == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			FactoryProcedureItems = await _memberFlowApplication.GetFactoryProcedureItemAll(MemberDto.MainMemberId, pid, FactoryProcedure.Id);
			var factories = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);
			foreach (var item in FactoryProcedureItems) {
				var fact = factories.FirstOrDefault(q => q.Id == item.FactoryId);
				if (fact == null) { continue; }
				item.Factory = fact.SimplifyName;
			}
			FactoryMachines = new List<FactoryMachineDto>();
			if (string.IsNullOrEmpty(machines) == false) {
				var ms = machines.Split(',');
				foreach (var item in ms) {
					var db = await _memberFlowApplication.GetFactoryMachineByName(MemberDto.MainMemberId, pid, item);
					if (db == null) { continue; }
					var fact = factories.FirstOrDefault(q => q.Id == db.FactoryId);
					if (fact == null) { continue; }
					db.Factory = fact.SimplifyName;
					FactoryMachines.Add(db);
				}
			}
			return Page();
		}
	}
}