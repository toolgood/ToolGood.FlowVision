/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 时间类型
	/// </summary>
	public class VaTimeTypeAttribute : RegularExpressionAttribute
	{
		/// <summary>
		/// 时间类型
		/// </summary>
		public VaTimeTypeAttribute()
			: base(@"^(\d{4}:\d{2}(:\d{2})?|\d{4}时\d{2}分(\d{2}秒)?)$")
		{
			ErrorMessage = "{0}为时间格式";
		}
	}
}