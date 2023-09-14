using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Commons.Controllers
{
	public abstract class ApiControllerCore : WebControllerBaseCore
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!ModelState.IsValid) {
				context.Result = Error(ModelState);
				return;
			}

			foreach (var item in context.ActionArguments) {
				if (item.Value is IRequest temp) {
					temp.Ip = GetRealIP();
					temp.UserAgent = GetUserAgent();
				}
			}

			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
			Config.Dispose();
		}

		protected IActionResult LayuiSuccess<T>(Page<T> page)
		{
			return ActionResultUtil.LayuiSuccess(page);
		}

		protected IActionResult LayuiSuccess<T>(List<T> page)
		{
			return ActionResultUtil.LayuiSuccess(page);
		}

		protected IActionResult LayuiSuccess(IDictionary dictionary)
		{
			return ActionResultUtil.LayuiSuccess(dictionary);
		}

		protected IActionResult LayuiError(string msg)
		{
			return ActionResultUtil.LayuiError(msg);
		}
	}
}