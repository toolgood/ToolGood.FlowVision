/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	public class VaMaxCountListAttribute : VaCountListAttribute
	{
		public VaMaxCountListAttribute(int max) : base(0, max)
		{
			ErrorMessage = "{0}集合个数不能大于{1}！";
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessage, name, MaxCount);
		}
	}
}