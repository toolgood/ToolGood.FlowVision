namespace System
{
	/// <summary>
	///
	/// </summary>
	public static partial class ObjectExtension
	{
		/// <summary>
		/// 对象是空
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool IsNull<T>(this T obj) where T : class
		{
			return obj == null;
		}

		/// <summary>
		/// 对象不为空
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static bool IsNotNull<T>(this T obj) where T : class
		{
			return obj != null;
		}

		/// <summary>
		///     Check if an item is in a list.
		/// </summary>
		/// <param name="item">Item to check</param>
		/// <param name="list">List of items</param>
		/// <typeparam name="T">Type of the items</typeparam>
		public static bool IsIn<T>(this T item, params T[] list)
		{
			return list.Contains(item);
		}

		/// <summary>
		///     Check if an item is in a list.
		/// </summary>
		/// <param name="item">Item to check</param>
		/// <param name="list">List of items</param>
		/// <typeparam name="T">Type of the items</typeparam>
		public static bool IsIn<T>(this T item, IEnumerable<T> list)
		{
			return list.Contains(item);
		}

		/// <summary>
		/// Linq's Select
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="t1"></param>
		/// <param name="valueFunc"></param>
		/// <returns></returns>
		public static T2 SelectNull<T1, T2>(this T1 t1, Func<T1, T2> valueFunc)
		{
			if (object.Equals(null, t1)) { return default(T2); }
			return valueFunc(t1);
		}

		/// <summary>
		/// Linq's Select
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="t1"></param>
		/// <param name="valueFunc"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static T2 SelectNull<T1, T2>(this T1 t1, Func<T1, T2> valueFunc, T2 defaultValue)
		{
			if (object.Equals(null, t1)) { return defaultValue; }
			return valueFunc(t1);
		}

		#region ToString

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this DateTime? dt, string fmt)
		{
			if (dt != null) {
				return ((DateTime)dt).ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="fmt"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static string ToString(this DateTime? dt, string fmt, DateTime @default)
		{
			if (dt != null) {
				return ((DateTime)dt).ToString(fmt);
			}
			return @default.ToString(fmt);
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="b"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static string ToString(this bool b, string trueString, string falseString = "")
		{
			return b ? trueString : falseString;
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static string ToString(this bool? b)
		{
			if (b.HasValue) {
				return b.Value.ToString();
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="b"></param>
		/// <param name="TrueString"></param>
		/// <param name="FalseString"></param>
		/// <returns></returns>
		public static string ToString(this bool? b, string TrueString, string FalseString = "")
		{
			if (b.HasValue) {
				if (b.Value) {
					return TrueString;
				}
				return FalseString;
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this short? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this int? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this long? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this ushort? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this uint? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this ulong? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this float? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this double? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		/// <summary>
		/// ToString 扩展
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static string ToString(this decimal? num, string fmt)
		{
			if (object.Equals(null, num)) {
				if (string.IsNullOrEmpty(fmt)) {
					return num.Value.ToString();
				}
				return num.Value.ToString(fmt);
			}
			return "";
		}

		#endregion ToString

		#region DeepClone

		/// <summary>
		/// 对象拷贝
		/// </summary>
		/// <param name="source">被复制对象</param>
		/// <returns>新对象</returns>
		public static T DeepClone<T>(this T source)
		{
			if (source is ICloneable) {
				return (T)(source as ICloneable).Clone();
			}
			return (T)Copy(source);
		}

		private static object Copy(object obj)
		{
			if (obj == null) return null;

			Object targetDeepCopyObj;
			Type targetType = obj.GetType();
			//值类型
			if (obj.GetType().IsValueType == true) {
				targetDeepCopyObj = obj;
			} else {
				targetDeepCopyObj = System.Activator.CreateInstance(targetType);   //创建引用对象
				System.Reflection.MemberInfo[] memberCollection = obj.GetType().GetMembers();

				foreach (System.Reflection.MemberInfo member in memberCollection) {
					if (member.MemberType == System.Reflection.MemberTypes.Field) {
						System.Reflection.FieldInfo field = (System.Reflection.FieldInfo)member;
						Object fieldValue = field.GetValue(obj);
						if (fieldValue is ICloneable) {
							field.SetValue(targetDeepCopyObj, (fieldValue as ICloneable).Clone());
						} else {
							field.SetValue(targetDeepCopyObj, Copy(fieldValue));
						}
					} else if (member.MemberType == System.Reflection.MemberTypes.Property) {
						System.Reflection.PropertyInfo myProperty = (System.Reflection.PropertyInfo)member;
						if (myProperty.CanWrite && myProperty.CanRead) {
							object propertyValue = myProperty.GetValue(obj, null);
							if (propertyValue is ICloneable) {
								myProperty.SetValue(targetDeepCopyObj, (propertyValue as ICloneable).Clone(), null);
							} else {
								myProperty.SetValue(targetDeepCopyObj, Copy(propertyValue), null);
							}
						}
					}
				}
			}
			return targetDeepCopyObj;
		}

		#endregion DeepClone
	}
}