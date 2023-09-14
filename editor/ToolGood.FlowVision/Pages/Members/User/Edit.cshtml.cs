using Microsoft.AspNetCore.Mvc;
using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.Controllers;

namespace ToolGood.FlowVision.Pages.Members.User
{
	public class EditModel : MemberPageModel
	{
		private readonly IMemberApplication _application;

		public EditModel(IMemberApplication application)
		{
			_application = application;
		}

		public DbSysMember Admin { get; private set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Admin = await _application.GetMemberById(MemberDto.Id);
			return Page();
		}
	}
}