using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities;
using ToolGood.FlowVision.Applications.Impl;
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
		public Dictionary<int, DbFactoryProcedureItem> Items { get; private set; }

		public List<int> FactoryIds { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id = 0)
		{
			if (id < 0 || pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			ButtonPass = await _memberApplication.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());

			FactoryList = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, pid);
			Items = new Dictionary<int, DbFactoryProcedureItem>();

			if (id == 0) {
				Dto = new DbFactoryProcedure();
				Dto.ProjectId = pid;
				if (FactoryList.Count == 1) { Dto.FactoryIds = FactoryList[0].Id.ToString(); }
				ViewData["Title"] = "新增厂区工艺";
			} else {
				Dto = await _memberFlowApplication.GetFactoryProcedureById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				var items = await _memberFlowApplication.GetFactoryProcedureItemAll(MemberDto.MainMemberId, pid, id);
				foreach (var item in items) {
					Items[item.FactoryId] = item;
				}
				ViewData["Title"] = "编辑厂区工艺";
			}
			FactoryIds = new List<int>();
			if (string.IsNullOrEmpty(Dto.FactoryIds) == false) {
				var fids = Dto.FactoryIds.Split(",", StringSplitOptions.RemoveEmptyEntries);
				foreach (var fid in fids) {
					FactoryIds.Add(int.Parse(fid));
				}
			}
			foreach (var factory in FactoryList) {
				if (Items.ContainsKey(factory.Id)==false) {
					Items[factory.Id] = new DbFactoryProcedureItem();
                }
            }
			return Page();
		}
	}
}