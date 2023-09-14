namespace ToolGood.FlowVision.Commons.My
{
	public class MyHttpContext
	{
		internal static IServiceCollection _serviceCollection;

		public static HttpContext Current {
			get
			{
				object factory = _serviceCollection.BuildServiceProvider().GetService(typeof(IHttpContextAccessor));
				HttpContext context = ((HttpContextAccessor)factory).HttpContext;
				return context;
			}
		}

		public static void SetServiceCollection(IServiceCollection serviceDescriptors)
		{
			_serviceCollection = serviceDescriptors;
		}
	}
}