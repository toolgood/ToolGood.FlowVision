using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.Members.Securitys
{
	[MemberMenu("MemberOperationLog", "show")]
	public class OperationListModel : MemberPageModel
	{
		private readonly IMemberApplication _application;

		public OperationListModel(IMemberApplication application)
		{
			_application = application;
		}

		public IButtonPass ButtonPass { get; private set; }

		public async void OnGetAsync()
		{
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());
		}
	}
}