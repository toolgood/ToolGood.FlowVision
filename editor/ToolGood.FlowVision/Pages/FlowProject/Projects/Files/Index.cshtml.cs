using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Files
{
	[MemberMenu("FlowProjectFile", "show")]
	public class IndexModel : MemberPageModel
	{
		private readonly IMemberApplication _application;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public IndexModel(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_application = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		public IButtonPass ButtonPass { get; private set; }

		public List<DbSysMember> Members { get; private set; }
		public List<DbProject> Projects { get; private set; }

		public async void OnGetAsync()
		{
			ButtonPass = await _application.GetButtonPassByMemberId(MemberDto.Id, ViewData["MenuCode"].ToString());
			Members = await _application.GetSubAccountAll(MemberDto.MainMemberId);
			Projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);
		}
	}
}