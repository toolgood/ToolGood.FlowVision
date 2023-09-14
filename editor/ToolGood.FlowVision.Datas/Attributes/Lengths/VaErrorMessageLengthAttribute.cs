/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 错误信息长度验证 200位
	/// </summary>
	public class VaErrorMessageLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 错误信息长度验证 200位
		/// </summary>
		public VaErrorMessageLengthAttribute() : base(200)
		{
		}
	}
}