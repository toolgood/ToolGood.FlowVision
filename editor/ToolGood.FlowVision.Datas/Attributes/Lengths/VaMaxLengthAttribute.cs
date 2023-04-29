/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	/// <summary>
	/// 最大长度验证
	/// </summary>
	public class VaMaxLengthAttribute : MaxLengthAttribute
	{
		/// <summary>
		/// 最大长度验证
		/// </summary>
		/// <param name="length"></param>
		public VaMaxLengthAttribute(int length) : base(length)
		{
		}

		public override string FormatErrorMessage(string name)
		{
			return $"{name}长度不大于{Length}位。";
		}
	}
}