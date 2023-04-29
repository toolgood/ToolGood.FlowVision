using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;

namespace ToolGood.FlowVision.Pages.Members.Projects
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberFlowApplication _memberFlowApplication;
		private readonly IMemberFlowImportApplication _memberFlowImportApplication;
		private readonly IMemberApplication _memberApplication;

		public AjaxController(IMemberFlowApplication memberFlowApplication, IMemberFlowImportApplication memberFlowImportApplication, IMemberApplication memberApplication)
		{
			_memberFlowApplication = memberFlowApplication;
			_memberFlowImportApplication = memberFlowImportApplication;
			_memberApplication = memberApplication;
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProject", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectListDto> request)
		{
			try {
				var Page = await _memberFlowApplication.GetProjectPage(request);
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowProject", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbProject> request)
		{
			if (MemberDto.AllowProject(request.Data.Id) == false) { return LayuiError("系统出了个小差！"); }
			try {
				request.MainMemberId = MemberDto.MainMemberId;
				request.Data.MainMemberId = MemberDto.MainMemberId;
				if (MemberDto.MainMemberId != MemberDto.Id) { return Error("无权添加修改项目！"); }

				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditProject(request);
				} else {
					var dt = await _memberApplication.GetRealTime();
					b = await _memberFlowApplication.AddProject(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowProject", "Import")]
		[HttpGet]
		public IActionResult DownloadTemplate()
		{
			var bytes = _memberFlowImportApplication.GetProjectTemplate();
			return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "FlowVision上传导入模板.xlsx");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowProject", "Import")]
		[HttpPost]
		public async Task<IActionResult> UploadTemplate(int projectId, string fingerprint, IFormFile file)
		{
			if (MemberDto.AllowProject(projectId) == false) { return LayuiError("系统出了个小差！"); }

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
		}
	}
}