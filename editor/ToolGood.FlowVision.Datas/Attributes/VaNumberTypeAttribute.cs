/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 数字类型验证
	/// </summary>
	public class VaNumberTypeAttribute : RegularExpressionAttribute
	{
		/// <summary>
		/// 数字类型验证
		/// </summary>
		public VaNumberTypeAttribute()
			: base(@"^-?\d+(\.\d+)$")
		{
			ErrorMessage = "{0}为数字类型";
		}
	}
}