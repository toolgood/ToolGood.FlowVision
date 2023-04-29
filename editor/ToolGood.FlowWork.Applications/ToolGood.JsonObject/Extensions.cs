using System.Globalization;
using ToolGood.JsonObject.Utilities;

namespace ToolGood.JsonObject
{
	/// <summary>
	/// Contains the LINQ to JSON extension methods.
	/// </summary>
	public static class Extensions
	{
		internal static IEnumerable<U> Values<T, U>(this IEnumerable<T> source, object key) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, nameof(source));

			if (key == null) {
				foreach (T token in source) {
					if (token is JValue value) {
						yield return value.Convert<JValue, U>();
					} else {
						foreach (JToken t in token.Children()) {
							yield return t.Convert<JToken, U>();
						}
					}
				}
			} else {
				foreach (T token in source) {
					JToken value = token[key];
					if (value != null) {
						yield return value.Convert<JToken, U>();
					}
				}
			}
		}

		internal static IEnumerable<U> Convert<T, U>(this IEnumerable<T> source) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(source, nameof(source));

			foreach (T token in source) {
				yield return token.Convert<JToken, U>();
			}
		}

		internal static U Convert<T, U>(this T token) where T : JToken
		{
			if (token == null) {
#pragma warning disable CS8653 // A default expression introduces a null value for a type parameter.
				return default;
#pragma warning restore CS8653 // A default expression introduces a null value for a type parameter.
			}

			if (token is U castValue
				// don't want to cast JValue to its interfaces, want to get the internal value
				&& typeof(U) != typeof(IComparable) && typeof(U) != typeof(IFormattable)) {
				return castValue;
			} else {
				if (!(token is JValue value)) {
					throw new InvalidCastException("Cannot cast {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, token.GetType(), typeof(T)));
				}

				if (value.Value is U u) {
					return u;
				}

				Type targetType = typeof(U);

				return (U)System.Convert.ChangeType(value.Value, targetType, CultureInfo.InvariantCulture);
			}
		}

		/// <summary>
		/// Converts the value.
		/// </summary>
		/// <typeparam name="U">The type to convert the value to.</typeparam>
		/// <param name="value">A <see cref="JToken"/> cast as a <see cref="IEnumerable{T}"/> of <see cref="JToken"/>.</param>
		/// <returns>A converted value.</returns>
		public static U Value<U>(this IEnumerable<JToken> value)
		{
			return value.Value<JToken, U>();
		}

		public static U Value<T, U>(this IEnumerable<T> value) where T : JToken
		{
			ValidationUtils.ArgumentNotNull(value, nameof(value));

			if (!(value is JToken token)) {
				throw new ArgumentException("Source value must be a JToken.");
			}

			return token.Convert<JToken, U>();
		}
	}
}