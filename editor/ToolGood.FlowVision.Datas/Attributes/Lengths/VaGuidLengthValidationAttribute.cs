/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// Guid长度验证 40位
	/// </summary>
	public class VaGuidLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// Guid长度验证 40位
		/// </summary>
		public VaGuidLengthAttribute() : base(40)
		{
		}
	}
}