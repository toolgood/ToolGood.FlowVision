/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// MAC地址验证
	/// </summary>
	public class VaMacAddressAttribute : RegularExpressionAttribute
	{
		private const string MacCheck = @"^[0-9A-F]{2}([-:][0-9A-F]{2}){5,6}$";

		/// <summary>
		/// MAC地址验证
		/// </summary>
		public VaMacAddressAttribute()
			: base(MacCheck)
		{
			ErrorMessage = "MAC地址格式不正确;";
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