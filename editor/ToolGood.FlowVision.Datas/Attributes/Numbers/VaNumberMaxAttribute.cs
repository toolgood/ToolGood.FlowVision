/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	public class VaNumberMaxAttribute : VaNumberAttribute
	{
		public VaNumberMaxAttribute(double maximum) : base(double.MinValue, maximum)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberMaxAttribute(int maximum) : base(int.MinValue, maximum)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberMaxAttribute(decimal maximum) : base(decimal.MinValue, maximum)
		{
		}
	}
}