using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowWork.Commons.My;
using ToolGood.FlowWork.Commons.Utils;

namespace ToolGood.FlowWork.Commons.Controllers
{
	public abstract class PageModelBaseCore : PageModel
	{
		//public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		//{
		//    //HttpContextHelper.BuildQueryArgs(context.HttpContext, context.HandlerArguments);
		//    base.OnPageHandlerExecuting(context);
		//}
		//public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
		//{
		//    base.OnPageHandlerExecuted(context);
		//}

		#region Success

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Success(object obj)
		{
			return ActionResultUtil.Success(obj);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objs"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Success<T>(List<T> objs)
		{
			return ActionResultUtil.Success(objs);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Success(string msg = "SUCCESS")
		{
			return ActionResultUtil.Success(msg);
		}

		#endregion Success

		#region Error

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Error(string msg = "ERROR")
		{
			return ActionResultUtil.Error(msg);
		}

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="code"></param>
		/// <param name="msg"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Error(int code, string msg)
		{
			return ActionResultUtil.Error(code, msg);
		}

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		protected IActionResult Error(object obj)
		{
			return ActionResultUtil.Error(obj);
		}

		#endregion Error

		#region Other CreatePassword GetUserAgent MapPath

		/// <summary>
		/// 获取 UserAgent
		/// </summary>
		/// <returns></returns>
		protected string GetUserAgent()
		{
			return HttpContext.Request.Headers[HeaderNames.UserAgent].ToString();
		}

		/// <summary>
		/// 获取文件绝对路径
		/// </summary>
		/// <param name="path">文件路径</param>
		/// <returns></returns>
		protected string MapPath(string path)
		{
			return MyHostingEnvironment.MapPath(path);
		}

		/// <summary>
		/// 获取文件绝对路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected string MapWebRootPath(string path)
		{
			return MyHostingEnvironment.MapWebRootPath(path);
		}

		#endregion Other CreatePassword GetUserAgent MapPath

		#region Session

		/// <summary>
		/// 获取 SessionId
		/// </summary>
		/// <returns></returns>
		protected string GetSessionId()
		{
			return HttpContext.Session.Id;
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		protected void SetSession(string key, string val)
		{
			HttpContext.Session.Set(key, Encoding.UTF8.GetBytes(val));
		}

		/// <summary>
		/// 设置Session
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		protected void SetSession(string key, object value)
		{
			HttpContext.Session.SetString(key, JsonUtil.SerializeObject(value));
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected string GetSession(string key)
		{
			return HttpContext.Session.GetString(key);
		}

		/// <summary>
		/// 判断session是否存在key
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected bool HasSession(string key)
		{
			return HttpContext.Session.Keys.Any(q => q == key);
		}

		/// <summary>
		/// 获取Session
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		protected T GetSession<T>(string key)
		{
			var value = HttpContext.Session.GetString(key);
			return value == null ? default : JsonUtil.DeserializeObject<T>(value);
		}

		/// <summary>
		/// 依据key删除Session
		/// </summary>
		/// <param name="key"></param>
		protected void DeleteSession(string key)
		{
			HttpContext.Session.Remove(key);
		}

		/// <summary>
		/// 核对Session
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <returns></returns>
		protected bool CheckSession(string key, string val)
		{
			var sessionCode = HttpContext.Session.GetString(key);
			if (string.IsNullOrEmpty(sessionCode) || sessionCode != val) {
				return false;
			}
			return true;
		}

		#endregion Session

		#region Cookie 操作

		/// <summary>
		/// 获取 Cookie
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected string GetCookie(string key)
		{
			return HttpContext.Request.Cookies[key];
		}

		/// <summary>
		/// 设置 Cookie
		/// </summary>
		/// <param name="key"></param>
		/// <param name="val"></param>
		protected void SetCookie(string key, string val)
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
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="minutes"></param>
		protected void SetCookie(string key, string val, int minutes)
		{
			HttpContext.Response.Cookies.Append(key, val, new CookieOptions() {
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
		/// <param name="key"></param>
		/// <param name="val"></param>
		/// <param name="dateTime"></param>
		protected void SetCookie(string key, string val, DateTime dateTime)
		{
			HttpContext.Response.Cookies.Append(key, val, new CookieOptions() {
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
		/// <param name="cookieName"></param>
		protected void DeleteCookie(string cookieName)
		{
			var val = HttpContext.Request.Cookies[cookieName];
			if (val != null) {
				HttpContext.Response.Cookies.Append(cookieName, "", new CookieOptions() {
					Path = "/",
					Expires = DateTime.Now.AddYears(-1),
					IsEssential = true,
					HttpOnly = true,
					//Secure = true,  //非https会无效
					//SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None, //猎豹浏览器 ajax 请求无效
				});
			}
		}

		/// <summary>
		/// 判断  Cookie是否包含cookieName
		/// </summary>
		/// <param name="cookieName"></param>
		/// <returns></returns>
		protected bool HasCookie(string cookieName)
		{
			return HttpContext.Request.Cookies.ContainsKey(cookieName);
		}

		#endregion Cookie 操作

		#region JumpUrl

		/// <summary>
		/// 跳转Url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		protected IActionResult JumpTopUrl(string url)
		{
			return ActionResultUtil.JumpTopUrl(url);
		}

		/// <summary>
		/// 跳转Url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		protected IActionResult JumpUrl(string url)
		{
			return ActionResultUtil.JumpUrl(url);
		}

		#endregion JumpUrl

		#region 获取真ip

		/// <summary>
		/// 获取真ip
		/// </summary>
		/// <returns></returns>
		protected string GetRealIP()
		{
			return GetRealIP(HttpContext);
		}

		#endregion 获取真ip

		#region private

		/// <summary>
		/// 获取真ip
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private static string GetRealIP(HttpContext context)
		{
			try {
				var t = context.Request.Headers;
			} catch (Exception) {
				return "";
			}

			string result = context.Request.Headers["HTTP_X_FORWARDED_FOR"];
			//可能有代理
			if (!string.IsNullOrWhiteSpace(result)) {
				//没有"." 肯定是非IP格式
				if (result.IndexOf(".") == -1) {
					result = null;
				} else {
					//有","，估计多个代理。取第一个不是内网的IP。
					if (result.IndexOf(",") != -1) {
						result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

						string[] temparyip = result.Split(",;".ToCharArray());

						if (temparyip != null && temparyip.Length > 0) {
							for (int i = 0; i < temparyip.Length; i++) {
								//找到不是内网的地址
								if (IsIPAddress(temparyip[i]) && temparyip[i].Substring(0, 3) != "10." && temparyip[i].Substring(0, 7) != "192.168" && temparyip[i].Substring(0, 7) != "172.16.") {
									return temparyip[i];
								}
							}
						}
					}
					//代理即是IP格式
					else if (IsIPAddress(result)) {
						return result;
					}
					//代理中的内容非IP
					else {
						result = null;
					}
				}
			}

			if (string.IsNullOrWhiteSpace(result)) {
				result = context.Request.Headers["REMOTE_ADDR"];
			}

			if (string.IsNullOrWhiteSpace(result)) {
				result = context.Connection.RemoteIpAddress.ToString();
			}
			return result;
		}

		private static bool IsIPAddress(string str)
		{
			if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
				return false;

			string regformat = @"^(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})";
			Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);

			return regex.IsMatch(str);
		}

		#endregion private
	}
}