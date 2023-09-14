using Microsoft.AspNetCore.Mvc;
using System.Text;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Datas.Projects;
using ToolGood.FlowVision.Dtos;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowWork.Commons;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps
{
	[Route("/FlowProject/Apps/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		private readonly IMemberFlowGraphApplication _memberFlowGraphApplication;
		private readonly IMemberFlowApplication _memberFlowApplication;
		private readonly IMemberApplication _memberApplication;

		public AjaxController(IMemberFlowGraphApplication memberFlowGraphApplication, IMemberFlowApplication memberFlowApplication, IMemberApplication memberApplication)
		{
			_memberFlowGraphApplication = memberFlowGraphApplication;
			_memberFlowApplication = memberFlowApplication;
			_memberApplication = memberApplication;
		}

		[IgnoreAntiforgeryToken]
		[MemberMenu("FlowApp", "show")]
		[HttpPost]
		public async Task<IActionResult> GetList([FromBody, FromForm] Request<GetAppListDto> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return LayuiError("系统出了个小差！"); }

			try {
				var Page = await _memberFlowApplication.GetAppPage(request);
				return LayuiSuccess(Page);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return LayuiError("系统出了个小差！");
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpPost]
		public async Task<IActionResult> EditItem([FromBody, FromForm] Request<DbApp> request)
		{
			if (MemberDto.AllowProject(request.Data.ProjectId) == false) { return Error("系统出了个小差！"); }
			try {
				bool b;
				if (request.Data.Id > 0) {
					b = await _memberFlowApplication.EditApp(request);
				} else {
					b = await _memberFlowApplication.AddApp(request);
				}
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowApp", "delete")]
		[HttpPost]
		public async Task<IActionResult> DeleteItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			try {
				var b = await _memberFlowApplication.DeleteApp(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpPost]
		public async Task<IActionResult> UpdateItem([FromBody, FromForm] Request<MemberIdDto> request)
		{
			if (request.Data.ProjectId == null || MemberDto.AllowProject(request.Data.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			request.Data.AppId = request.Data.Id;
			try {
				var b = await _memberFlowGraphApplication.GraphUpdate(request);
				if (b == false) return Error(request.Message);
				return Success();
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
			}
			return Error("系统出了个小差！");
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpGet]
		public async Task<IActionResult> Build(MemberIdDto request)
		{
			if (request.ProjectId == null || MemberDto.AllowProject(request.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			try {
				var b = await _memberFlowGraphApplication.Exports(MemberDto.MainMemberId, request.ProjectId.Value);
				var json = b.SaveJson();
				await _memberFlowApplication.AddProjectFile(MemberDto.MainMemberId, request.ProjectId.Value, MemberDto.Id);
				await _memberFlowApplication.AddProjectWorkFile(MemberDto.MainMemberId, request.ProjectId.Value, json);
				return Content("生成完毕！");
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
				return Content(ex.Message);
			}
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpGet]
		public async Task<IActionResult> BuildRefresh(MemberIdDto request)
		{
			if (request.ProjectId == null || MemberDto.AllowProject(request.ProjectId.Value) == false) { return Error("系统出了个小差！"); }
			try {
				var b = await _memberFlowGraphApplication.Exports(MemberDto.MainMemberId, request.ProjectId.Value);
				var json = b.SaveJson();
				await _memberFlowApplication.AddProjectFile(MemberDto.MainMemberId, request.ProjectId.Value, MemberDto.Id);
				await _memberFlowApplication.AddProjectWorkFile(MemberDto.MainMemberId, request.ProjectId.Value, json);
				ProjectWorkCache.Update(_memberFlowApplication);
				return Content("生成完毕并更新缓存！");
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
				return Content(ex.Message);
			}
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpGet]
		public async Task<IActionResult> Exports(MemberIdDto request)
		{
			if (request.ProjectId == null || MemberDto.AllowProject(request.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

			try {
				var dt = await _memberApplication.GetRealTime();
				var b = await _memberFlowGraphApplication.Exports(MemberDto.MainMemberId, request.ProjectId.Value);
				var json = b.SaveJson();
				await _memberFlowApplication.AddProjectFile(MemberDto.MainMemberId, request.ProjectId.Value, MemberDto.Id);
				return File(Encoding.UTF8.GetBytes(json), "text/json", b.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".json");
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
				return Content(ex.Message);
			}
		}

		[MemberMenu("FlowApp", "edit")]
		[HttpGet]
		public async Task<IActionResult> ExportsUseRsa(MemberIdDto request)
		{
			if (request.ProjectId == null || MemberDto.AllowProject(request.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

			try {
				var dt = await _memberApplication.GetRealTime();
				var b = await _memberFlowGraphApplication.Exports(MemberDto.MainMemberId, request.ProjectId.Value);
				var privateKey = "<RSAKeyValue><Modulus>u3W3xI6mH9tr3A+sNZVhyIbQWFhePbPWdFeTtM39yR7kO4Akp6Dsb1NYKpKxSGjIwDv1TC6/IUwOgOYYSVa0pgfIujHPrYFO/LlDk6kPAyHluLimKFkHkze5FsY7YAqd2mExqdJ4Zfb1pXgIrVFgOs4o69p9vyBV6kWS0FBOnyyUK92bRYxeqS1raRfM3GUlIEaQW5ZIuJxQtFrfwSnsfDVhkp8rvFVt7I5aqawWeoJZu+/HZqQO/gz+BJ7ntlUWoPgfe13/U3kIOHMTc/Deczb5x3DeBv9XrwJ5+DrzrJV8jTdhiYeNcBNBYaKoHGS15chxt6no4sIDZYsI2N4ciQ==</Modulus><Exponent>AQAB</Exponent><P>xXFPDIdmFIhu7y0bXEAzWC7AVbpSYsvsz2j2bW3Hy+SNM75MK+rL4LPhQRpVfPVXYyBJH8CZy4hz9O9uYJorh38OQxqcTE/mq7GkwVhbsM2fqAvseEggLQ30LrLUsXdKynxQuRmZQ4zPu1O5oOnv8yR1hYfsyo0DAT3tfDCmSXs=</P><Q>8w55RwigC7NZ+e+Ys59TdAuMI6KiN2iM57mJsX6F4kmMyXF3t8Q2doV103RrNKyA/y6kFV498Z0hZCLO1I9zHgVVllf4+/62VieEBtyi3GzETSfWjGgx1fFzmcqFm/v4+M10kylWHC3DKzPCblggBViQH6hQ8rpOgEpMQhNCCMs=</Q><DP>BPPus7XBzcoOXlmXUh0aLKAfmwbtgiTzb9CGPgB+/pJhKGchqghdzOk2QVfTxSqyYn4w12cRdJWbsw0+i5XSebeqN+y29wMaGjx+kYsR4cGGu8RwziOhCwBKm9FInJsNeT7dF4eY3KTvoRdclLjJmCPV/t+GBR68cI+JIZBm23k=</DP><DQ>jYncLfVj3exvdRCFh/Q8ENO4o/fNJx8HDtoIyQe4x3G1PomJAcaNQK+vUZf15hKee/uZKahbwhuSmlF6yGVQ0CajFI3ePECzxa/1Plm1rU7ZcWTFl7YFb2TPwLsi2xb8gUaDoD7uGjK7+KiHjOJco0BTjVvi57Z+iE674a59Qck=</DQ><InverseQ>kvCB+Bv8dopMdNpPLRA9JedmwjCNksF6hKa84vmmHLGxpwMkuQbYpJoB9Ac/l6Fv1Uwyj5ldOP87jCEdN4DF78EuFgROcIBM7orN1EKBb+k5nv3qd2tS8yCj9ex+gnPW0jkvoX78xaiMEokF9wq9XlV9jd8JXkUFogrqbsWQo0I=</InverseQ><D>eoJuPMi1DLDLhp+/fa1IoFJqqrHltRFon9P0Nf9BUkUcBz6xtdNXcVHYlsoTjizctbT5lHYgdtRLzjWRvawqly8TIYwYG14cjtFMtp2PsA5hIR2biVbVuoJ6NcYLW7LUgrluSXvBL3H8C82sbgh2iTfeSnUaJOQvAdHCJt205BSQR6AIozs+ZSOAfAfapK9zDakpifOEJ9qs3kNGLlZZaZZBPtHr/rlEZo9DA6i+2VIK33UEwGNpQby/UaBDne5FcdX9BxSVd1mL2I6nKlYWerp9jT3agej5JWJFhvZ165PbLZbLDL27cZiWx2beEiG7P1G/dOleflFCra7IvRz8uQ==</D></RSAKeyValue>";
				var json = b.SaveJsonWithRsa(privateKey);
				await _memberFlowApplication.AddProjectFile(MemberDto.MainMemberId, request.ProjectId.Value, MemberDto.Id);
				return File(json, "text/json", b.Name + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".data");
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
				return Content(ex.Message);
			}
		}


        [MemberMenu("FlowApp", "edit")]
        [HttpGet]
        public async Task<IActionResult> EnumerationConditions(MemberIdDto request)
        {
            if (MemberDto.AllowProject(request.ProjectId.Value) == false) { return Error("系统出了个小差！"); }

            try {
                //var dt = await _memberApplication.GetRealTime();
                //if (MachineCode.GetLicense().Check(dt) != LicenseType.Effective) {
                //    return Error("无法导出项目，请订阅服务！");
                //}


                var b = await _memberFlowGraphApplication.EnumerationConditions(MemberDto.MainMemberId, request.ProjectId.Value);
                return Success(b);
            } catch (Exception ex) {
                LogUtil.Error(HttpContext, ex);
            }
            return Error("系统出了个小差！");
        }
    }
}