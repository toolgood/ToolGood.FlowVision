/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 日期类型验证
	/// </summary>
	public class VaDateTimeTypeAttribute : RegularExpressionAttribute
	{
		private const string DateTime = @"^((\d{4})[-/](1[012]|\d)[-/](3[01]|[12]?\d) (2[0-3]|[01]?\d):([0-5]?\d)(:([0-5]?\d)?|)|\d{4}年(1[012]|\d)月(3[01]|[12]?\d)日(2[0-3]|[01]?\d)时[0-5]?\d分([0-5]?\d秒)?)$";

		/// <summary>
		/// 日期类型
		/// </summary>
		public VaDateTimeTypeAttribute()
			: base(DateTime)
		{
			ErrorMessage = "{0}为日期时间类型";
		}
	}
}