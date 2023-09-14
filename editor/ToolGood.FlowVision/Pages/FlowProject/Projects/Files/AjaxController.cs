using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Files
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/Files/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberFlowApplication _memberFlowApplication;
		private readonly IMemberApplication _memberApplication;

		public AjaxController(IMemberFlowApplication memberFlowApplication, IMemberApplication memberApplication)
		{
			_memberFlowApplication = memberFlowApplication;
			_memberApplication = memberApplication;
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProjectFile", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectLogListDto> request)
		{
			try {
				var Page = await _memberFlowApplication.GetProjectFilePage(request);
				var projects = await _memberFlowApplication.GetProjectAll(request.MainMemberId);
				var members = await _memberApplication.GetSubAccountAll(request.MainMemberId);
				foreach (var item in Page.Items) {
					var project = projects.FirstOrDefault(q => q.Id == item.ProjectId);
					if (project != null) {
						item.ProjectName = project.Name;
					}
					var member = members.FirstOrDefault(q => q.Id == item.CreateUserId);
					if (member != null) {
						item.TrueName = member.TrueName;
					}
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowProjectFile", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (MemberDto.AllowProject(request.Data.Id.Value) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var b = await _memberFlowApplication.DeleteProjectFile(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}