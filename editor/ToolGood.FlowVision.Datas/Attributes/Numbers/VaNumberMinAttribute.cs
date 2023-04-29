/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	public class VaNumberMinAttribute : VaNumberAttribute
	{
		public VaNumberMinAttribute(double minValue) : base(minValue, double.MaxValue)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberMinAttribute(int minValue) : base(minValue, double.MaxValue)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberMinAttribute(decimal minValue) : base(minValue, decimal.MaxValue)
		{
		}
	}
}