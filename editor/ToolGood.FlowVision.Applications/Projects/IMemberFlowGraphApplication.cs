using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowVision.Applications.Projects
{
	public interface IMemberFlowGraphApplication
	{
		/// <summary>
		/// 图片 更新，更新厂区名称,机械名称 工艺名称
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> GraphUpdate(Request<MemberIdDto> request);

		/// <summary>
		/// 更新公式
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> GraphUpdateFormulas(Request<MemberIdDto> request);

		Task<ProjectWork> Exports(int mainMemberId, int projectId);

		Task<List<String>> EnumerationConditions(int mainMemberId, int projectId);
    }
}