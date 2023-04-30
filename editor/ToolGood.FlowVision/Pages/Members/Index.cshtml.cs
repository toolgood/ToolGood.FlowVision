using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.Members
{
	public class IndexModel : MemberPageModel
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;
		public String Logo { get; private set; }

		public List<TopMemberMenuDto> TopMenus { get; private set; }
		public TreeMemberMenuDto TreeMenuDto { get; private set; }

		public IndexModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public async Task<IActionResult> OnGetAsync(string code = "MemberTopDesktop")
		{
			TopMenus = new List<TopMemberMenuDto>();
			var topMenus = await _memberApplication.GetTopMenu(MemberDto.Id);
			foreach (var item in topMenus) {
				TopMenus.Add(new TopMemberMenuDto(item, code));
			}
			TopMemberMenuDto topmenu = TopMenus.FirstOrDefault(q => q.Activity);
			if (string.IsNullOrEmpty(code)) {
				if (topmenu == null && TopMenus.Count > 0) { topmenu = TopMenus[0]; }
			}
			if (topmenu == null) { topmenu = TopMenus[0]; }
			if (topmenu != null) {
				topmenu.Activity = true;

				if (topmenu.Code == "ProjectTopDesktop") {
					var projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);
					TreeMenuDto = new TreeMemberMenuDto();
					int id = 1;
					foreach (var item in projects) {
						if (MemberDto.AllowProject(item.Id) == false) { continue; }

						var TreeMenuDto2 = new TreeMemberMenuDto();
						TreeMenuDto2.Id = id++;
						TreeMenuDto2.Name = item.Name;

						var TreeMenuDto3_1 = new TreeMemberMenuDto();
						TreeMenuDto2.Items.Add(TreeMenuDto3_1);
						TreeMenuDto3_1.Id = id++;
						TreeMenuDto3_1.Name = "�ο��ĵ�";
						TreeMenuDto3_1.Items.Add(new TreeMemberMenuDto() { Name = "��Ŀ�ֵ�", Id = id++, Url = "/FlowProject/Projects/Dicts/Index?pid=" + item.Id });
						TreeMenuDto3_1.Items.Add(new TreeMemberMenuDto() { Name = "��Ŀ��ʽ", Id = id++, Url = "/FlowProject/Projects/Formulas/Index?pid=" + item.Id });
						TreeMenuDto3_1.Items.Add(new TreeMemberMenuDto() { Name = "��Ŀ����", Id = id++, Url = "/FlowProject/Projects/Datas/Index?pid=" + item.Id });

						var TreeMenuDto3 = new TreeMemberMenuDto();
						TreeMenuDto2.Items.Add(TreeMenuDto3);
						TreeMenuDto3.Id = id++;
						TreeMenuDto3.Name = "��������";
						TreeMenuDto3.Items.Add(new TreeMemberMenuDto() { Name = "������Ϣ", Id = id++, Url = "/FlowProject/Factorys/Index?pid=" + item.Id });
						TreeMenuDto3.Items.Add(new TreeMemberMenuDto() { Name = "������е", Id = id++, Url = "/FlowProject/Factorys/Machines/Index?pid=" + item.Id });
						TreeMenuDto3.Items.Add(new TreeMemberMenuDto() { Name = "��������", Id = id++, Url = "/FlowProject/Factorys/Procedures/Index?pid=" + item.Id });

						var TreeMenuDto5 = new TreeMemberMenuDto();
						TreeMenuDto2.Items.Add(TreeMenuDto5);
						TreeMenuDto5.Id = id++;
						TreeMenuDto5.Name = "�������";
						TreeMenuDto5.Items.Add(new TreeMemberMenuDto() { Name = "������Ϣ", Id = id++, Url = "/FlowProject/Apps/Index?pid=" + item.Id });
						TreeMenuDto5.Items.Add(new TreeMemberMenuDto() { Name = "������", Id = id++, Url = "/FlowProject/Apps/Input/Index?pid=" + item.Id });
						TreeMenuDto5.Items.Add(new TreeMemberMenuDto() { Name = "��ʼֵ", Id = id++, Url = "/FlowProject/Apps/InitValue/Index?pid=" + item.Id });

						TreeMenuDto.Items.Add(TreeMenuDto2);
					}
				} else {
					TreeMenuDto = await _memberApplication.GetSidebarMenu(MemberDto.Id, topmenu.Id, topmenu.Ids);
				}
			}

			ViewData["Title"] = "FlowVision �������ƽ̨";
			return Page();
		}

		public HtmlString GetMenuClass(TopMemberMenuDto dto)
		{
			if (dto.Activity) {
				return new HtmlString("class=\"layui-this\"");
			}
			return null;
		}
	}
}