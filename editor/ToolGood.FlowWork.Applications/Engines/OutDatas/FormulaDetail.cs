using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolGood.FlowWork.Applications.Engines.OutDatas
{
	public sealed class FormulaDetail
	{
		public List<FormulaDetailItem> Exps { get; set; }

		public List<FormulaDetailItem> Items { get; set; }
	}

	public sealed class FormulaDetailItem
	{
		public string Name { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public List<IFormulaItem> Formulas { get; set; }

		public string Value { get; set; }
		public string Type { get; set; }

		[System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string NodeId { get; set; }

		[JsonIgnore]
		public int Layer { get; set; }

		public void SetMaxLayer(int layer)
		{
			if (layer > Layer) {
				Layer = layer;
			}
		}
	}

	public class FormulaItemConverter : JsonConverter<IFormulaItem>
	{
		public override IFormulaItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, IFormulaItem value, JsonSerializerOptions options)
		{
			if (value is null) { writer.WriteNullValue(); return; }
			if (value is TextItem textItem) {
				JsonSerializer.Serialize(writer, textItem, options);
			} else if (value is FormulaItem formulaItem) {
				JsonSerializer.Serialize(writer, formulaItem, options);
			}
		}
	}

	[JsonConverter(typeof(FormulaItemConverter))]
	public interface IFormulaItem
	{
	}

	public sealed class TextItem : IFormulaItem
	{
		public string Text { get; set; }
	}

	public sealed class FormulaItem : IFormulaItem
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}
}