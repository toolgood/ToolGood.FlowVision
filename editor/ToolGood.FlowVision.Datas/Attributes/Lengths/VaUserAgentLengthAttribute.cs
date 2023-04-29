/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	/// <summary>
	/// UserAgent验证
	/// </summary>
	public class VaUserAgentLengthAttribute : VaMaxLengthAttribute
	{
		/// <summary>
		/// UserAgent验证
		/// </summary>
		public VaUserAgentLengthAttribute() : base(250)
		{
		}
	}
}