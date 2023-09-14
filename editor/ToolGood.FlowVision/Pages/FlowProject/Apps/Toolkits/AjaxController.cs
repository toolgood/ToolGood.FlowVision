using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowWork.Applications.Engines;

namespace ToolGood.FlowVision.Pages.FlowProject.Apps.Toolkits
{
	[Microsoft.AspNetCore.Mvc.Route("/FlowProject/Apps/Toolkits/Ajax/{action}")]
	public class AjaxController : MemberApiController
	{
		[MemberMenu("FlowApp", "edit")]
		[HttpPost]
		public IActionResult JsDebug(string js, string json, string attachData)
		{
			try {
				JsDebugEngine jsDebugEngine = new JsDebugEngine();
				jsDebugEngine.SetInput(json);
				jsDebugEngine.SetAttachData(attachData);
				jsDebugEngine.EvaluateJs(js);

				var result = jsDebugEngine.GetTempData();
				return Success(result);
			} catch (Exception ex) {
				LogUtil.Error(HttpContext, ex);
				return Error(ex.Message);
			}
		}
	}
}