/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	public class VaMinCountListAttribute : VaCountListAttribute
	{
		public VaMinCountListAttribute(int min) : base(min, int.MaxValue)
		{
			ErrorMessage = "{0}集合个数不能小于{1}！";
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessage, name, MinCount);
		}
	}
}