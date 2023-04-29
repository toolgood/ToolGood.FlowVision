using System.Globalization;
using System.Text;
using ToolGood.JsonObject.Handlings;
using ToolGood.JsonObject.Utilities;

namespace ToolGood.JsonObject.Commens
{
	internal enum JsonContainerType
	{
		None = 0,
		Object = 1,
		Array = 2,
		Constructor = 3
	}

	internal struct JsonPosition
	{
		private static readonly char[] SpecialCharacters = { '.', ' ', '\'', '/', '"', '[', ']', '(', ')', '\t', '\n', '\r', '\f', '\b', '\\', '\u0085', '\u2028', '\u2029' };

		internal JsonContainerType Type;
		internal int Position;
		internal string PropertyName;
		internal bool HasIndex;

		public JsonPosition(JsonContainerType type)
		{
			Type = type;
			HasIndex = TypeHasIndex(type);
			Position = -1;
			PropertyName = null;
		}

		internal int CalculateLength()
		{
			switch (Type) {
				case JsonContainerType.Object:
					return PropertyName!.Length + 5;

				case JsonContainerType.Array:
				case JsonContainerType.Constructor:
					return MathUtils.IntLength((ulong)Position) + 2;

				default:
					throw new ArgumentOutOfRangeException(nameof(Type));
			}
		}

		internal void WriteTo(StringBuilder sb, ref StringWriter writer, ref char[] buffer)
		{
			switch (Type) {
				case JsonContainerType.Object:
					string propertyName = PropertyName!;
					if (propertyName.IndexOfAny(SpecialCharacters) != -1) {
						sb.Append(@"['");

						if (writer == null) {
							writer = new StringWriter(sb);
						}

						JavaScriptUtils.WriteEscapedJavaScriptString(writer, propertyName, '\'', false, JavaScriptUtils.SingleQuoteCharEscapeFlags, StringEscapeHandling.Default, null, ref buffer);

						sb.Append(@"']");
					} else {
						if (sb.Length > 0) {
							sb.Append('.');
						}

						sb.Append(propertyName);
					}
					break;

				case JsonContainerType.Array:
				case JsonContainerType.Constructor:
					sb.Append('[');
					sb.Append(Position);
					sb.Append(']');
					break;
			}
		}

		internal static bool TypeHasIndex(JsonContainerType type)
		{
			return type == JsonContainerType.Array || type == JsonContainerType.Constructor;
		}

		internal static string BuildPath(List<JsonPosition> positions, JsonPosition? currentPosition)
		{
			int capacity = 0;
			if (positions != null) {
				for (int i = 0; i < positions.Count; i++) {
					capacity += positions[i].CalculateLength();
				}
			}
			if (currentPosition != null) {
				capacity += currentPosition.GetValueOrDefault().CalculateLength();
			}

			StringBuilder sb = new StringBuilder(capacity);
			StringWriter writer = null;
			char[] buffer = null;
			if (positions != null) {
				foreach (JsonPosition state in positions) {
					state.WriteTo(sb, ref writer, ref buffer);
				}
			}
			if (currentPosition != null) {
				currentPosition.GetValueOrDefault().WriteTo(sb, ref writer, ref buffer);
			}

			return sb.ToString();
		}

		internal static string FormatMessage(IJsonLineInfo lineInfo, string path, string message)
		{
			// don't add a fullstop and space when message ends with a new line
			if (!message.EndsWith(Environment.NewLine, StringComparison.Ordinal)) {
				message = message.Trim();

				if (!message.EndsWith('.')) {
					message += ".";
				}

				message += " ";
			}

			message += "Path '{0}'".FormatWith(CultureInfo.InvariantCulture, path);

			if (lineInfo != null && lineInfo.HasLineInfo()) {
				message += ", line {0}, position {1}".FormatWith(CultureInfo.InvariantCulture, lineInfo.LineNumber, lineInfo.LinePosition);
			}

			message += ".";

			return message;
		}
	}
}