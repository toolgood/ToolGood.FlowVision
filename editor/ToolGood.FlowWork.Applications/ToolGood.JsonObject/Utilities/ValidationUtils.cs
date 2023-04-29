using System.Diagnostics.CodeAnalysis;

namespace ToolGood.JsonObject.Utilities
{
	internal static class ValidationUtils
	{
		public static void ArgumentNotNull([NotNull] object value, string parameterName)
		{
			if (value == null) {
				throw new ArgumentNullException(parameterName);
			}
		}
	}
}