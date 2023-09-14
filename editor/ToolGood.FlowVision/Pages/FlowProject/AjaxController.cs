using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;

namespace ToolGood.FlowVision.Pages.FlowProject
{
	public class AjaxController : MemberApiController
	{
		private readonly IMemberFlowApplication _memberFlowApplication;

		public AjaxController(IMemberFlowApplication memberFlowApplication)
		{
			_memberFlowApplication = memberFlowApplication;
		}

		[HttpGet("/_/inputControl/AceKeywords.js")]
		public async Task<IActionResult> AceKeywords(int pid = 0, int appid = 0)
		{
			if (MemberDto.AllowProject(pid) == false) { return Error("系统出了个小差！"); }
			try {
				var str = await _memberFlowApplication.GetAceKeywords(MemberDto.MainMemberId, pid, appid);
				return Content(str, "application/x-javascript");
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}