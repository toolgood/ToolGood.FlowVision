/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 短名称验证
	/// </summary>
	public class VaShortNameLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 短名称验证
		/// </summary>
		public VaShortNameLengthAttribute() : base(50)
		{
		}
	}
}