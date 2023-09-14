using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps.Flow
{
	[Route("/FlowProject/Apps/Flow/Ajax/{action}")]
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
		[MemberMenu("FlowAppFlow", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetAppFlowListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var Page = await _memberFlowApplication.GetAppFlowPage(request);
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowAppFlow", "show")]
		[HttpPost]
		public async Task<IActionResult> GetItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			if (request.Data.AppId == null) { return Error("系统出了个小差！"); }

			try {
				var flow = await _memberFlowApplication.GetAppFlowByAppid(request.MainMemberId, request.Data.AppId.Value);
				return Success(flow);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowAppFlow", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbAppFlow> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }
			var appFlow = await _memberFlowApplication.GetAppFlowByAppCode(request.MainMemberId, request.Data.AppCode);
			if (appFlow != null) { request.Data.Id = appFlow.Id; }

			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditAppFlow(request);
				} else {
					b = await _memberFlowApplication.AddAppFlow(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowAppFlow", "show")]
		[HttpPost]
		public async Task<IActionResult> GetMachineSelectList([FromBody, FromForm] Request<GetFactoryMachineListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
			if (string.IsNullOrWhiteSpace(request.Data.Field)) {
				request.Data.Field = "Category asc, SubCategory asc, OrderNum asc,id asc";
				request.Data.Order = "";
			} else {
				request.Data.Field = $"{request.Data.Field} {request.Data.Order}, OrderNum asc,id asc";
				request.Data.Order = "";
			}

			try {
				var Page = await _memberFlowApplication.GetFactoryMachinePage(request);
				var factories = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, request.Data.ProjectId);
				foreach (var item in Page.Items) {
					var factory = factories.Where(q => q.Id == item.FactoryId).FirstOrDefault();
					if (factory != null) {
						item.Factory = factory.SimplifyName;
						item.FactoryCode = factory.Code;
					}
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowAppFlow", "show")]
		[HttpPost]
		public async Task<IActionResult> GetProcedureSelectList([FromBody, FromForm] Request<GetFactoryProcedureListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
			if (string.IsNullOrWhiteSpace(request.Data.Field)) {
				request.Data.Field = "id";
				request.Data.Order = "asc";
			}

			try {
				var Page = await _memberFlowApplication.GetFactoryProcedurePage(request);
				var factories = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, request.Data.ProjectId);
				foreach (var item in Page.Items) {
					var fids = item.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
					HashSet<string> fs = new HashSet<string>();
					HashSet<string> fcodes = new HashSet<string>();
					foreach (var fid in fids) {
						var factory = factories.Where(q => q.Id.ToString() == fid).FirstOrDefault();
						if (factory != null) {
							fs.Add(factory.SimplifyName);
							fcodes.Add(factory.Code);
						}
					}
					if (fs.Count == factories.Count) {
						item.Factorys = "所有厂区";
					} else {
						item.Factorys = string.Join(',', fs);
					}
					item.FactoryCodes = string.Join(',', fcodes);
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowAppFlow", "show")]
		[HttpPost]
		public async Task<IActionResult> GetSubCategoryList([FromBody, FromForm] Request<GetFactoryMachineListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var b = await _memberFlowApplication.GetSubCategoryInFactoryMachine(MemberDto.MainMemberId, request.Data.ProjectId, request.Data.Category);
				return Success(b);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}