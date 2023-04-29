/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

using System.ComponentModel.DataAnnotations;

namespace ToolGood.Attributes
{
	public class VaRequiredAttribute : RequiredAttribute
	{
		public override string FormatErrorMessage(string name)
		{
			return $"{name}为必填字段。";
		}
	}
}