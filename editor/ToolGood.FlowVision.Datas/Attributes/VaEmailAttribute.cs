/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// Email 验证
	/// </summary>
	public class VaEmailAttribute : RegularExpressionAttribute
	{
		/// <summary>
		/// Email 验证
		/// </summary>
		public VaEmailAttribute()
			: base(@"^[A-Za-z0-9\u4e00-\u9fa5\u3400-\u4DB5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$")
		{
			ErrorMessage = "{0}为电子邮件类型！";
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var val = value.ToString();
			if (string.IsNullOrEmpty(val)) { return true; }
			if (val.Length <= 50) {
				return base.IsValid(value);
			}
			return false;
		}
	}
}