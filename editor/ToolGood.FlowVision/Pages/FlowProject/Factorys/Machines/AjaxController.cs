using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;

namespace ToolGood.FlowVision.Pages.Members.Factorys.Machines
{
    [Microsoft.AspNetCore.Mvc.Route("/FlowProject/Factorys/Machines/Ajax/{action}")]
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
        [MemberMenu("FlowFactoryMachines", "show")]
        [HttpPost]
        public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetFactoryMachineListDto> request)
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
                        item.FactoryCode = factory.Code;
                        item.Factory = factory.SimplifyName;
                    }
                }
                return LayuiSuccess(Page);
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return LayuiError("系统出了个小差！");
        }

        [MemberMenu("FlowFactoryMachines", "edit")]
        [HttpPost]
        public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbFactoryMachine> request)
        {
            if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }
            try {
                bool b;
                if (request.Data.Id > 0) {
                    b = await _memberFlowApplication.EditFactoryMachine(request);
                } else {
                    b = await _memberFlowApplication.AddFactoryMachine(request);
                }
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [MemberMenu("FlowFactoryMachines", "delete")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
        {
            if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

            try {
                var b = await _memberFlowApplication.DeleteFactoryMachine(request);
                if (b == false) return Error(request.Message);
                return Success();
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }

        [IgnoreAntiforgeryToken]
        [MemberMenu("FlowFactoryMachines", "show")]
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