using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ToolGood.FlowVision.Commons.Extensions
{
	/// <summary>
	/// 生命周期
	/// </summary>
	public enum LifeStyle
	{
		/// <summary>
		/// 默认
		/// </summary>
		Transient = 1,

		/// <summary>
		/// 单例
		/// </summary>
		Singleton,

		/// <summary>
		/// 在一个生命周期域中，每一个依赖或调用创建一个单一的共享的实例，且每一个不同的生命周期域，实例是唯一的，不共享的。
		/// </summary>
		PerLifetimeScope
	}

	public static class IServiceCollectionExtensions
	{
		/// <summary>
		/// 注册类型
		/// </summary>
		/// <param name="service"></param>
		/// <param name="implementationType"></param>
		/// <param name="lifeStyle">生命周期</param>
		/// <returns></returns>
		public static IServiceCollection RegisterType(this IServiceCollection service, Type implementationType, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
		{
			switch (lifeStyle) {
				case LifeStyle.Transient: service.AddTransient(implementationType); break;
				case LifeStyle.Singleton: service.AddSingleton(implementationType); break;
				case LifeStyle.PerLifetimeScope: service.AddScoped(implementationType); break;
				default: break;
			}
			return service;
		}

		/// <summary>
		/// 注册类型
		/// </summary>
		/// <param name="service"></param>
		/// <param name="serviceType"></param>
		/// <param name="implementationType"></param>
		/// <param name="lifeStyle">生命周期</param>
		/// <returns></returns>
		public static IServiceCollection RegisterType(this IServiceCollection service, Type serviceType, Type implementationType, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
		{
			switch (lifeStyle) {
				case LifeStyle.Transient: service.AddTransient(serviceType, implementationType); break;
				case LifeStyle.Singleton: service.AddSingleton(serviceType, implementationType); break;
				case LifeStyle.PerLifetimeScope: service.AddScoped(serviceType, implementationType); break;
				default: break;
			}
			return service;
		}

		/// <summary>
		/// 根据程序集注册 接口
		/// </summary>
		/// <param name="service"></param>
		/// <param name="assemblyName"></param>
		/// <param name="predicate"></param>
		/// <param name="lifeStyle"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterAssemblyInterfaces(this IServiceCollection service, string assemblyName, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.Singleton)
		{
			var assembly = Assembly.Load(assemblyName);
			return RegisterAssemblyInterfaces(service, assembly, predicate, lifeStyle);
		}

		/// <summary>
		/// 根据程序集注册 接口
		/// </summary>
		/// <param name="service"></param>
		/// <param name="assembly"></param>
		/// <param name="predicate"></param>
		/// <param name="lifeStyle"></param>
		/// <returns></returns>
		public static IServiceCollection RegisterAssemblyInterfaces(this IServiceCollection service, Assembly assembly, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
		{
			var types = assembly.GetTypes();
			foreach (var implementationType in types) {
				if (JumpType(implementationType)) { continue; }

				if (predicate != null) {
					if (predicate(implementationType) == false) {
						continue;
					}
				}
				var interfacesTypes = implementationType.GetInterfaces();
				if (interfacesTypes.Length == 1) {
					RegisterType(service, interfacesTypes[0], implementationType, lifeStyle);
				}
			}
			return service;
		}

		/// <summary>
		/// 根据程序集注册
		/// </summary>
		/// <param name="service"></param>
		/// <param name="assemblyName">程序集</param>
		/// <param name="predicate">筛选条件</param>
		/// <param name="lifeStyle">生命周期</param>
		public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection service, string assemblyName, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.Singleton)
		{
			var assembly = Assembly.Load(assemblyName);
			return RegisterAssemblyTypes(service, assembly, predicate, lifeStyle);
		}

		/// <summary>
		/// 根据程序集注册
		/// </summary>
		/// <param name="service"></param>
		/// <param name="assembly">程序集</param>
		/// <param name="predicate">筛选条件</param>
		/// <param name="lifeStyle">生命周期</param>
		/// <returns></returns>
		public static IServiceCollection RegisterAssemblyTypes(this IServiceCollection service, Assembly assembly, Func<Type, bool> predicate = null, LifeStyle lifeStyle = LifeStyle.PerLifetimeScope)
		{
			var types = assembly.GetTypes();
			foreach (var implementationType in types) {
				if (JumpType(implementationType)) { continue; }

				if (predicate != null) {
					if (predicate(implementationType) == false) {
						continue;
					}
				}
				var interfacesTypes = implementationType.GetInterfaces();
				if (interfacesTypes.Length == 1) {
					RegisterType(service, interfacesTypes[0], implementationType, lifeStyle);
				} else {
					RegisterType(service, implementationType, lifeStyle);
				}
			}
			return service;
		}

		private static bool JumpType(Type implementationType)
		{
			if (implementationType.IsClass == false) return true;

			if (implementationType.IsAbstract) return true;
			if (implementationType.IsInterface) return true;
			if (implementationType.IsImport) return true;
			if (implementationType.IsGenericType) return true;
			if (implementationType.IsSealed) return true;
			if (implementationType.IsNestedPrivate) return true;
			if (implementationType.IsNested) return true;
			if (implementationType.IsNestedPublic) return true;
			if (implementationType.IsNotPublic) return true;
			if (implementationType.IsEnum) return true;

			return false;
		}
	}
}