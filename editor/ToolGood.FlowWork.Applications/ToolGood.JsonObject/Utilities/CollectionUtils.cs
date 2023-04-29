namespace ToolGood.JsonObject.Utilities
{
	internal static class CollectionUtils
	{
		public static int IndexOfReference<T>(this List<T> list, T item)
		{
			for (int i = 0; i < list.Count; i++) {
				if (ReferenceEquals(item, list[i])) {
					return i;
				}
			}

			return -1;
		}

		public static T[] ArrayEmpty<T>()
		{
			// Enumerable.Empty<T> no longer returns an empty array in .NET Core 3.0
			return EmptyArrayContainer<T>.Empty;
		}

		private static class EmptyArrayContainer<T>
		{
#pragma warning disable CA1825 // Avoid zero-length array allocations.
			public static readonly T[] Empty = new T[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.
		}
	}
}