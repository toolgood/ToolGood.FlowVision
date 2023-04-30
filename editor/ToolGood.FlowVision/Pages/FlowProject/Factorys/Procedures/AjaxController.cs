using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Projects.Dtos;

namespace ToolGood.FlowVision.Pages.Members.Technologys.FactoryProcedures
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Factorys/Procedures/Ajax/{action}")]
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
		[MemberMenu("TechnologyProcedures", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetFactoryProcedureListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var Page = await _memberFlowApplication.GetFactoryProcedurePage(request);
				var factories = await _memberFlowApplication.GetFactoryAll(MemberDto.MainMemberId, request.Data.ProjectId);
				foreach (var item in Page.Items) {
					var fids = item.FactoryIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
					HashSet<string> fs = new HashSet<string>();
					foreach (var fid in fids) {
						var factory = factories.Where(q => q.Id.ToString() == fid).FirstOrDefault();
						if (factory != null) { fs.Add(factory.SimplifyName); }
					}
					if (fs.Count == factories.Count) {
						item.Factorys = "所有厂区";
					} else {
						item.Factorys = string.Join(',', fs);
					}
				}
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("TechnologyProcedures", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<FactoryProcedureEditDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }
            try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditFactoryProcedure(request);
				} else {
					b = await _memberFlowApplication.AddFactoryProcedure(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("TechnologyProcedures", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

			try {
				var b = await _memberFlowApplication.DeleteFactoryProcedure(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}
	}
}