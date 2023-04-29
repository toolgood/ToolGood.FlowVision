using System.Globalization;
using ToolGood.JsonObject.Handlings;
using ToolGood.JsonObject.Utilities;

namespace ToolGood.JsonObject.Commens
{
	/// <summary>
	/// Provides methods for converting between .NET types and JSON types.
	/// </summary>
	public static class JsonConvert
	{
		/// <summary>
		/// Represents JavaScript's boolean value <c>true</c> as a string. This field is read-only.
		/// </summary>
		public static readonly string True = "true";

		/// <summary>
		/// Represents JavaScript's boolean value <c>false</c> as a string. This field is read-only.
		/// </summary>
		public static readonly string False = "false";

		/// <summary>
		/// Represents JavaScript's <c>null</c> as a string. This field is read-only.
		/// </summary>
		public static readonly string Null = "null";

		/// <summary>
		/// Represents JavaScript's <c>undefined</c> as a string. This field is read-only.
		/// </summary>
		public static readonly string Undefined = "undefined";

		/// <summary>
		/// Represents JavaScript's positive infinity as a string. This field is read-only.
		/// </summary>
		public static readonly string PositiveInfinity = "Infinity";

		/// <summary>
		/// Represents JavaScript's negative infinity as a string. This field is read-only.
		/// </summary>
		public static readonly string NegativeInfinity = "-Infinity";

		/// <summary>
		/// Represents JavaScript's <c>NaN</c> as a string. This field is read-only.
		/// </summary>
		public static readonly string NaN = "NaN";

		/// <summary>
		/// Converts the <see cref="bool"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A JSON string representation of the <see cref="bool"/>.</returns>
		public static string ToString(bool value)
		{
			return value ? True : False;
		}

		/// <summary>
		/// Converts the <see cref="char"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A JSON string representation of the <see cref="char"/>.</returns>
		public static string ToString(char value)
		{
			return ToString(char.ToString(value));
		}

		internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			if (floatFormatHandling == FloatFormatHandling.Symbol || !(double.IsInfinity(value) || double.IsNaN(value))) {
				return text;
			}

			if (floatFormatHandling == FloatFormatHandling.DefaultValue) {
				return !nullable ? "0.0" : Null;
			}

			return quoteChar + text + quoteChar;
		}

		internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
		{
			return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
		}

		private static string EnsureDecimalPlace(double value, string text)
		{
			if (double.IsNaN(value) || double.IsInfinity(value) || StringUtils.IndexOf(text, '.') != -1 || StringUtils.IndexOf(text, 'E') != -1 || StringUtils.IndexOf(text, 'e') != -1) {
				return text;
			}

			return text + ".0";
		}

		private static string EnsureDecimalPlace(string text)
		{
			if (StringUtils.IndexOf(text, '.') != -1) {
				return text;
			}

			return text + ".0";
		}

		/// <summary>
		/// Converts the <see cref="sbyte"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A JSON string representation of the <see cref="sbyte"/>.</returns>
		/// <summary>
		/// Converts the <see cref="decimal"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A JSON string representation of the <see cref="decimal"/>.</returns>
		public static string ToString(decimal value)
		{
			return EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Converts the <see cref="string"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A JSON string representation of the <see cref="string"/>.</returns>
		public static string ToString(string value)
		{
			return ToString(value, '"');
		}

		/// <summary>
		/// Converts the <see cref="string"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="delimiter">The string delimiter character.</param>
		/// <returns>A JSON string representation of the <see cref="string"/>.</returns>
		public static string ToString(string value, char delimiter)
		{
			return ToString(value, delimiter, StringEscapeHandling.Default);
		}

		/// <summary>
		/// Converts the <see cref="string"/> to its JSON string representation.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="delimiter">The string delimiter character.</param>
		/// <param name="stringEscapeHandling">The string escape handling.</param>
		/// <returns>A JSON string representation of the <see cref="string"/>.</returns>
		public static string ToString(string value, char delimiter, StringEscapeHandling stringEscapeHandling)
		{
			if (delimiter != '"' && delimiter != '\'') {
				throw new ArgumentException("Delimiter must be a single or double quote.", nameof(delimiter));
			}

			return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
		}
	}
}