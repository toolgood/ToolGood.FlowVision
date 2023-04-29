/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 字符串长度验证
	/// </summary>
	public class VaStringLengthAttribute : ValidationAttribute
	{
		public int MaximumLength { get; private set; }
		public int MinimumLength { get; private set; }

		/// <summary>
		/// 字符串长度验证
		/// </summary>
		/// <param name="minimumLength"></param>
		/// <param name="maximumLength"></param>
		public VaStringLengthAttribute(int minimumLength, int maximumLength) : base()
		{
			MinimumLength = minimumLength;
			MaximumLength = maximumLength;
		}

		public VaStringLengthAttribute(int maximumLength) : base()
		{
			MinimumLength = -1;
			MaximumLength = maximumLength;
		}

		public override bool IsValid(object value)
		{
			if (null == value) { return true; }
			var val = value.ToString();

			if (val.Length >= MinimumLength && val.Length <= MaximumLength) {
				return true;
			}
			return false;
		}

		public override string FormatErrorMessage(string name)
		{
			if (MinimumLength == -1) {
				return $"{name}长度不大于{MaximumLength}位。";
			}
			return $"{name}长度不小于{MinimumLength}位,不大于{MaximumLength}位。";
		}
	}
}