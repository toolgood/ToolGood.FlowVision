using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.Members.Factorys
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Factorys/Ajax/{action}")]
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
		[MemberMenu("FlowFactorys", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetFactoryListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var Page = await _memberFlowApplication.GetFactoryPage(request);
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowFactorys", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbFactory> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }

			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditFactory(request);
				} else {
					b = await _memberFlowApplication.AddFactory(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowFactorys", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

			try {
				var b = await _memberFlowApplication.DeleteFactory(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}