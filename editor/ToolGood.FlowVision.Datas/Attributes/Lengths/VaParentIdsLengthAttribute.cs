/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// ParentIds 长度验证 250位
	/// </summary>
	public class VaParentIdsLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// ParentIds 长度验证 250位
		/// </summary>
		public VaParentIdsLengthAttribute() : base(250)
		{
		}
	}
}