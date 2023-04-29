using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas.Projects;

namespace ToolGood.FlowVision.Pages.Members.Apps.InitValue
{
	[MemberMenu("FlowAppInitValue", "edit")]
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public EditModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public DbAppInitValue Dto { get; private set; }
		public List<DbApp> Apps { get; private set; }
		public List<SettingFormulaItem> Items { get; private set; }
		public List<int> AppIds { get; private set; }

		public async Task<IActionResult> OnGetAsync(int pid, int id = 0)
		{
			if (id < 0 || pid <= 0) { return Redirect(UrlSetting.MemberNotFoundUrl); }
			if (MemberDto.AllowProject(pid) == false) { return Redirect(UrlSetting.MemberNotFoundUrl); }

			AppIds = new List<int>();
			Apps = await _memberFlowApplication.GetAppAll(MemberDto.MainMemberId, pid);
			if (id == 0) {
				Dto = new DbAppInitValue();
				Dto.ProjectId = pid;
				Dto.InputType = "number";
				Dto.SettingFormula = "[{\"condition\":\"1\",\"formula\":\"\"}]";
				if (Apps.Count == 1) {
					AppIds.Add(Apps[0].Id);
				}
				ViewData["Title"] = "新增初始值";
				Items = new List<SettingFormulaItem>();
			} else {
				Dto = await _memberFlowApplication.GetAppInitValueById(MemberDto.MainMemberId, id);
				if (Dto == null) { return Redirect(UrlSetting.MemberNotFoundUrl); }
				if (Dto.ProjectId != pid) { return Redirect(UrlSetting.MemberNotFoundUrl); }

				if (string.IsNullOrEmpty(Dto.SettingFormula)) {
					Dto.SettingFormula = "[]";
					Items = new List<SettingFormulaItem>();
				} else {
					Items = JsonUtil.DeserializeObject<List<SettingFormulaItem>>(Dto.SettingFormula);
				}
				var ids = Dto.AppIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (var id2 in ids) { AppIds.Add(int.Parse(id2)); }

				ViewData["Title"] = "编辑初始值";
			}

			return Page();
		}

		public class SettingFormulaItem
		{
			public string Condition { get; internal set; }
			public string Formula { get; internal set; }
		}
	}
}