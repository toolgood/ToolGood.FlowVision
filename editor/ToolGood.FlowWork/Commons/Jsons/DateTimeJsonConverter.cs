using System.Text.Json;

namespace ToolGood.FlowWork.Commons.Jsons
{
	/// <summary>
	/// 设置Json默认DateTime格式
	/// </summary>
	public class DateTimeJsonConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
	{
		private readonly string _Format;

		public DateTimeJsonConverter(string format)
		{
			_Format = format;
		}

		public override void Write(Utf8JsonWriter writer, DateTime date, JsonSerializerOptions options)
		{
			writer.WriteStringValue(date.ToString(_Format));
		}

		public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			try {
				return DateTime.ParseExact(reader.GetString(), _Format, null);
			} catch (Exception) { }
			return DateTime.Parse(reader.GetString());
		}
	}
}