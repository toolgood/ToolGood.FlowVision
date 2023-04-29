/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 用户名验证
	/// </summary>
	public class VaUserNameAttribute : RegularExpressionAttribute
	{
		/// <summary>
		/// 用户名验证
		/// </summary>
		public VaUserNameAttribute()
			: base("^[a-zA-Z0-9_]{2,20}$")
		{
			ErrorMessage = "用户名只能由26个英文字母、0－9的数字、下划线“_”组成，不可以使用其他字符;";
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var val = value.ToString();
			if (string.IsNullOrEmpty(val)) { return true; }
			if (val.Length <= 20) {
				return base.IsValid(value);
			}
			return false;
		}
	}
}