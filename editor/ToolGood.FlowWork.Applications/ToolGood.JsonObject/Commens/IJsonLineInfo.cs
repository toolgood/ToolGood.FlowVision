namespace ToolGood.JsonObject.Commens
{
	/// <summary>
	/// Provides an interface to enable a class to return line and position information.
	/// </summary>
	public interface IJsonLineInfo
	{
		/// <summary>
		/// Gets a value indicating whether the class can return line information.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if <see cref="LineNumber"/> and <see cref="LinePosition"/> can be provided; otherwise, <c>false</c>.
		/// </returns>
		bool HasLineInfo();

		/// <summary>
		/// Gets the current line number.
		/// </summary>
		/// <value>The current line number or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
		int LineNumber { get; }

		/// <summary>
		/// Gets the current line position.
		/// </summary>
		/// <value>The current line position or 0 if no line information is available (for example, when <see cref="HasLineInfo"/> returns <c>false</c>).</value>
		int LinePosition { get; }
	}
}