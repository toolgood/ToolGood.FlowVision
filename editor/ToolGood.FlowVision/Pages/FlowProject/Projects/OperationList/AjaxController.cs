using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos;

namespace ToolGood.FlowVision.Pages.Members.FlowProjects.OperationList
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/OperationList/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;

		public AjaxController(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProjectLog", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectLogListDto> request)
		{
			try {
				var Page = await _memberFlowApplication.GetProjectLogPage(request);
				var projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);
				foreach (var item in Page.Items) {
					var project = projects.FirstOrDefault(q => q.Id == item.ProjectId);
					if (project != null) { item.ProjectName = project.Name; }
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}
	}
}