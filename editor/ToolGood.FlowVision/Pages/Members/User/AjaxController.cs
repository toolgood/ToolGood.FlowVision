using Microsoft.AspNetCore.Mvc;
using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Members.Changes;

namespace ToolGood.FlowVision.Pages.Members.User
{
	[Route("/Members/User/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberApplication _memberApplication;

		public AjaxController(IMemberApplication MemberApplication)
		{
			_memberApplication = MemberApplication;
		}

		[HttpPost]
		public async Task<IActionResult> EditMember([FromBody, FromForm] Request<DbSysMember> request)
		{
			try {
				request.Data.Id = MemberDto.Id;
				var b = await _memberApplication.EditMember(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[HttpPost]
		public async Task<IActionResult> ChangePassword([FromBody, FromForm] Request<MemberChangePwdDto> request)
		{
			try {
				request.Data.MemberId = MemberDto.Id;
				var b = await _memberApplication.ChangePassword(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}