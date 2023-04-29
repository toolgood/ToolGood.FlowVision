using System.Globalization;
using System.Text;

namespace ToolGood.JsonObject.Utilities
{
	internal static class StringUtils
	{
		public const char CarriageReturn = '\r';
		public const char LineFeed = '\n';
		public const char Tab = '\t';

		public static string FormatWith(this string format, IFormatProvider provider, object arg0)
		{
			return format.FormatWith(provider, new object[] { arg0 });
		}

		public static string FormatWith(this string format, IFormatProvider provider, object arg0, object arg1)
		{
			return format.FormatWith(provider, new object[] { arg0, arg1 });
		}

		private static string FormatWith(this string format, IFormatProvider provider, params object[] args)
		{
			// leave this a private to force code to use an explicit overload
			// avoids stack memory being reserved for the object array
			ValidationUtils.ArgumentNotNull(format, nameof(format));

			return string.Format(provider, format, args);
		}

		public static bool IsWhiteSpace(string s)
		{
			if (s == null) {
				throw new ArgumentNullException(nameof(s));
			}

			if (s.Length == 0) {
				return false;
			}

			for (int i = 0; i < s.Length; i++) {
				if (!char.IsWhiteSpace(s[i])) {
					return false;
				}
			}

			return true;
		}

		public static StringWriter CreateStringWriter(int capacity)
		{
			StringBuilder sb = new StringBuilder(capacity);
			StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);

			return sw;
		}

		public static void ToCharAsUnicode(char c, char[] buffer)
		{
			buffer[0] = '\\';
			buffer[1] = 'u';
			buffer[2] = MathUtils.IntToHex(c >> 12 & '\x000f');
			buffer[3] = MathUtils.IntToHex(c >> 8 & '\x000f');
			buffer[4] = MathUtils.IntToHex(c >> 4 & '\x000f');
			buffer[5] = MathUtils.IntToHex(c & '\x000f');
		}

		public static bool IsHighSurrogate(char c)
		{
			return char.IsHighSurrogate(c);
		}

		public static bool IsLowSurrogate(char c)
		{
			return char.IsLowSurrogate(c);
		}

		public static int IndexOf(string s, char c)
		{
			return s.IndexOf(c, StringComparison.Ordinal);
		}
	}
}