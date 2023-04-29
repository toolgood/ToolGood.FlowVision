using ToolGood.FlowVision.Commons;

namespace ToolGood.FlowVision.Applications.Projects
{
	public interface IMemberFlowImportApplication
	{
		byte[] GetProjectTemplate();

		ValueTask<bool> ImportProject(int projectId, Request<Stream> body);
	}
}