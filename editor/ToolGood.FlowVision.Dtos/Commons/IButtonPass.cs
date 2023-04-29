/*!
 *  版权所有(C) 2021 ToolGood(林知君)
 *  GPLv3 License - http://www.gnu.org/licenses/gpl-3.0.html
 */

namespace ToolGood.FlowVision.Dtos.Commons
{
	public interface IButtonPass
	{
		bool HasButtonCode(string buttonCode);

		bool CanEdit { get; }
		bool CanDelete { get; }
		bool CanEditOrDelete { get; }
	}
}