/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 名字验证 20位
	/// </summary>
	public class VaNameLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 名字验证 20位
		/// </summary>
		public VaNameLengthAttribute() : base(20)
		{
		}
	}
}