using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.Members.Factorys.Machines
{
	[MemberMenu("FlowFactoryMachines", "show")]
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
		public DbFactoryMachine Dto { get; private set; }
		public List<DbFactory> FactoryList { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id = 0)
		{
			if (id < 0 || pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			ButtonPass = await _memberApplication.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			FactoryList = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);
			if (id == 0) {
				Dto = new DbFactoryMachine();
				Dto.ProjectId = pid;
				Dto.OrderNum = 10000;
				if (FactoryList.Count >= 1) { Dto.FactoryId = FactoryList[0].Id; }
				ViewData["Title"] = "新增厂区机械";
			} else {
				Dto = await _memberFlowApplication.GetFactoryMachineById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				ViewData["Title"] = "编辑厂区机械";
			}
			return Page();
		}
	}
}