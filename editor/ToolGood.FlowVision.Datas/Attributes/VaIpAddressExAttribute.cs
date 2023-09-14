/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToolGood.Attributes
{
	/// <summary>
	/// IP地址扩展验证
	/// </summary>
	public class VaIpAddressExAttribute : ValidationAttribute
	{
		private static Regex IPv6 = new Regex(@"^((([\da-f]{1,4}:){7}([\da-f]{1,4}|:))|(([\da-f]{1,4}:){6}(:[\da-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([\da-f]{1,4}:){5}(((:[\da-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([\da-f]{1,4}:){4}(((:[\da-f]{1,4}){1,3})|((:[\da-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([\da-f]{1,4}:){3}(((:[\da-f]{1,4}){1,4})|((:[\da-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([\da-f]{1,4}:){2}(((:[\da-f]{1,4}){1,5})|((:[\da-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([\da-f]{1,4}:){1}(((:[\da-f]{1,4}){1,6})|((:[\da-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[\da-f]{1,4}){1,7})|((:[\da-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(/[1-9]\d*)?$"
								, RegexOptions.Compiled | RegexOptions.IgnoreCase);

		private static Regex IPv4 = new Regex(@"^((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.){3}(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(/[1-9]\d*)?$", RegexOptions.Compiled);

		/// <summary>
		/// IP地址验证
		/// </summary>
		public VaIpAddressExAttribute()
		{
			ErrorMessage = "IP地址验证错误";
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var ip = value.ToString();
			if (string.IsNullOrEmpty(ip)) { return true; }

			if (IPv4.IsMatch(ip)) { return true; }
			if (IPv6.IsMatch(ip)) { return true; }
			return false;
		}
	}
}