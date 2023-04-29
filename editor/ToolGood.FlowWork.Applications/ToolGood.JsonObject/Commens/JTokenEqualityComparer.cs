﻿namespace ToolGood.JsonObject.Commens
{
	/// <summary>
	/// Compares tokens to determine whether they are equal.
	/// </summary>
	public class JTokenEqualityComparer : IEqualityComparer<JToken>
	{
		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <see cref="JToken"/> to compare.</param>
		/// <param name="y">The second object of type <see cref="JToken"/> to compare.</param>
		/// <returns>
		/// <c>true</c> if the specified objects are equal; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals(JToken x, JToken y)
		{
			return JToken.DeepEquals(x, y);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="ArgumentNullException">The type of <paramref name="obj"/> is a reference type and <paramref name="obj"/> is <c>null</c>.</exception>
		public int GetHashCode(JToken obj)
		{
			if (obj == null) {
				return 0;
			}

			return obj.GetDeepHashCode();
		}
	}
}