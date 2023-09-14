using Microsoft.AspNetCore.Html;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace ToolGood.FlowVision.Commons.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static partial class ObjectExtensions
	{
		#region ToHtml

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="replaceEnter"></param>
		/// <param name="useHtml"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this string txt, bool replaceEnter = false, bool useHtml = false)
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			if (useHtml) {
				txt = System.Web.HttpUtility.HtmlEncode(txt);
			}
			if (replaceEnter) {
				txt = txt.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
			}
			return new HtmlString(txt);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this bool b, string trueString, string falseString = "")
		{
			var txt = b ? trueString : falseString;
			return new HtmlString(txt);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this DateTime? dt, string fmt)
		{
			if (dt != null) {
				return new HtmlString(dt.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="dt"></param>
		/// <param name="fmt"></param>
		/// <param name="default"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this DateTime? dt, string fmt, DateTime @default)
		{
			if (dt != null) {
				return new HtmlString(dt.Value.ToString(fmt));
			}
			return new HtmlString(@default.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this bool? b)
		{
			if (b.HasValue) {
				return new HtmlString(b.Value.ToString());
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="TrueString"></param>
		/// <param name="FalseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this bool? b, string TrueString, string FalseString = "")
		{
			if (b.HasValue) {
				if (b.Value) {
					return new HtmlString(TrueString);
				}
				return new HtmlString(FalseString);
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this short num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this int num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this long num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this ushort num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this uint num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this ulong num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this float num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this double num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this decimal num, string fmt = null)
		{
			if (string.IsNullOrEmpty(fmt)) {
				return new HtmlString(num.ToString());
			}
			return new HtmlString(num.ToString(fmt));
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this short? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this int? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this long? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this ushort? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this uint? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this ulong? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this float? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this double? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="num"></param>
		/// <param name="fmt"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this decimal? num, string fmt = null)
		{
			if (num.HasValue) {
				if (string.IsNullOrEmpty(fmt)) {
					return new HtmlString(num.Value.ToString());
				}
				return new HtmlString(num.Value.ToString(fmt));
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="fmt"></param>
		/// <param name="FalseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtml(this object txt, string fmt = null, string FalseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }
			if (txt is string) {
				var txt2 = txt.ToString().Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "<br />");
				return new HtmlString(txt2);
			}
			if (string.IsNullOrEmpty(fmt) && string.IsNullOrEmpty(FalseString)) {
				return new HtmlString(txt.ToString());
			}

			#region UInt16 UInt32 UInt64 Int16 Int32 Int64

			if (txt is UInt16) {
				var val = (UInt16)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is UInt32) {
				var val = (UInt32)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is UInt64) {
				var val = (UInt64)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int16) {
				var val = (Int16)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int32) {
				var val = (Int32)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int64) {
				var val = (Int64)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}

			#endregion UInt16 UInt32 UInt64 Int16 Int32 Int64

			#region Single Double Decimal

			if (txt is Single) {
				var val = (Single)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Double) {
				var val = (Double)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Decimal) {
				var val = (Decimal)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}

			#endregion Single Double Decimal

			#region UInt16 UInt32 UInt64 Int16 Int32 Int64

			if (txt is UInt16?) {
				var val = (UInt16?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is UInt32) {
				var val = (UInt32?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is UInt64) {
				var val = (UInt64?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int16) {
				var val = (Int16?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int32) {
				var val = (Int32?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Int64) {
				var val = (Int64?)txt;
				return new HtmlString(val.ToString(fmt));
			}

			#endregion UInt16 UInt32 UInt64 Int16 Int32 Int64

			#region Single Double Decimal

			if (txt is Single?) {
				var val = (Single?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Double?) {
				var val = (Double?)txt;
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is Decimal?) {
				var val = (Decimal?)txt;
				return new HtmlString(val.ToString(fmt));
			}

			#endregion Single Double Decimal

			#region DateTime DateTime?

			if (txt is DateTime) {
				var val = (DateTime)txt;
				if (string.IsNullOrEmpty(fmt)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt));
			}
			if (txt is DateTime?) {
				var val = (DateTime?)txt;
				return new HtmlString(val.ToString(fmt));
			}

			#endregion DateTime DateTime?

			#region bool

			if (txt is bool) {
				var val = (bool)txt;
				if (string.IsNullOrEmpty(fmt) && string.IsNullOrEmpty(FalseString)) { return new HtmlString(val.ToString()); }
				return new HtmlString(val.ToString(fmt, FalseString));
			}
			if (txt is bool?) {
				var val = (bool?)txt;
				return new HtmlString(val.ToString(fmt, FalseString));
			}

			#endregion bool

			return new HtmlString(txt.ToString());
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalString"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this string txt, string equalString, string trueString, string falseString = "")
		{
			var t = txt == equalString ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int16 txt, Int16 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int32 txt, Int32 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int64 txt, Int64 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt16 txt, UInt16 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt32 txt, UInt32 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt64 txt, UInt64 equalValue, string trueString, string falseString = "")
		{
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Single txt, Single equalValue, string trueString, string falseString = "")
		{
			var t = Math.Round(txt - equalValue, 10) == 0.0f ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Double txt, Double equalValue, string trueString, string falseString = "")
		{
			var t = Math.Round(txt - equalValue, 10) == 0.0d ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Decimal txt, Decimal equalValue, string trueString, string falseString = "")
		{
			var t = Math.Round(txt - equalValue, 10) == 0.0m ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int16? txt, Int16 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }
			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int32? txt, Int32 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Int64? txt, Int64 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt16? txt, UInt16 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt32? txt, UInt32 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this UInt64? txt, UInt64 equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = txt == equalValue ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Single? txt, Single equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = Math.Round(txt.Value - equalValue, 10) == 0.0f ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Double? txt, Double equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = Math.Round(txt.Value - equalValue, 10) == 0.0d ? trueString : falseString;
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalValue"></param>
		/// <param name="trueString"></param>
		/// <param name="falseString"></param>
		/// <returns></returns>
		public static HtmlString ToHtmlWhenIs(this Decimal? txt, Decimal equalValue, string trueString, string falseString = "")
		{
			if (object.Equals(null, txt)) { return new HtmlString(""); }

			var t = Math.Round(txt.Value - equalValue, 10) == 0.0m ? trueString : falseString;
			return new HtmlString(t);
		}

		#endregion ToHtml

		#region ToUrlHtml

		/// <summary>
		/// 转成 Url 的 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static HtmlString ToUrlHtml(this string txt)
		{
			if (object.Equals(null, txt)) {
				return new HtmlString("");
				//return new HtmlString(HttpUtility.UrlEncode("/"));
			}
			return new HtmlString(HttpUtility.UrlEncode(txt));
		}

		/// <summary>
		/// 转成 Url 的 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static HtmlString ToUrlHtml(this object txt)
		{
			if (object.Equals(null, txt)) {
				return new HtmlString("");
				//return new HtmlString(HttpUtility.UrlEncode("/"));
			}
			return new HtmlString(HttpUtility.UrlEncode(txt.ToString()));
		}

		#endregion ToUrlHtml

		#region ToOption

		private static string HtmlEncode(this string txt)
		{
			if (string.IsNullOrEmpty(txt)) {
				return "";
			}
			return System.Web.HttpUtility.HtmlEncode(txt);
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static HtmlString ToOption(this IEnumerable<int> list)
		{
			string html = "<option value=\"{0}\">{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (var item in list) {
				sb.AppendFormat(html, item, item);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <param name="list"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption(this IEnumerable<int> list, int value)
		{
			string html = "<option value=\"{0}\" {2}>{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (var item in list) {
				var selected = item == value ? "selected" : "";
				sb.AppendFormat(html, item, item, selected);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public static HtmlString ToOption(this IEnumerable<string> list)
		{
			string html = "<option value=\"{0}\">{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (var item in list) {
				sb.AppendFormat(html, item.HtmlEncode(), item);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <param name="list"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption(this IEnumerable<string> list, string value)
		{
			string html = "<option value=\"{0}\" {2}>{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (var item in list) {
				var selected = item == value ? "selected" : "";
				sb.AppendFormat(html, item.HtmlEncode(), item, selected);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// Value 为显示的值  Key为传送的值
		/// </summary>
		/// <param name="dict"></param>
		/// <returns></returns>
		public static HtmlString ToOption<TKey, TValue>(this IDictionary<TKey, TValue> dict)
		{
			string html = "<option value=\"{0}\">{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<TKey, TValue> item in dict) {
				sb.AppendFormat(html, item.Key, item.Value.ToSafeString().HtmlEncode());
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// Value 为显示的值  Key为传送的值
		/// </summary>
		/// <param name="dict"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey value)
		{
			string html = "<option value=\"{0}\" {2}>{1}</option>";
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<TKey, TValue> item in dict) {
				var selected = object.Equals(item.Key, value) ? "selected" : "";
				sb.AppendFormat(html, item.Key, item.Value.ToSafeString().HtmlEncode(), selected);
			}
			return new HtmlString(sb.ToString());
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="show"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T>(this IEnumerable<T> list, Expression<Func<T, string>> show) where T : class
		{
			List<string> tlist = new List<string>();
			foreach (var item in list) {
				var k = show.Compile()(item);
				tlist.Add(k);
			}
			return tlist.ToOption();
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="show"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T>(this IEnumerable<T> list, Expression<Func<T, int>> show) where T : class
		{
			List<string> tlist = new List<string>();
			foreach (var item in list) {
				var k = show.Compile()(item);
				tlist.Add(k.ToString());
			}
			return tlist.ToOption();
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="show"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T>(this IEnumerable<T> list, Expression<Func<T, string>> show, string value) where T : class
		{
			List<string> tlist = new List<string>();
			foreach (var item in list) {
				var k = show.Compile()(item);
				tlist.Add(k);
			}
			return tlist.ToOption(value);
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="show"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T>(this IEnumerable<T> list, Expression<Func<T, int>> show, int value) where T : class
		{
			List<string> tlist = new List<string>();
			foreach (var item in list) {
				var k = show.Compile()(item);
				tlist.Add(k.ToString());
			}
			return tlist.ToOption(value.ToString());
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="list"></param>
		/// <param name="showFun"></param>
		/// <param name="valueFun"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T, TKey, TValue>(this IEnumerable<T> list,
			Expression<Func<T, TValue>> showFun, Expression<Func<T, TKey>> valueFun) where T : class
		{
			Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
			foreach (var item in list) {
				var s = showFun.Compile()(item);
				var v = valueFun.Compile()(item);
				dict[v] = s;
			}
			return dict.ToOption();
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="list"></param>
		/// <param name="showFun"></param>
		/// <param name="valueFun"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOption<T, TKey, TValue>(this IEnumerable<T> list,
			Expression<Func<T, TValue>> showFun, Expression<Func<T, TKey>> valueFun, TKey value) where T : class
		{
			Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
			foreach (var item in list) {
				var s = showFun.Compile()(item);
				var v = valueFun.Compile()(item);
				dict[v] = s;
			}
			return dict.ToOption(value);
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="show"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> show)
			where T : class
		{
			return ToOptionWithGroup(list, group, show, show, new string[0]);
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="show"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> show, string value)
			where T : class
		{
			return ToOptionWithGroup(list, group, show, show, new string[1] { value });
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="show"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> show, string[] values)
			where T : class
		{
			return ToOptionWithGroup(list, group, show, show, values);
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="key"></param>
		/// <param name="show"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> key, Expression<Func<T, string>> show)
			where T : class
		{
			return ToOptionWithGroup(list, group, key, show, new string[0] { });
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="key"></param>
		/// <param name="show"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> key, Expression<Func<T, string>> show, string value)
			where T : class
		{
			return ToOptionWithGroup(list, group, key, show, new string[1] { value });
		}

		/// <summary>
		/// 转成 Option 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="group"></param>
		/// <param name="key"></param>
		/// <param name="show"></param>
		/// <param name="vvalues"></param>
		/// <returns></returns>
		public static HtmlString ToOptionWithGroup<T>(this IEnumerable<T> list, Expression<Func<T, string>> group, Expression<Func<T, string>> key, Expression<Func<T, string>> show, string[] vvalues)
			where T : class
		{
			List<Tuple<string, string, string>> groups = new List<Tuple<string, string, string>>();
			foreach (var item in list) {
				var g = group.Compile()(item);
				if (g == null) g = "";
				var k = key.Compile()(item);
				var s = show.Compile()(item);
				groups.Add(Tuple.Create(g, k, s));
			}
			return toOption(groups, vvalues);
		}

		/// <summary>
		/// list 结构为  1 Group 2 Show 3 Value
		/// </summary>
		/// <param name="list"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		private static HtmlString toOption(List<Tuple<string, string, string>> list, string[] values)
		{
			var gs = list.Select(q => q.Item1).OrderBy(q => q).Distinct().ToList();
			StringBuilder sb = new StringBuilder();
			string html = "<option value=\"{1}\">{0}</option>";
			if (values.Length > 0) html = "<option value=\"{1}\" {2}>{0}</option>";

			foreach (var g in gs) {
				if (g != "") sb.AppendFormat("<optgroup label=\"{0}\">", g.HtmlEncode());
				foreach (var item in list.Where(q => q.Item1 == g)) {
					var selected = values.Contains(item.Item3) ? "selected" : "";
					sb.AppendFormat(html, item.Item2.HtmlEncode(), item.Item3, selected);
				}
				if (g != "") sb.Append("</optgroup>");
			}
			return new HtmlString(sb.ToString());
		}

		#endregion ToOption

		#region ToChecked

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs<T>(this T b, T checkValue)
		{
			if (object.Equals(null, b)) {
				return new HtmlString("");
			}
			if (object.Equals(b, checkValue)) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this List<string> objs, string checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this string[] objs, string checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this List<int> objs, int checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this int[] objs, int checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this List<long> objs, long checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this long[] objs, long checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="objs"></param>
		/// <param name="value"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs<T, T2>(this List<T> objs, Func<T, T2> value, T2 checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				var val = value(item);
				if (object.Equals(val, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="objs"></param>
		/// <param name="value"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs<T, T2>(this T[] objs, Func<T, T2> value, T2 checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				var val = value(item);
				if (object.Equals(val, checkValue)) {
					return new HtmlString("checked");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this bool b)
		{
			if (b) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int16 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int32 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int64 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt16 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt32 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt64 b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Single b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Double b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Decimal b)
		{
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this bool? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b.Value) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int16? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int32? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Int64? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt16? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt32? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this UInt64? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Single? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Double? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToChecked(this Decimal? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("checked");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalString"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this string txt, string equalString)
		{
			var t = txt == equalString ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int16 b, Int16 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int32 b, Int32 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int64 b, Int64 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt16 b, UInt16 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt32 b, UInt32 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt64 b, UInt64 equalValue)
		{
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Single b, Single equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0f ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Double b, Double equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0d ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Decimal b, Decimal equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0M ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int16? b, Int16 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int32? b, Int32 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Int64? b, Int64 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt16? b, UInt16 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt32? b, UInt32 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this UInt64? b, UInt64 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Single? b, Single equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0f ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Double? b, Double equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0d ? "checked" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToCheckedWhenIs(this Decimal? b, Decimal equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0m ? "checked" : "";
			return new HtmlString(t);
		}

		#endregion ToChecked

		#region ToSelected

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs<T>(this T b, T checkValue)
		{
			if (object.Equals(null, b)) {
				return new HtmlString("");
			}
			if (object.Equals(b, checkValue)) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this List<string> objs, string checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this string[] objs, string checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this List<int> objs, int checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this int[] objs, int checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this List<long> objs, long checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 checked 的 HtmlString 类型
		/// </summary>
		/// <param name="objs"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this long[] objs, long checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				if (object.Equals(item, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="objs"></param>
		/// <param name="value"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs<T, T2>(this List<T> objs, Func<T, T2> value, T2 checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				var val = value(item);
				if (object.Equals(val, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		///
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="objs"></param>
		/// <param name="value"></param>
		/// <param name="checkValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs<T, T2>(this T[] objs, Func<T, T2> value, T2 checkValue)
		{
			foreach (var item in objs) {
				if (object.Equals(null, item)) { continue; }
				var val = value(item);
				if (object.Equals(val, checkValue)) {
					return new HtmlString("selected");
				}
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this bool b)
		{
			if (b) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int16 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int32 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int64 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt16 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt32 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt64 b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Single b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Double b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Decimal b)
		{
			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this bool? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b.Value) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int16? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int32? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Int64? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt16? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt32? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this UInt64? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Single? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Double? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <returns></returns>
		public static HtmlString ToSelected(this Decimal? b)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }

			if (b > 0) {
				return new HtmlString("selected");
			}
			return new HtmlString("");
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="txt"></param>
		/// <param name="equalString"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this string txt, string equalString)
		{
			var t = txt == equalString ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int16 b, Int16 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int32 b, Int32 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int64 b, Int64 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt16 b, UInt16 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt32 b, UInt32 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt64 b, UInt64 equalValue)
		{
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Single b, Single equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0f ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Double b, Double equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0d ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Decimal b, Decimal equalValue)
		{
			var t = Math.Round(b - equalValue, 10) == 0.0m ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int16? b, Int16 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int32? b, Int32 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Int64? b, Int64 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt16? b, UInt16 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt32? b, UInt32 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this UInt64? b, UInt64 equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = b == equalValue ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Single? b, Single equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0f ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Double? b, Double equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0f ? "selected" : "";
			return new HtmlString(t);
		}

		/// <summary>
		/// 转成 HtmlString 类型
		/// </summary>
		/// <param name="b"></param>
		/// <param name="equalValue"></param>
		/// <returns></returns>
		public static HtmlString ToSelectedWhenIs(this Decimal? b, Decimal equalValue)
		{
			if (object.Equals(null, b)) { return new HtmlString(""); }
			var t = Math.Round(b.Value - equalValue, 10) == 0.0m ? "selected" : "";
			return new HtmlString(t);
		}

		#endregion ToSelected

		//#region ToJsonHtml
		///// <summary>
		///// 转成 Json 的 HtmlString 类型
		///// </summary>
		///// <param name="obj"></param>
		///// <returns></returns>
		//public static HtmlString ToJsonHtml(this object obj)
		//{
		//    return obj.ToJson().ToHtml();
		//}
		//#endregion

		///// <summary>
		///// Linq's Select
		///// </summary>
		///// <typeparam name="T1"></typeparam>
		///// <typeparam name="T2"></typeparam>
		///// <param name="t1"></param>
		///// <param name="valueFunc"></param>
		///// <returns></returns>
		//public static T2 SelectNull<T1, T2>(this T1 t1, Func<T1, T2> valueFunc)
		//{
		//    if (object.Equals(null, t1)) { return default(T2); }
		//    return valueFunc(t1);
		//}

		///// <summary>
		///// Linq's Select
		///// </summary>
		///// <typeparam name="T1"></typeparam>
		///// <typeparam name="T2"></typeparam>
		///// <param name="t1"></param>
		///// <param name="valueFunc"></param>
		///// <param name="defaultValue"></param>
		///// <returns></returns>
		//public static T2 SelectNull<T1, T2>(this T1 t1, Func<T1, T2> valueFunc, T2 defaultValue)
		//{
		//    if (object.Equals(null, t1)) { return defaultValue; }
		//    return valueFunc(t1);
		//}
	}
}