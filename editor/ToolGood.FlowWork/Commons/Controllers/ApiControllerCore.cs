using Microsoft.AspNetCore.Mvc.Filters;

namespace ToolGood.FlowWork.Commons.Controllers
{
	public abstract class ApiControllerCore : WebControllerBaseCore
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (!ModelState.IsValid) {
				context.Result = Error(ModelState);
				return;
			}

			base.OnActionExecuting(context);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			base.OnActionExecuted(context);
		}
	}
}