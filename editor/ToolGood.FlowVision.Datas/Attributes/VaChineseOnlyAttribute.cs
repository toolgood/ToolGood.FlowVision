/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 中文验证
	/// </summary>
	public class VaChineseOnlyAttribute : RegularExpressionAttribute
	{
		private const string Chinese = "^[\u4E00-\u9FFF\u3400-\u4DB5\u3007\uF900-\uFAD9]+$";

		/// <summary>
		/// 中文验证
		/// </summary>
		public VaChineseOnlyAttribute()
			: base(Chinese)
		{
			ErrorMessage = "{0}应该为全部中文字！";
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var val = value.ToString();
			if (string.IsNullOrEmpty(val)) { return true; }
			return base.IsValid(value);
		}
	}
}