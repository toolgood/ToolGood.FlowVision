/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 日期类型
	/// </summary>
	public class VaDateTypeAttribute : RegularExpressionAttribute
	{
		private const string Date = @"^((\d{4})[-/](1[012]|\d)[-/](3[01]|[12]?\d)|\d{4}年(1[012]|\d)月(3[01]|[12]?\d)日)$";

		/// <summary>
		/// 日期类型
		/// </summary>
		public VaDateTypeAttribute()
			: base(Date)
		{
			ErrorMessage = "{0}为日期类型";
		}
	}
}