using Microsoft.AspNetCore.Mvc.Filters;
using ToolGood.FlowVision.Commons.Utils;

namespace ToolGood.FlowVision.Commons.Exceptions
{
	public class HttpGlobalExceptionFilter : IExceptionFilter
	{
		public void OnException(ExceptionContext context)
		{
			LogUtil.Error(context.HttpContext, context.Exception);

			if (context.HttpContext.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase)) {
				context.Result = ActionResultUtil.Error("系统出了个小差！");
			}
		}
	}
}