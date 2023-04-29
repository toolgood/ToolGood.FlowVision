namespace ToolGood.FlowVision.Commons.My
{
	public static class MyIoc
	{
		//internal static IServiceCollection _serviceCollection;
		internal static IServiceProvider _serviceProvider;

		public static void SetServiceProvider(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public static T Create<T>()
		{
			using (var serviceCope = _serviceProvider.CreateScope()) {
				return serviceCope.ServiceProvider.GetService<T>();
			}

			//return _serviceProvider.GetRequiredService<T>();
			//return _serviceCollection.BuildServiceProvider().GetService<T>();
		}
	}
}