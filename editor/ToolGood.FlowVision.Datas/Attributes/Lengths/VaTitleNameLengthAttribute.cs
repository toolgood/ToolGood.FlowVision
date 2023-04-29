/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// 标题名称长度验证
	/// </summary>
	public class VaTitleNameLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// 标题名称长度验证
		/// </summary>
		public VaTitleNameLengthAttribute() : base(100)
		{
		}
	}
}