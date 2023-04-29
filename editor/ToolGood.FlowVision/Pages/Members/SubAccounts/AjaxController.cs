using Microsoft.AspNetCore.Mvc;
using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Members.Changes;

namespace ToolGood.FlowVision.Pages.Members.SubAccounts
{
	[Microsoft.AspNetCore.Mvc.Route("/Members/SubAccounts/Ajax/{action}")]
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
		[MemberMenu("SubAccount", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetMemberListDto> request)
		{
			try {
				var Page = await _memberApplication.GetSubAccountPage(request);
				var Groups = await _memberApplication.GetMemberGroupAll();
				foreach (var item in Page.Items) {
					var group = Groups.FirstOrDefault(q => q.Id == item.GroupId);
					item.GroupName = group.Name;
				}
				var projects = await _memberFlowApplication.GetProjectAll(request.MainMemberId);

				foreach (var item in Page.Items) {
					if (string.IsNullOrEmpty(item.ProjectIds)) { continue; }
					var fids = item.ProjectIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
					List<string> fs = new List<string>();
					foreach (var fid in fids) {
						var project = projects.Where(q => q.Id.ToString() == fid).FirstOrDefault();
						if (project != null) { fs.Add(project.Name); }
					}
					item.ProjectNames = String.Join(",", fs);
				}

				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("SubAccount", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbSysMember> request)
		{
			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberApplication.EditSubAccount(request);
				} else {
					b = await _memberApplication.AddSubAccount(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("SubAccount", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			try {
				var b = await _memberApplication.DeleteSubAccount(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("SubAccount", "delete")]
		[HttpPost]
		public async Task<IActionResult> FrozenItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			try {
				var b = await _memberApplication.FrozenItem(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("SubAccount", "delete")]
		[HttpPost]
		public async Task<IActionResult> RecoveryItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			try {
				var b = await _memberApplication.RecoveryItem(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("SubAccount", "edit")]
		[HttpPost]
		public async Task<IActionResult> ChangePassword([FromBody, FromForm] Request<MemberChangePwdDto> request)
		{
			try {
				request.Data.MemberId = MemberDto.Id;
				var b = await _memberApplication.ChangeSubAccountPassword(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("SubAccountLoginList", "show")]
		[HttpPost]
		public async Task<IActionResult> GetLoginList([FromBody, FromForm] Request<GetMemberLoginListDto> request)
		{
			try {
				var Page = await _memberApplication.GetSubAccountLoginLogPage(request);
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