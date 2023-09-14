using System.Text.Json;

namespace ToolGood.FlowVision.Commons.Jsons
{
	public class BoolJsonConverter : System.Text.Json.Serialization.JsonConverter<bool>
	{
		public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number) {
				return reader.GetInt32() == 1;
			}
			if (reader.TokenType == JsonTokenType.String) {
				return bool.Parse(reader.GetString());
			}
			return reader.GetBoolean();
		}

		public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}