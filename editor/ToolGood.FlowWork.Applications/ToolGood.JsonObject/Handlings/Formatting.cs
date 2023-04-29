using ToolGood.JsonObject.Commens;

namespace ToolGood.JsonObject.Handlings
{
	/// <summary>
	/// Specifies formatting options for the <see cref="JsonTextWriter"/>.
	/// </summary>
	public enum Formatting
	{
		/// <summary>
		/// No special formatting is applied. This is the default.
		/// </summary>
		None = 0,

		/// <summary>
		/// Causes child objects to be indented according to the <see cref="JsonTextWriter.Indentation"/> and <see cref="JsonTextWriter.IndentChar"/> settings.
		/// </summary>
		Indented = 1
	}
}