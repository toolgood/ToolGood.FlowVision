using ToolGood.JsonObject.Commens;

namespace ToolGood.JsonObject.Exceptions
{
	/// <summary>
	/// The exception thrown when an error occurs while writing JSON text.
	/// </summary>
	public class JsonWriterException : JsonException
	{
		/// <summary>
		/// Gets the path to the JSON where the error occurred.
		/// </summary>
		/// <value>The path to the JSON where the error occurred.</value>
		public string Path { get; }

		public JsonWriterException(string message, string path, Exception innerException)
			: base(message, innerException)
		{
			Path = path;
		}

		internal static JsonWriterException Create(JsonWriter writer, string message, Exception ex)
		{
			return Create(writer.ContainerPath, message, ex);
		}

		internal static JsonWriterException Create(string path, string message, Exception ex)
		{
			message = JsonPosition.FormatMessage(null, path, message);

			return new JsonWriterException(message, path, ex);
		}
	}
}