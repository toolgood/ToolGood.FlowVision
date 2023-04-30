using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos;
using Newtonsoft.Json.Linq;

namespace ToolGood.FlowVision.Pages.FlowProject.Projects.Datas
{
    [Microsoft.AspNetCore.Mvc.Route("/FlowProject/Projects/Datas/Ajax/{action}")]
    public class AjaxController : MemberApiController
    {
        private readonly IMemberFlowApplication _memberFlowApplication;

        public AjaxController(IMemberFlowApplication memberFlowApplication)
        {
            _memberFlowApplication = memberFlowApplication;
        }

        [IgnoreAntiforgeryToken]
        [MemberMenu("FlowProjectData", "show")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetProjectDataListDto> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
            try {
                var Page = await _memberFlowApplication.GetProjectDataPage(request);
                return LayuiSuccess(Page);
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return LayuiError("系统出了个小差！");
        }

        [MemberMenu("FlowProjectData", "edit")]
        [HttpPost]
        public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbProjectData> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }
			try {
				if (string.IsNullOrWhiteSpace(request.Data.Data) == false) {
					JObject.Parse(request.Data.Data);
				}
			} catch {
				return Error("参数(json格式)有误！");
			}

			try {
                bool b;
                if (request.Data.Id > 0) {
                    b = await _memberFlowApplication.EditProjectData(request);
                } else {
                    b = await _memberFlowApplication.AddProjectData(request);
                }
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowProjectData", "delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
        {
            if (MemberDto.AllowProject(request.Data.Id.Value) == false) { return LayuiError("系统出了个小差！"); }

            try {
                var b = await _memberFlowApplication.DeleteProjectData(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

    }
}
