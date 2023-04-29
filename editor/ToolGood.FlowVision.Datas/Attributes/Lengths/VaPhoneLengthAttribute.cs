/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 手机号长度验证
	/// </summary>
	public class VaPhoneLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 手机号长度验证
		/// </summary>
		/// <param name="length"></param>
		public VaPhoneLengthAttribute() : base(20)
		{
		}
	}
}