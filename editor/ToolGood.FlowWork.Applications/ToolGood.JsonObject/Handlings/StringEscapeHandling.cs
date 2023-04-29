namespace ToolGood.JsonObject.Handlings
{
	public enum StringEscapeHandling
	{
		/// <summary>
		/// Only control characters (e.g. newline) are escaped.
		/// </summary>
		Default = 0,

		/// <summary>
		/// All non-ASCII and control characters (e.g. newline) are escaped.
		/// </summary>
		EscapeNonAscii = 1,

		/// <summary>
		/// HTML (&lt;, &gt;, &amp;, &apos;, &quot;) and control characters (e.g. newline) are escaped.
		/// </summary>
		EscapeHtml = 2
	}
}