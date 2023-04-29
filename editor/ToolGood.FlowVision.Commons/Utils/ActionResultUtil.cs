using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolGood.FlowVision.Commons.Jsons;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Commons.Utils
{
	public static class ActionResultUtil
	{
		private static int SuccessCode { get { return 1; } }
		private static int ErrorCode { get { return 0; } }

		#region Error

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="ms"></param>
		/// <returns></returns>
		public static IActionResult Error(ModelStateDictionary ms)
		{
			List<string> sb = new List<string>();
			//获取所有错误的Key
			List<string> Keys = ms.Keys.ToList();
			//获取每一个key对应的ModelStateDictionary
			foreach (var key in Keys) {
				var errors = ms[key].Errors.ToList();
				//将错误描述添加到sb中
				foreach (var error in errors) {
					sb.Add(error.ErrorMessage);
				}
			}
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = ErrorCode;
			data["message"] = string.Join(",", sb);
			data["state"] = "ERROR";

			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="msg"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		public static IActionResult Error(string msg = "ERROR")
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = ErrorCode;
			data["message"] = msg;
			data["state"] = "ERROR";
			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="code"></param>
		/// <param name="msg"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		public static IActionResult Error(int code, string msg)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = code;
			data["message"] = msg;
			data["state"] = "ERROR";
			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回错误
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="usePassword"></param>
		/// <returns></returns>
		public static IActionResult Error(object obj)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = ErrorCode;
			data["data"] = obj;
			data["state"] = "ERROR";
			data["message"] = "ERROR";

			return CamelCaseJson(data);
		}

		#endregion Error

		#region Success

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="ignoreNames"></param>
		/// <returns></returns>
		public static IActionResult Success(object objs, List<string> status)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["data"] = objs;
			data["state"] = "SUCCESS";
			data["message"] = "SUCCESS";
			data["status"] = status;
			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="ignoreNames"></param>
		/// <returns></returns>
		public static IActionResult Success(object objs)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["data"] = objs;
			data["state"] = "SUCCESS";
			data["message"] = "SUCCESS";

			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objs"></param>
		/// <param name="ignoreNames"></param>
		/// <returns></returns>
		public static IActionResult Success<T>(List<T> objs)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["data"] = objs;
			data["state"] = "SUCCESS";
			data["message"] = "SUCCESS";

			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="page"></param>
		/// <param name="ignoreNames"></param>
		/// <returns></returns>
		public static IActionResult Success<T>(Page<T> page)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["data"] = page;
			data["state"] = "SUCCESS";
			data["message"] = "SUCCESS";

			return CamelCaseJson(data);
		}

		/// <summary>
		/// 返回成功
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static IActionResult Success(string msg = "SUCCESS")
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["message"] = msg;
			data["state"] = "SUCCESS";

			return CamelCaseJson(data);
		}

		public static IActionResult LayuiSuccess<T>(List<T> list)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["message"] = "SUCCESS";
			data["state"] = "SUCCESS";
			data["data"] = list;

			return CamelCaseJson(data);
		}

		public static IActionResult LayuiSuccess(IDictionary dictionary)
		{
			List<KeyValue> list = new List<KeyValue>();
			foreach (var key in dictionary.Keys) {
				list.Add(new KeyValue() {
					Key = ToSafeString(key),
					Value = ToSafeString(dictionary[key])
				});
			}
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["message"] = "SUCCESS";
			data["state"] = "SUCCESS";
			data["data"] = list;

			return CamelCaseJson(data);
		}

		public static IActionResult LayuiSuccess<T>(List<T> page, int total)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["message"] = "SUCCESS";
			data["state"] = "SUCCESS";
			data["data"] = page;
			data["count"] = total;
			return CamelCaseJson(data);
		}

		public static IActionResult LayuiSuccess<T>(Page<T> page)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = SuccessCode;
			data["message"] = "SUCCESS";
			data["state"] = "SUCCESS";
			data["data"] = page.Items;
			data["count"] = page.TotalItems;
			return CamelCaseJson(data);
		}

		public static IActionResult LayuiError(string msg)
		{
			Dictionary<string, object> data = new Dictionary<string, object>();
			data["code"] = ErrorCode;
			data["state"] = "ERROR";
			data["message"] = msg;
			return CamelCaseJson(data);
		}

		#endregion Success

		#region JumpTopUrl

		/// <summary>
		/// 跳转Url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static IActionResult JumpTopUrl(string url)
		{
			var content = new ContentResult();
			content.Content = $"<script>top.location.href='{url}'</script>";
			content.ContentType = "text/html";
			return content;
		}

		public static IActionResult JumpTopUrl(string url, string msg)
		{
			var content = new ContentResult();
			if (msg == null) { msg = ""; }
			msg = JsonUtil.SerializeObject(msg);

			content.Content = $"<script>var msg={msg}; console.log(msg);top.location.href='{url}'</script>";
			content.ContentType = "text/html";
			return content;
		}

		/// <summary>
		/// 跳转Url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static IActionResult JumpUrl(string url)
		{
			var content = new ContentResult();
			content.Content = $"<script>location.href='{url}'</script>";
			content.ContentType = "text/html";
			return content;
		}

		public static IActionResult JumpUrl(string url, string msg)
		{
			var content = new ContentResult();
			if (msg == null) { msg = ""; }
			msg = JsonUtil.SerializeObject(msg);

			content.Content = $"<script>var msg={msg}; console.log(msg);location.href='{url}'</script>";
			content.ContentType = "text/html";
			return content;
		}

		#endregion JumpTopUrl

		#region private

		/// <summary>
		/// 首字母小写json
		/// </summary>
		/// <param name="data">原数据</param>
		/// <param name="ignoreNames">忽略的字段名</param>
		/// <returns></returns>
		private static IActionResult CamelCaseJson(object data)
		{
			var json = ToJson(data, true, false, false);
			return new ContentResult() {
				Content = json,
				StatusCode = 200,
				ContentType = "application/json"
			};
		}

		///// <summary>
		///// 序列化 ,忽略指定的字段
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="obj"></param>
		///// <param name="camelCase"></param>
		///// <param name="indented"></param>
		///// <param name="ignoreNull"></param>
		///// <returns></returns>
		//private static string ToJson2<T>(T obj, bool camelCase, bool indented, bool ignoreNull)
		//{
		//    if (Equals(null, obj)) { return ""; }

		//    var settings = new JsonSerializerSettings();
		//    settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
		//    if (camelCase) {
		//        settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		//    }

		//    if (indented) {
		//        settings.Formatting = Formatting.Indented;
		//    }
		//    if (ignoreNull) {
		//        settings.NullValueHandling = NullValueHandling.Ignore;
		//    }
		//    settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
		//    settings.Converters.Add(new JsonCustomDoubleConvert());// json序列化时， 防止double，末尾出现小数点浮动,
		//    settings.Converters.Add(new JsonCustomDoubleNullConvert());// json序列化时， 防止double，末尾出现小数点浮动,
		//   // settings.TypeNameHandling = TypeNameHandling.Auto;

		//    return JsonConvert.SerializeObject(obj, settings);
		//}
		private static string ToJson<T>(T obj, bool camelCase, bool indented, bool ignoreNull)
		{
			if (Equals(null, obj)) { return ""; }
			JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions();
			jsonSerializerSettings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
			jsonSerializerSettings.PropertyNameCaseInsensitive = true;
			jsonSerializerSettings.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			jsonSerializerSettings.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			jsonSerializerSettings.AllowTrailingCommas = true;
			jsonSerializerSettings.ReadCommentHandling = JsonCommentHandling.Skip;
			jsonSerializerSettings.NumberHandling = JsonNumberHandling.AllowReadingFromString;
			if (camelCase) {
				jsonSerializerSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			} else {
				jsonSerializerSettings.PropertyNamingPolicy = null;
			}
			jsonSerializerSettings.WriteIndented = indented;
			if (ignoreNull) {
				jsonSerializerSettings.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
			}
			jsonSerializerSettings.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));

			return System.Text.Json.JsonSerializer.Serialize(obj, jsonSerializerSettings);
		}

		private static string ToSafeString(object obj)
		{
			bool flag = obj == null;
			string result;
			if (flag) {
				result = "";
			} else {
				result = obj.ToString();
			}
			return result;
		}

		public class KeyValue
		{
			public string Key { get; set; }
			public string Value { get; set; }
		}

		#endregion private
	}
}