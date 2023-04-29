using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.Members.Apps.InitValue
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Apps/InitValue/Ajax/{action}")]
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
		[MemberMenu("FlowAppInitValue", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetAppInitValueListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var Page = await _memberFlowApplication.GetAppInitValuePage(request);
				var apps = await _memberFlowApplication.GetAppAll(MemberDto.MainMemberId, request.Data.ProjectId);
				foreach (var item in Page.Items) {
					HashSet<string> names = new HashSet<string>();
					foreach (var id in item.AppIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)) {
						var app = apps.FirstOrDefault(q => q.Id.ToString() == id);
						if (app != null) {
							names.Add(app.Name);
						}
					}
					item.Apps = String.Join(",", names);
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowAppInitValue", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbAppInitValue> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }
			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditAppInitValue(request);
				} else {
					b = await _memberFlowApplication.AddAppInitValue(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowAppInitValue", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			try {
				var b = await _memberFlowApplication.DeleteAppInitValue(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}