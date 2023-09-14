using ToolGood.FlowVision.Commons;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Applications.Members.Impl
{
	public abstract class ApplicationBase
	{
		protected SqlHelper GetWriteHelper()
		{
			return Config.WriteHelper;
		}

		protected SqlHelper GetReadHelper()
		{
			return Config.ReadHelper;
		}
	}
}