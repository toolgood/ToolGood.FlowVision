using System.Text.RegularExpressions;

namespace System
{
	/// <summary>
	///
	/// </summary>
	public static partial class ObjectExtension
	{
		/// <summary>
		/// 将object安全的转为int类型
		/// </summary>
		/// <param name="o">要转换的值</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns>int值</returns>

		public static int ToSafeInt32(this string o, int defaultValue)
		{
			bool flag = o != null && !string.IsNullOrWhiteSpace(o.ToString());// && Regex.IsMatch(o.ToString().Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
			if (flag) {
				string s = o.ToString().Trim().ToLower();
				string a = s;
				if (a == "true") {
					return 1;
				}
				if (a == "false") {
					return 0;
				}
				int num;
				bool flag2 = int.TryParse(s, out num);
				if (flag2) {
					return num;
				}
			}
			return defaultValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToSafeInt64(this string o, int defaultValue)
		{
			bool flag = o != null && !string.IsNullOrWhiteSpace(o.ToString());// && Regex.IsMatch(o.ToString().Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
			if (flag) {
				string s = o.ToString().Trim().ToLower();
				string a = s;
				if (a == "true") {
					return 1L;
				}
				if (a == "false") {
					return 0L;
				}
				long num;
				bool flag2 = long.TryParse(s, out num);
				if (flag2) {
					return num;
				}
			}
			return (long)defaultValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static float ToSafeFloat(this string o, float defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			float result2;
			if (flag) {
				result2 = defValue;
			} else {
				float result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					float.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static double ToSafeDouble(this string o, double defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			double result2;
			if (flag) {
				result2 = defValue;
			} else {
				double result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					double.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static decimal ToSafeDecimal(this string o, decimal defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			decimal result2;
			if (flag) {
				result2 = defValue;
			} else {
				decimal result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					decimal.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		/// 将object安全的转为int类型
		/// </summary>
		/// <param name="o">要转换的值</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns>int值</returns>

		public static int ToSafeInt32(this IComparable o, int defaultValue)
		{
			bool flag = o != null && !string.IsNullOrWhiteSpace(o.ToString());// && Regex.IsMatch(o.ToString().Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
			if (flag) {
				string s = o.ToString().Trim().ToLower();
				string a = s;
				if (a == "true") {
					return 1;
				}
				if (a == "false") {
					return 0;
				}
				int num;
				bool flag2 = int.TryParse(s, out num);
				if (flag2) {
					return num;
				}
			}
			return defaultValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static long ToSafeInt64(this IComparable o, int defaultValue)
		{
			bool flag = o != null && !string.IsNullOrWhiteSpace(o.ToString());// && Regex.IsMatch(o.ToString().Trim(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
			if (flag) {
				string s = o.ToString().Trim().ToLower();
				string a = s;
				if (a == "true") {
					return 1L;
				}
				if (a == "false") {
					return 0L;
				}
				long num;
				bool flag2 = long.TryParse(s, out num);
				if (flag2) {
					return num;
				}
			}
			return (long)defaultValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static float ToSafeFloat(this IComparable o, float defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			float result2;
			if (flag) {
				result2 = defValue;
			} else {
				float result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					float.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static double ToSafeDouble(this IComparable o, double defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			double result2;
			if (flag) {
				result2 = defValue;
			} else {
				double result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					double.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static decimal ToSafeDecimal(this IComparable o, decimal defValue)
		{
			bool flag = o == null || string.IsNullOrWhiteSpace(o.ToString());
			decimal result2;
			if (flag) {
				result2 = defValue;
			} else {
				decimal result = defValue;
				bool flag2 = o != null && Regex.IsMatch(o.ToString(), "^([-]|[0-9])[0-9]*(\\.\\w*)?$");
				if (flag2) {
					decimal.TryParse(o.ToString(), out result);
				}
				result2 = result;
			}
			return result2;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static bool ToSafeBool(this string o, bool defValue)
		{
			bool flag = o != null;
			if (flag) {
				bool flag2 = string.Compare(o.ToString(), "1") == 0;
				if (flag2) {
					return true;
				}
				bool flag3 = string.Compare(o.ToString(), "0") == 0;
				if (flag3) {
					return false;
				}
				bool flag4 = string.Compare(o.ToString().Trim(), "true", true) == 0;
				if (flag4) {
					return true;
				}
				bool flag5 = string.Compare(o.ToString().Trim(), "false", true) == 0;
				if (flag5) {
					return false;
				}
			}
			return defValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="o"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		public static bool ToSafeBool(this IComparable o, bool defValue)
		{
			bool flag = o != null;
			if (flag) {
				bool flag2 = string.Compare(o.ToString(), "1") == 0;
				if (flag2) {
					return true;
				}
				bool flag3 = string.Compare(o.ToString(), "0") == 0;
				if (flag3) {
					return false;
				}
				bool flag4 = string.Compare(o.ToString().Trim(), "true", true) == 0;
				if (flag4) {
					return true;
				}
				bool flag5 = string.Compare(o.ToString().Trim(), "false", true) == 0;
				if (flag5) {
					return false;
				}
			}
			return defValue;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static DateTime ToSafeDateTime(this string obj, DateTime defaultValue)
		{
			bool flag = obj == null || string.IsNullOrWhiteSpace(obj.ToString());
			DateTime result2;
			if (flag) {
				result2 = defaultValue;
			} else {
				DateTime result;
				result2 = (DateTime.TryParse(obj.ToString(), out result) ? result : defaultValue);
			}
			return result2;
		}

		/// <summary>
		/// object ToString转化
		/// </summary>
		/// <param name="obj">object</param>
		/// <returns></returns>

		public static string ToSafeString(this object obj)
		{
			bool flag = obj == null;
			string result;
			if (flag) {
				result = "";
			} else {
				result = obj.ToString();
			}
			return result;
		}

		#region ToSafeEnum

		/// <summary>
		/// 字符串转枚举
		/// </summary>
		/// <typeparam name="T">输入</typeparam>
		/// <param name="str"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static T ToSafeEnum<T>(this IComparable str, T t) where T : struct
		{
			return Enum.TryParse<T>(str.ToString(), out var result) ? result : t;
			//return (T)Enum.ToObject(typeof(T), str);
		}

		/// <summary>
		/// 字符串转枚举
		/// </summary>
		/// <typeparam name="T">输入</typeparam>
		/// <param name="str"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static T ToSafeEnum<T>(this string str, T t) where T : struct
		{
			return Enum.TryParse<T>(str, out var result) ? result : t;
		}

		/// <summary>
		///     Converts string to enum value.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <returns>Returns enum object</returns>
		public static T ToSafeEnum<T>(this string value)
			where T : struct
		{
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}

			return (T)Enum.Parse(typeof(T), value);
		}

		/// <summary>
		///     Converts string to enum value.
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <param name="ignoreCase">Ignore case</param>
		/// <returns>Returns enum object</returns>
		public static T ToSafeEnum<T>(this string value, bool ignoreCase)
			where T : struct
		{
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}

			return (T)Enum.Parse(typeof(T), value, ignoreCase);
		}

		#endregion ToSafeEnum

		/// <summary>
		/// 转成类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ConvertTo<T>(this object value)
		{
			var obj = ConvertTo(value, typeof(T));
			return (T)obj;
		}

		/// <summary>
		/// 转成类型
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public static object ConvertTo(this object value, Type type)
		{
			if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
			if (value == null) return null;
			if (type == value.GetType()) return value;
			if (type.IsEnum) {
				if (value is string)
					return Enum.Parse(type, value as string);
				else
					return Enum.ToObject(type, value);
			}
			if (!type.IsInterface && type.IsGenericType) {
				Type innerType = type.GetGenericArguments()[0];
				object innerValue = ConvertTo(value, innerType);
				return Activator.CreateInstance(type, new object[] { innerValue });
			}
			if (value is string && type == typeof(Guid)) return new Guid(value as string);
			if (value is string && type == typeof(Version)) return new Version(value as string);
			if (!(value is IConvertible)) return value;
			return Convert.ChangeType(value, type);
		}
	}
}