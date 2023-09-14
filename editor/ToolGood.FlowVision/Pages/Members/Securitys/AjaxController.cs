using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.Members.Securitys
{
	[Microsoft.AspNetCore.Mvc.Route("/Members/Securitys/Ajax/{action}")]
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
		[MemberMenu("MemberLoginLog", "show")]
		[HttpPost]
		public async Task<IActionResult> GetLoginList([FromBody, FromForm] Request<GetMemberLoginListDto> request)
		{
			try {
				var Page = await _memberApplication.GetMemberLoginLogPage(request);
				//var projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);
				//foreach (var item in Page.Items) {
				//    var project = projects.FirstOrDefault(q => q.Id == item.ProjectId);
				//    if (project != null) { item.ProjectName = project.Name; }
				//}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("MemberOperationLog", "show")]
		[HttpPost]
		public async Task<IActionResult> GetOperationList([FromBody, FromForm] Request<GetMemberOperationListDto> request)
		{
			try {
				var Page = await _memberApplication.GetMemberOperationLogPage(request);
				//var projects = await _memberFlowApplication.GetProjectAll(MemberDto.MainMemberId);
				//foreach (var item in Page.Items) {
				//    var project = projects.FirstOrDefault(q => q.Id == item.ProjectId);
				//    if (project != null) { item.ProjectName = project.Name; }
				//}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}
	}
}