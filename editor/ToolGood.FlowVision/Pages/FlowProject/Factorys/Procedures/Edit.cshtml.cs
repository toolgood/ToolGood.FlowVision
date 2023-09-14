using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.Members.Technologys.FactoryProcedures
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
		public DbFactoryProcedure Dto { get; private set; }
		public List<DbFactory> FactoryList { get; private set; }
		public List<int> FactoryIds { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id = 0)
		{
			if (id < 0 || pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			ButtonPass = await _memberApplication.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			FactoryList = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);

			if (id == 0) {
				Dto = new DbFactoryProcedure();
				Dto.ProjectId = pid;
				if (FactoryList.Count == 1) { Dto.FactoryIds = FactoryList[0].Id.ToString(); }
				ViewData["Title"] = "������������";
			} else {
				Dto = await _memberFlowApplication.GetFactoryProcedureById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				ViewData["Title"] = "�༭��������";
			}
			FactoryIds = new List<int>();
			if (string.IsNullOrEmpty(Dto.FactoryIds) == false) {
				var fids = Dto.FactoryIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
				foreach (var fid in fids) {
					FactoryIds.Add(int.Parse(fid));
				}
			}

			return Page();
		}
	}
}