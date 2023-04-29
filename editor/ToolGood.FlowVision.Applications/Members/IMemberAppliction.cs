using ToolGood.Datas;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Datas;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Members.Changes;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Applications.Members
{
	public interface IMemberApplication
	{
		#region 日志

		ValueTask<bool> WriteLoginLog(string msg, bool success, IRequest request);

		ValueTask<bool> WriteOperationLog(string msg, IRequest request);

		Task<Page<MemberLoginLogDto>> GetMemberLoginLogPage(Request<GetMemberLoginListDto> request);

		Task<Page<MemberOperationLogDto>> GetMemberOperationLogPage(Request<GetMemberOperationListDto> request);

		#endregion 日志

		#region 子账号管理

		/// <summary>
		/// 添加管理员
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> AddSubAccount(Request<DbSysMember> request);

		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> EditSubAccount(Request<DbSysMember> request);

		/// <summary>
		/// 删除管理员
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> DeleteSubAccount(Request<MemberIdDto> request);

		ValueTask<bool> FrozenItem(Request<MemberIdDto> request);

		ValueTask<bool> RecoveryItem(Request<MemberIdDto> request);

		/// <summary>
		/// 获取 页面信息
		/// </summary>
		/// <param name="name"></param>
		/// <param name="groupId"></param>
		/// <param name="isFrozen"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<Page<MemberDto>> GetSubAccountPage(Request<GetMemberListDto> request);

		/// <summary>
		/// 获取管理员
		/// </summary>
		/// <returns></returns>
		Task<List<DbSysMember>> GetSubAccountAll(int mainMemberId);

		Task<DbSysMember> GetSubAccountById(int mainMemberId, int id);

		Task<Page<MemberLoginLogDto>> GetSubAccountLoginLogPage(Request<GetMemberLoginListDto> request);

		ValueTask<bool> ChangeSubAccountPassword(Request<MemberChangePwdDto> request);

		#endregion 子账号管理

		#region 管理员操作

		ValueTask<bool> CheckAdminDefaultPassword();

		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		ValueTask<bool> EditMember(Request<DbSysMember> request);

		/// <summary>
		/// 管理员登录
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<DbSysMember> MemberLogin(LoginReq<MemberLoginDto> request);

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="request"></param>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		ValueTask<bool> ChangePassword(Request<MemberChangePwdDto> request);

		/// <summary>
		/// 获取 管理员信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<DbSysMember> GetMemberById(int id);

		/// <summary>
		/// 是否有权限
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="menuCode"></param>
		/// <param name="btnCode"></param>
		/// <returns></returns>
		ValueTask<bool> IsPass(int MemberId, string menuCode, string btnCode);

		#endregion 管理员操作

		#region 菜单操作

		Task<List<DbSysMemberMenu>> GetTopMenu(int MemberId);

		/// <summary>
		/// 获取侧边菜单
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="menuIds"></param>
		/// <returns></returns>
		Task<TreeMemberMenuDto> GetSidebarMenu(int MemberId, int menuId, string menuIds);

		Task<Dtos.Commons.IButtonPass> GetButtonPassByMemberId(int id, string menuCode);

		#endregion 菜单操作

		/// <summary>
		/// 获取所有管理组
		/// </summary>
		/// <returns></returns>
		Task<List<DbSysMemberGroup>> GetMemberGroupAll();

		Task<DbSysSettingValue> GetSettingValueByCode(string code);

		ValueTask<bool> SetTime(DateTime dateTime);

		ValueTask<DateTime> GetTime();

		ValueTask<DateTime> GetRealTime();
	}
}