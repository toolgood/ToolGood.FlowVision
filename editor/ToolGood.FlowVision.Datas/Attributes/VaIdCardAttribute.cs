/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToolGood.Attributes
{
	public class VaIdCardAttribute : ValidationAttribute
	{
		private const string IdCard = @"(^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$)|(^[1-9]\d{5}\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{2}[0-9Xx]$)";

		public override string FormatErrorMessage(string name)
		{
			return $"{name}为身份证类型！";
		}

		public override bool IsValid(object value)
		{
			if (value == null) return true;
			var val = value.ToString();
			if (string.IsNullOrEmpty(val)) { return true; }
			return IsIdCard(value.ToString());
		}

		/// <summary>
		/// 是否为有效身份证
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool IsIdCard(string value)
		{
			if (value == null)
				return false;
			var v = value.ToString();
			if (new Regex(IdCard).IsMatch(v) == false) {
				return false;
			}
			if (v.Length == 18) {
				return Check18(v);
			} else if (v.Length == 15) {
				return true;
			}
			return false;
		}

		private bool Check18(string id)
		{
			int[] weights = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
			string checks = "10X98765432";
			int val = 0;
			for (var i = 0; i < 17; i++) {
				val += (id[i] - '0') * weights[i];
			}
			return id[17] == checks[val % 11];
		}
	}
}