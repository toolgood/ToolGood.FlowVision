/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 密码长度验证
	/// </summary>
	public class VaPasswordLengthAttribute : VaMinLengthAttribute
	{
		/// <summary>
		/// 密码长度验证
		/// </summary>
		public VaPasswordLengthAttribute() : base(4)
		{
			ErrorMessage = "密码必须大于4位字符。";
		}
	}
}