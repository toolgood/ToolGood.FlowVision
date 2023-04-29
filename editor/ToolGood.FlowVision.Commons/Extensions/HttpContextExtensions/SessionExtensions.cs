using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ToolGood.FlowVision.Commons.Utils;

namespace Microsoft.AspNetCore.Extensions
{
	public static partial class HttpContextExtensions
	{
		#region HttpContext Extensions

		/// <summary>
		/// 获取 SessionId
		/// </summary>
		/// <returns></returns>
		public static string GetSessionId(this HttpContext context)
		{
			return context.Session.Id;
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static void SetSession(this HttpContext HttpContext, string key, string val)
		{
			HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(val));
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void SetSession(this HttpContext HttpContext, string key, object value)
		{
			HttpContext.Session.SetString(key, JsonUtil.SerializeObject(value));
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetSession(this HttpContext HttpContext, string key)
		{
			return HttpContext.Session.GetString(key);
		}

		/// <summary>
		/// 判断session是否存在key
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool HasSession(this HttpContext HttpContext, string key)
		{
			return HttpContext.Session.Keys.Any(q => q == key);
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T GetSession<T>(this HttpContext HttpContext, string key)
		{
			var value = HttpContext.Session.GetString(key);
			return value == null ? default(T) : JsonUtil.DeserializeObject<T>(value);
		}

		/// <summary>
		/// 依据key删除Session
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		public static void DeleteSession(this HttpContext HttpContext, string key)
		{
			HttpContext.Session.Remove(key);
		}

		/// <summary>
		/// 核对Session
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool CheckSession(this HttpContext HttpContext, string key, string val)
		{
			var sessionCode = HttpContext.Session.GetString(key);
			if (string.IsNullOrEmpty(sessionCode) || sessionCode != val) {
				return false;
			}
			return true;
		}

		#endregion HttpContext Extensions

		#region Session

		/// <summary>
		/// 获取 SessionId
		/// </summary>
		/// <returns></returns>
		public static string GetSessionId(this ActionContext actionContext)
		{
			return actionContext.HttpContext.GetSessionId();
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static void SetSession(this ActionContext actionContext, string key, string val)
		{
			actionContext.HttpContext.SetSession(key, val);
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public static void SetSession(this ActionContext actionContext, string key, object value)
		{
			actionContext.HttpContext.SetSession(key, value);
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetSession(this ActionContext actionContext, string key)
		{
			return actionContext.HttpContext.GetSession(key);
		}

		/// <summary>
		/// 判断session是否存在key
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool HasSession(this ActionContext actionContext, string key)
		{
			return actionContext.HttpContext.HasSession(key);
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T GetSession<T>(this ActionContext actionContext, string key)
		{
			return actionContext.HttpContext.GetSession<T>(key);
		}

		/// <summary>
		/// 依据key删除Session
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		public static void DeleteSession(this ActionContext actionContext, string key)
		{
			actionContext.HttpContext.DeleteSession(key);
		}

		/// <summary>
		/// 核对Session
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		public static bool CheckSession(this ActionContext actionContext, string key, string val)
		{
			return actionContext.HttpContext.CheckSession(key, val);
		}

		#endregion Session
	}
}