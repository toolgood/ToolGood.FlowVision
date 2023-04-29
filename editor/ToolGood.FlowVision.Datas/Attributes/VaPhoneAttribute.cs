/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 手机号验证
	/// </summary>
	public class VaPhoneAttribute : RegularExpressionAttribute
	{
		/// <summary>
		/// 手机号验证
		/// </summary>
		public VaPhoneAttribute()
			: base(@"^((0\d{2,3}-\d{7,8})|(1[3456789]\d{9}))$")
		{
			ErrorMessage = "手机号格式错误！";
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