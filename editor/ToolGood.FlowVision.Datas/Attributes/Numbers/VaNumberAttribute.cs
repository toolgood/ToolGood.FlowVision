/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	public class VaNumberAttribute : RangeAttribute
	{
		public VaNumberAttribute(double minimum, double maximum) : base(minimum, maximum)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberAttribute(int minimum, int maximum) : base(minimum, maximum)
		{
		}

		/// <summary>
		/// 最大值验证特性
		/// </summary>
		/// <param name="maximum"></param>
		public VaNumberAttribute(decimal minimum, decimal maximum) : base(typeof(decimal), minimum.ToString(), maximum.ToString())
		{
		}

		public VaNumberAttribute(Type type, string minimum, string maximum) : base(type, minimum, maximum)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			if (Minimum is int) {
				if ((int)Minimum == int.MinValue) {
					return $"{name}不大于{Maximum}";
				} else {
					return $"{name}不小于{Minimum}，不大于{Maximum}";
				}
			}
			if (Minimum is double) {
				if ((int)Minimum == double.MinValue) {
					return $"{name}不大于{Maximum}";
				} else {
					return $"{name}不小于{Minimum}，不大于{Maximum}";
				}
			}
			if (decimal.TryParse(Minimum.ToString(), out decimal min)) {
				if (min == decimal.MinValue) {
					return $"{name}不大于{Maximum}";
				} else {
					return $"{name}不小于{Minimum}，不大于{Maximum}";
				}
			}

			return base.FormatErrorMessage(name);
		}
	}
}