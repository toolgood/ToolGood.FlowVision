using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolGood.FlowWork.Commons.Jsons;

namespace ToolGood.FlowWork.Commons.Utils
{
	public static class JsonUtil
	{
		private static JsonSerializerOptions Create()
		{
			var options = new JsonSerializerOptions();
			options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
			options.PropertyNameCaseInsensitive = true;
			options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			options.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
			options.Converters.Add(new BoolJsonConverter());
			options.AllowTrailingCommas = true;
			options.ReadCommentHandling = JsonCommentHandling.Skip;
			options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
			options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.WriteIndented = false;
			return options;
		}

		public static T DeserializeObject<T>(string json)
		{
			if (object.Equals(null, json)) { return default(T); }
			var options = Create();
			return JsonSerializer.Deserialize<T>(json, options);
		}

		public static T Deserialize<T>(string json)
		{
			if (object.Equals(null, json)) { return default(T); }
			var options = Create();
			return JsonSerializer.Deserialize<T>(json, options);
		}

		public static string Serialize<T>(T obj)
		{
			var options = Create();
			return JsonSerializer.Serialize(obj, options);
		}

		public static string SerializeObject<T>(T obj)
		{
			var options = Create();
			return JsonSerializer.Serialize(obj, options);
		}

		public static JsonDocument Parse<T>(string obj)
		{
			var document = JsonDocument.Parse(obj);

			return document;
		}

		/// <summary>
		/// 序列化 ,忽略指定的字段
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="ignoreNames"></param>
		/// <returns></returns>
		public static string ToJson<T>(this T obj)
		{
			var options = Create();
			return JsonSerializer.Serialize(obj, options);
		}

		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T ToObject<T>(this string json) where T : class
		{
			if (object.Equals(null, json)) { return default(T); }
			var options = Create();
			return JsonSerializer.Deserialize<T>(json, options);
		}
	}
}