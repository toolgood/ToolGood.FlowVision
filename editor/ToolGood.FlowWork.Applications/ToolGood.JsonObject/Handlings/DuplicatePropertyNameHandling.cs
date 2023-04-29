using ToolGood.JsonObject.Exceptions;

namespace ToolGood.JsonObject.Handlings
{
	/// <summary>
	/// Specifies how duplicate property names are handled when loading JSON.
	/// </summary>
	public enum DuplicatePropertyNameHandling
	{
		/// <summary>
		/// Replace the existing value when there is a duplicate property. The value of the last property in the JSON object will be used.
		/// </summary>
		Replace = 0,

		/// <summary>
		/// Ignore the new value when there is a duplicate property. The value of the first property in the JSON object will be used.
		/// </summary>
		Ignore = 1,

		/// <summary>
		/// Throw a <see cref="JsonReaderException"/> when a duplicate property is encountered.
		/// </summary>
		Error = 2
	}
}