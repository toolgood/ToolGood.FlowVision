using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Extensions
{
	public static partial class HttpContextExtensions
	{
		#region HttpContext Extensions

		/// <summary>
		/// 获取 Cookie
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetCookie(this HttpContext HttpContext, string key)
		{
			return HttpContext.Request.Cookies[key];
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static void SetCookie(this HttpContext HttpContext, string key, string val)
		{
			HttpContext.Response.Cookies.Append(key, val, new CookieOptions() {
				Path = "/",
				IsEssential = true,
				HttpOnly = true,
				//Secure = true, //非https会无效
				//SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, //猎豹浏览器 ajax 请求无效
			});
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="minutes"></param>
		public static void SetCookie(this HttpContext HttpContext, string key, string val, int minutes)
		{
			HttpContext.Response.Cookies.Append(key, val, new Microsoft.AspNetCore.Http.CookieOptions() {
				Path = "/",
				Expires = DateTime.Now.AddMinutes(minutes),
				IsEssential = true,
				HttpOnly = true,
				//Secure = true, //非https会无效
				//SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, //猎豹浏览器 ajax 请求无效
			});
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="dateTime"></param>
		public static void SetCookie(this HttpContext HttpContext, string key, string val, DateTime dateTime)
		{
			HttpContext.Response.Cookies.Append(key, val, new Microsoft.AspNetCore.Http.CookieOptions() {
				Path = "/",
				Expires = dateTime,
				IsEssential = true,
				HttpOnly = true,
				//Secure = true,  //非https会无效
				//SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, //猎豹浏览器 ajax 请求无效
			});
		}

		/// <summary>
		/// 依据cookieName 删除 cookie
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="cookieName"></param>
		public static void DeleteCookie(this HttpContext HttpContext, string cookieName)
		{
			var val = HttpContext.Request.Cookies[cookieName];
			if (val != null) {
				SetCookie(HttpContext, cookieName, "", DateTime.Now.AddYears(-1));
			}
		}

		/// <summary>
		/// 判断  Cookie是否包含cookieName
		/// </summary>
		/// <param name="HttpContext"></param>
		/// <param name="cookieName"></param>
		/// <returns></returns>
		public static bool HasCookie(this HttpContext HttpContext, string cookieName)
		{
			return HttpContext.Request.Cookies.ContainsKey(cookieName);
		}

		#endregion HttpContext Extensions

		#region ActionContext Extensions

		/// <summary>
		/// 获取 Cookie
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetCookie(this ActionContext actionContext, string key)
		{
			return actionContext.HttpContext.GetCookie(key);
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		public static void SetCookie(this ActionContext actionContext, string key, string val)
		{
			actionContext.HttpContext.SetCookie(key, val);
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="minutes"></param>
		public static void SetCookie(this ActionContext actionContext, string key, string val, int minutes)
		{
			actionContext.HttpContext.SetCookie(key, val, minutes);
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="dateTime"></param>
		public static void SetCookie(this ActionContext actionContext, string key, string val, DateTime dateTime)
		{
			actionContext.HttpContext.SetCookie(key, val, dateTime);
		}

		/// <summary>
		/// 依据cookieName 删除 cookie
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="cookieName"></param>
		public static void DeleteCookie(this ActionContext actionContext, string cookieName)
		{
			actionContext.HttpContext.DeleteCookie(cookieName);
		}

		/// <summary>
		/// 判断  Cookie是否包含cookieName
		/// </summary>
		/// <param name="actionContext"></param>
		/// <param name="cookieName"></param>
		/// <returns></returns>
		public static bool HasCookie(this ActionContext actionContext, string cookieName)
		{
			return actionContext.HttpContext.HasCookie(cookieName);
		}

		#endregion ActionContext Extensions
	}
}