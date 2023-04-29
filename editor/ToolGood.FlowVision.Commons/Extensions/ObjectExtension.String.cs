using System.Text;
using System.Text.RegularExpressions;

namespace System
{
	/// <summary>
	/// 字符串扩展方法
	/// </summary>
	public static partial class ObjectExtension
	{
		#region 字符串扩展

		/// <summary>忽略大小写的字符串开始比较，判断是否以任意一个待比较字符串开始</summary>
		/// <param name="value">字符串</param>
		/// <param name="strs">待比较字符串数组</param>
		/// <returns></returns>
		public static Boolean IsStartsInWithIgnoreCase(this String value, params String[] strs)
		{
			if (String.IsNullOrEmpty(value)) return false;

			foreach (var item in strs) {
				if (value.StartsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
			}
			return false;
		}

		/// <summary>忽略大小写的字符串结束比较，判断是否以任意一个待比较字符串结束</summary>
		/// <param name="value">字符串</param>
		/// <param name="strs">待比较字符串数组</param>
		/// <returns></returns>
		public static Boolean IsEndsInWithIgnoreCase(this String value, params String[] strs)
		{
			if (String.IsNullOrEmpty(value)) return false;

			foreach (var item in strs) {
				if (value.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
			}
			return false;
		}

		/// <summary>
		/// 忽略大小写的字符串结束比较，判断是否包含任意一个待比较字符串
		/// </summary>
		/// <param name="value"></param>
		/// <param name="strs"></param>
		/// <returns></returns>
		public static bool IsContainsInWithIgnoreCase(this string value, params string[] strs)
		{
			if (String.IsNullOrEmpty(value)) return false;
			var lowStr = value.ToLowerInvariant();
			foreach (var item in strs) {
				if (value.Contains(item.ToLowerInvariant())) {
					return true;
				}
			}
			return false;
		}

		/// <summary>忽略大小写的字符串开始比较，判断是否以任意一个待比较字符串开始</summary>
		/// <param name="value">字符串</param>
		/// <param name="strs">待比较字符串数组</param>
		/// <returns></returns>
		public static Boolean IsStartsInWithIgnoreCase(this String value, IEnumerable<string> strs)
		{
			if (String.IsNullOrEmpty(value)) return false;

			foreach (var item in strs) {
				if (value.StartsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
			}
			return false;
		}

		/// <summary>忽略大小写的字符串结束比较，判断是否以任意一个待比较字符串结束</summary>
		/// <param name="value">字符串</param>
		/// <param name="strs">待比较字符串数组</param>
		/// <returns></returns>
		public static Boolean IsEndsInWithIgnoreCase(this String value, IEnumerable<string> strs)
		{
			if (String.IsNullOrEmpty(value)) return false;

			foreach (var item in strs) {
				if (value.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
			}
			return false;
		}

		/// <summary>
		/// 忽略大小写的字符串结束比较，判断是否包含任意一个待比较字符串
		/// </summary>
		/// <param name="value"></param>
		/// <param name="strs"></param>
		/// <returns></returns>
		public static bool IsContainsInWithIgnoreCase(this string value, IEnumerable<string> strs)
		{
			if (String.IsNullOrEmpty(value)) return false;
			var lowStr = value.ToLowerInvariant();
			foreach (var item in strs) {
				if (value.Contains(item.ToLowerInvariant())) {
					return true;
				}
			}
			return false;
		}

		/// <summary>
		///     忽略大小写比较
		/// </summary>
		/// <param name="data">字符串</param>
		/// <param name="compareData">比较字符串</param>
		/// <returns>是否相等</returns>
		public static bool CompareIgnoreCase(this string data, string compareData)
		{
			return string.Equals(data, compareData, StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		///     清除字符串内空格
		///     <para>eg:StringHelper.ClearBlanks(" 11 22 33 44  ");==>11223344</para>
		/// </summary>
		/// <param name="data">需要处理的字符串</param>
		/// <returns>处理的字符串</returns>
		public static string ClearBlanks(this string data)
		{
			var count = data.Length;
			var builder = new StringBuilder(count);

			for (var i = 0; i < count; i++) {
				var tmp = data[i];

				if (!char.IsWhiteSpace(tmp)) builder.Append(tmp);
			}

			return builder.ToString();
		}

		#endregion 字符串扩展

		#region 空判断

		/// <summary>
		/// 对家是否为空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNullOrEmpty(this string inputStr)
		{
			return string.IsNullOrEmpty(inputStr);
		}

		/// <summary>
		/// 对家是否为空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNullOrWhiteSpace(this string inputStr)
		{
			return string.IsNullOrWhiteSpace(inputStr);
		}

		/// <summary>
		/// 对家是否为空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsNotSet(this string inputStr)
		{
			return string.IsNullOrWhiteSpace(inputStr);
		}

		/// <summary>
		/// 对家是否不为空
		/// </summary>
		/// <param name="inputStr"></param>
		/// <returns></returns>
		public static bool IsSet(this string inputStr)
		{
			return string.IsNullOrWhiteSpace(inputStr) == false;
		}

		#endregion 空判断

		#region 替换字符串

		/// <summary>
		/// 替换字符串
		/// </summary>
		/// <param name="inputStr"></param>
		/// <param name="oldStr"></param>
		/// <param name="newStr"></param>
		/// <returns></returns>
		public static string TryReplace(this string inputStr, string oldStr, string newStr)
		{
			return inputStr.IsNullOrEmpty() ? inputStr : inputStr.Replace(oldStr, newStr);
		}

		/// <summary>
		/// 替换字符串
		/// </summary>
		/// <param name="inputStr"></param>
		/// <param name="pattern"></param>
		/// <param name="replacement"></param>
		/// <returns></returns>
		public static string RegexReplace(this string inputStr, string pattern, string replacement)
		{
			return inputStr.IsNullOrEmpty() ? inputStr : Regex.Replace(inputStr, pattern, replacement);
		}

		/// <summary>
		/// 替换字符串
		/// </summary>
		/// <param name="inputStr"></param>
		/// <param name="pattern"></param>
		/// <param name="replaceFunc"></param>
		/// <returns></returns>
		public static string RegexReplace(this string inputStr, string pattern, Func<Match, string> replaceFunc)
		{
			return inputStr.IsNullOrEmpty() ? inputStr : Regex.Replace(inputStr, pattern, new MatchEvaluator(replaceFunc));
		}

		#endregion 替换字符串

		#region 字符格式化

		/// <summary>
		/// 字符格式化
		/// </summary>
		/// <param name="input"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static string Format(this string input, params object[] param)
		{
			if (input.IsNullOrWhiteSpace()) {
				throw new ArgumentNullException("input");
			}

			var result = string.Format(input, param);
			return result;
		}

		/// <summary>
		/// 字符格式化
		/// </summary>
		/// <param name="input"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public static string Format(this string input, IEnumerable<object> param)
		{
			if (input.IsNullOrWhiteSpace()) {
				throw new ArgumentNullException("input");
			}

			var result = string.Format(input, param);
			return result;
		}

		///// <summary>
		///// object 转换为自定义格式的字符串
		///// </summary>
		///// <param name="obj">目标对象</param>
		///// <param name="format">自定义格式</param>
		///// <param name="defaultValue">object为null时的默认输出</param>
		///// <returns></returns>
		//public static string Format(this T obj, string format, string defaultValue = "") where T
		//{
		//    bool flag = obj == null || string.IsNullOrWhiteSpace(obj.ToString());
		//    string result;
		//    if (flag) {
		//        result = defaultValue;
		//    } else {
		//        bool flag2 = string.IsNullOrWhiteSpace(format);
		//        if (flag2) {
		//            result = obj.ToString();
		//        } else {
		//            decimal tmpDecimal = 0m;
		//            bool flag3 = decimal.TryParse(obj.ToString(), out tmpDecimal);
		//            if (flag3) {
		//                result = tmpDecimal.ToString(format, null);
		//            } else {
		//                IFormattable formattableObj = obj as IFormattable;
		//                bool flag4 = formattableObj == null;
		//                if (flag4) {
		//                    result = obj.ToString();
		//                } else {
		//                    result = formattableObj.ToString(format, null);
		//                }
		//            }
		//        }
		//    }
		//    return result;
		//}

		#endregion 字符格式化

		#region 格式化文本

		/// <summary>
		/// 格式化电话
		/// </summary>
		/// <param name="mobile"></param>
		/// <returns></returns>
		public static string FmtMobile(this string mobile)
		{
			if (!mobile.IsNullOrEmpty() && mobile.Length > 7) {
				var regx = new Regex(@"(?<=\d{3}).+(?=\d{4})", RegexOptions.IgnoreCase);
				mobile = regx.Replace(mobile, "****");
			}

			return mobile;
		}

		/// <summary>
		/// 格式化证件号码
		/// </summary>
		/// <param name="idCard"></param>
		/// <returns></returns>
		public static string FmtIdCard(this string idCard)
		{
			if (!idCard.IsNullOrEmpty() && idCard.Length > 10) {
				var regx = new Regex(@"(?<=\w{6}).+(?=\w{4})", RegexOptions.IgnoreCase);
				idCard = regx.Replace(idCard, "********");
			}

			return idCard;
		}

		/// <summary>
		/// 格式化银行卡号
		/// </summary>
		/// <param name="bankCard"></param>
		/// <returns></returns>
		public static string FmtBankCard(this string bankCard)
		{
			if (!bankCard.IsNullOrEmpty() && bankCard.Length > 4) {
				var regx = new Regex(@"(?<=\d{4})\d+(?=\d{4})", RegexOptions.IgnoreCase);
				bankCard = regx.Replace(bankCard, " **** **** ");
			}

			return bankCard;
		}

		#endregion 格式化文本

		/// <summary>
		///     Converts line endings in the string to <see cref="Environment.NewLine" />.
		/// </summary>
		public static string NormalizeLineEnter(this string str)
		{
			return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
		}

		#region 左/右 字符串截取

		/// <summary>
		///     Gets a substring of a string from beginning of the string.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="len" /> is bigger that string's length</exception>
		public static string Left(this string str, int len)
		{
			if (str == null) {
				throw new ArgumentNullException("str");
			}

			if (str.Length < len) {
				throw new ArgumentException("len argument can not be bigger than given string's length!");
			}

			return str.Substring(0, len);
		}

		/// <summary>
		///     Gets a substring of a string from end of the string.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="len" /> is bigger that string's length</exception>
		public static string Right(this string str, int len)
		{
			if (str == null) {
				throw new ArgumentNullException("str");
			}

			if (str.Length < len) {
				throw new ArgumentException("len argument can not be bigger than given string's length!");
			}

			return str.Substring(str.Length - len, len);
		}

		/// <summary>
		///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
		public static string Truncate(this string str, int maxLength)
		{
			if (str == null) {
				return null;
			}

			if (str.Length <= maxLength) {
				return str;
			}

			return str.Left(maxLength);
		}

		/// <summary>
		///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
		///     It adds a "..." postfix to end of the string if it's truncated.
		///     Returning string can not be longer than maxLength.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
		public static string TruncateWithPostfix(this string str, int maxLength)
		{
			return TruncateWithPostfix(str, maxLength, "...");
		}

		/// <summary>
		///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
		///     It adds given <paramref name="postfix" /> to end of the string if it's truncated.
		///     Returning string can not be longer than maxLength.
		/// </summary>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
		public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
		{
			if (str == null) {
				return null;
			}

			if (str == string.Empty || maxLength == 0) {
				return string.Empty;
			}

			if (str.Length <= maxLength) {
				return str;
			}

			if (maxLength <= postfix.Length) {
				return postfix.Left(maxLength);
			}

			return str.Left(maxLength - postfix.Length) + postfix;
		}

		#endregion 左/右 字符串截取

		#region 分割

		/// <summary>
		///     Uses string.SplitChar method to split given string by given separator.
		/// </summary>
		public static string[] SplitChar(this string str, string separator)
		{
			return str.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>拆分字符串，过滤空格，无效时返回空数组</summary>
		/// <param name="value">字符串</param>
		/// <param name="separators">分组分隔符，默认逗号分号</param>
		/// <returns></returns>
		public static String[] Split(this String value, params String[] separators)
		{
			if (String.IsNullOrEmpty(value)) return new String[0];
			if (separators == null || separators.Length < 1 || separators.Length == 1 && separators[0].IsNullOrEmpty()) separators = new String[] { ",", ";" };

			return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		}

		/// <summary>拆分字符串成为整型数组，默认逗号分号分隔，无效时返回空数组</summary>
		/// <remarks>过滤空格、过滤无效、不过滤重复</remarks>
		/// <param name="value">字符串</param>
		/// <param name="separators">分组分隔符，默认逗号分号</param>
		/// <returns></returns>
		public static Int32[] SplitToInt(this String value, params String[] separators)
		{
			if (String.IsNullOrEmpty(value)) return new Int32[0];
			if (separators == null || separators.Length < 1) separators = new String[] { ",", ";" };

			var ss = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
			var list = new List<Int32>();
			foreach (var item in ss) {
				if (!Int32.TryParse(item.Trim(), out var id)) continue;

				// 本意只是拆分字符串然后转为数字，不应该过滤重复项
				//if (!list.Contains(id))
				list.Add(id);
			}

			return list.ToArray();
		}

		/// <summary>拆分字符串成为不区分大小写的可空名值字典。逗号分组，等号分隔</summary>
		/// <param name="value">字符串</param>
		/// <param name="nameValueSeparator">名值分隔符，默认等于号</param>
		/// <param name="separator">分组分隔符，默认分号</param>
		/// <param name="trimQuotation">去掉括号</param>
		/// <returns></returns>
		public static IDictionary<String, String> SplitToDictionary(this String value, String nameValueSeparator = "=", String separator = ";", Boolean trimQuotation = false)
		{
			var dic = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase);
			if (value.IsNullOrWhiteSpace()) return dic;

			if (nameValueSeparator.IsNullOrEmpty()) nameValueSeparator = "=";
			//if (separator == null || separator.Length < 1) separator = new String[] { ",", ";" };

			var ss = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
			if (ss == null || ss.Length < 1) return null;

			foreach (var item in ss) {
				var p = item.IndexOf(nameValueSeparator);
				if (p <= 0) continue;

				var key = item.Substring(0, p).Trim();
				if (key.IsNullOrWhiteSpace()) { continue; }

				var val = item.Substring(p + nameValueSeparator.Length).Trim();

				// 处理单引号双引号
				if (trimQuotation && !val.IsNullOrEmpty()) {
					if (val[0] == '\'' && val[val.Length - 1] == '\'') val = val.Trim('\'');
					if (val[0] == '"' && val[val.Length - 1] == '"') val = val.Trim('"');
				}

				dic[key] = val;
			}

			return dic;
		}

		#endregion 分割

		/// <summary>
		///
		/// </summary>
		/// <param name="Htmlstring"></param>
		/// <returns></returns>
		public static string RemoveHTML(this string Htmlstring)
		{
			Htmlstring = Regex.Replace(Htmlstring, @"<script[\s\S]*?</script>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<noscript[\s\S]*?</noscript>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<style[\s\S]*?</style>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<.*?>", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", " ", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", " ", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
			Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", " ", RegexOptions.IgnoreCase);
			return Htmlstring;
		}

		/// <summary>
		/// object 转换为指定的隐藏字符串
		/// </summary>
		/// <param name="obj">目标对象</param>
		/// <param name="frontLen">前面保留显示位数</param>
		/// <param name="endLen">后面保留显示位数</param>
		/// <param name="spaceModNum">每隔多少个字符增加一个空格，默认为4个字符</param>
		/// <param name="hideFormatStr">隐藏符，默认为“*”号</param>
		/// <param name="defaultValue">默认值</param>
		/// <returns></returns>
		public static string ToHideString(this string obj, int frontLen, int endLen, int spaceModNum = 4, string hideFormatStr = "*", string defaultValue = "")
		{
			bool flag = obj == null || string.IsNullOrWhiteSpace(obj.ToString());
			string result;
			if (flag) {
				result = defaultValue;
			} else {
				string targetStr = obj.ToString().Trim();
				bool flag2 = targetStr.Length <= frontLen + endLen;
				if (flag2) {
					result = targetStr;
				} else {
					StringBuilder strBuilder = new StringBuilder();
					string hideStr = string.IsNullOrWhiteSpace(hideFormatStr) ? "*" : hideFormatStr;
					for (int i = 0; i < targetStr.Length; i++) {
						bool flag3 = i < frontLen || i >= targetStr.Length - endLen;
						if (flag3) {
							bool flag4 = i != 0 && i % spaceModNum == 0;
							if (flag4) {
								strBuilder.Append(" ");
								strBuilder.Append(targetStr[i]);
							} else {
								strBuilder.Append(targetStr[i]);
							}
						} else {
							bool flag5 = i != 0 && i % spaceModNum == 0;
							if (flag5) {
								strBuilder.Append(" ");
								strBuilder.Append(hideStr);
							} else {
								strBuilder.Append(hideStr);
							}
						}
					}
					result = strBuilder.ToString();
				}
			}
			return result;
		}
	}
}