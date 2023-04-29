using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Dicts
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/Dicts/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberApplication _memberApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;
		private readonly IMemberFlowImportApplication _memberFlowImportApplication;

		public AjaxController(IMemberApplication MemberApplication, IMemberFlowApplication memberFlowApplication, IMemberFlowImportApplication memberFlowImportApplication)
		{
			_memberApplication = MemberApplication;
			_memberFlowApplication = memberFlowApplication;
			_memberFlowImportApplication = memberFlowImportApplication;
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProjectDict", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectDictListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
			try {
				var Page = await _memberFlowApplication.GetProjectDictPage(request);
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowProjectDict", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbProjectDict> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditProjectDict(request);
				} else {
					b = await _memberFlowApplication.AddProjectDict(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowProjectDict", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (MemberDto.AllowProject(request.Data.Id.Value) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var b = await _memberFlowApplication.DeleteProjectDict(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowProjectDict", "Import")]
		[HttpGet]
		public IActionResult DownloadTemplate()
		{
			var bytes = _memberFlowImportApplication.GetProjectTemplate();
			return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FlowVision上传导入模板");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProjectDict", "Import")]
		[HttpPost]
		public async Task<IActionResult> UploadTemplate(int projectId, string fingerprint, IFormFile file)
		{
			if (MemberDto.AllowProject(projectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var stream = file.OpenReadStream();
				Request<Stream> request = new Request<Stream> {
					Data = stream,
					Ip = GetRealIP(),
					UserAgent = GetUserAgent(),
					MainMemberId = MemberDto.MainMemberId,
					OperatorId = MemberDto.Id,
					OperatorTrueName = MemberDto.TrueName,
					Fingerprint = fingerprint,
				};
				var b = await _memberFlowImportApplication.ImportProject(projectId, request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}