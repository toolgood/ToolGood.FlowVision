using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Formulas
{
    [Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/Formulas/Ajax/{action}")]
    public class AjaxController : MemberApiController
    {
        private readonly IMemberFlowApplication _memberFlowApplication;
        private readonly IMemberFlowGraphApplication _memberFlowGraphApplication;

        public AjaxController(IMemberFlowApplication memberFlowApplication, IMemberFlowGraphApplication memberFlowGraphApplication)
        {
            _memberFlowApplication = memberFlowApplication;
            _memberFlowGraphApplication = memberFlowGraphApplication;
        }

        [IgnoreAntiforgeryToken]
        [MemberMenu("FlowProjectFormula", "show")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectFormulaListDto> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
            try {
                var Page = await _memberFlowApplication.GetProjectFormulaPage(request);
                return LayuiSuccess(Page);
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return LayuiError("系统出了个小差！");
        }

        [MemberMenu("FlowProjectFormula", "edit")]
        [HttpPost]
        public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbProjectFormula> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
            try {
                bool b;
                if (request.Data.Id > 0) {
                    b = await _memberFlowApplication.EditProjectFormula(request);
                } else {
                    b = await _memberFlowApplication.AddProjectFormula(request);
                }
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowProjectFormula", "delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
        {
            if (MemberDto.AllowProject(request.Data.Id.Value) == false) { return LayuiError("系统出了个小差！"); }
            try {
                var b = await _memberFlowApplication.DeleteProjectFormula(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowProjectFormula", "edit")]
        [HttpPost]
        public async Task<IActionResult> CheckAllFormulas([FromBody, FromForm] Request<MemberIdDto> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return LayuiError("系统出了个小差！"); }

            try {
                var b = await _memberFlowApplication.CheckedAllProjectFormula(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowProjectFormula", "edit")]
        [HttpPost]
        public async Task<IActionResult> UncheckFormulas([FromBody, FromForm] Request<GetProjectFormulaListDto> request)
        {
            try {
                var b = await _memberFlowApplication.UncheckedProjectFormulaByName(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowProjectFormula", "edit")]
        [HttpPost]
        public async Task<IActionResult> ReplaceFormulas([FromBody, FromForm] Request<MemberIdDto> request)
        {
            try {
                var b = await _memberFlowGraphApplication.GraphUpdateFormulas(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }
    }
}