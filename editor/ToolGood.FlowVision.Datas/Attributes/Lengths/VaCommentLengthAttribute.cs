/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 备注长度验证 500位
	/// </summary>
	public class VaCommentLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 备注长度验证 500位
		/// </summary>
		public VaCommentLengthAttribute() : base(500)
		{
		}
	}
}