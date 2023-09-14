/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	public class VaUrlAttribute : RegularExpressionAttribute
	{
		private const string UrlCheck = @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+ %\$#_=]*)?$";

		public VaUrlAttribute()
			: base(UrlCheck)
		{
			ErrorMessage = "网址格式不正确！";
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var val = value.ToString();
			if (string.IsNullOrEmpty(val)) { return true; }
			if (val.Length <= 200) {
				return base.IsValid(value);
			}
			return false;
		}
	}
}