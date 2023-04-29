/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.Attributes
{
	public class VaNotEmptyListAttribute : VaCountListAttribute
	{
		public VaNotEmptyListAttribute() : base(1, int.MaxValue, false)
		{
			ErrorMessage = "{0}为非空集合！";
		}
	}
}