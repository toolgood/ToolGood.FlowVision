using Microsoft.AspNetCore.Http;
using ToolGood.Datas;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Datas;
using ToolGood.FlowVision.Dtos.Commons;
using ToolGood.FlowVision.Dtos.Members;
using ToolGood.FlowVision.Dtos.Members.Changes;
using ToolGood.ReadyGo3;

namespace ToolGood.FlowVision.Applications.Members.Impl
{
	public class MemberApplication : ApplicationBase, IMemberApplication
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public MemberApplication(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public MemberApplication()
		{ }

		#region 日志

		public async ValueTask<bool> WriteLoginLog(string msg, bool success, IRequest request)
		{
			var sessionId = _httpContextAccessor.HttpContext.Session.Id;
			var ip = request.Ip;
			var userAgent = request.UserAgent;

			var helper = GetWriteHelper();
			await helper.Insert_Async(new DbSysMemberLoginLog() {
				CreateTime = DateTime.Now,
				Name = request.OperatorName,
				TrueName = request.OperatorTrueName,
				MemberId = request.OperatorId,
				MainMemberId = request.MainMemberId,

				Message = msg,
				SessionID = sessionId,
				Ip = ip,
				Success = success,
				UserAgent = userAgent,
				Fingerprint = request.Fingerprint,
			});
			return true;
		}

		public async ValueTask<bool> WriteOperationLog(string message, IRequest request)
		{
			var sessionId = _httpContextAccessor.HttpContext.Session.Id;
			var ip = request.Ip;
			var userAgent = request.UserAgent;

			var helper = GetWriteHelper();
			var member = await helper.FirstOrDefault_Async<DbSysMember>(request.OperatorId);

			DbSysMemberOperationLog operationLog = new DbSysMemberOperationLog {
				CreateTime = DateTime.Now,
				Name = member.Name,
				MemberId = request.OperatorId,
				TrueName = request.OperatorTrueName,
				MainMemberId = request.MainMemberId,

				Message = message,
				Ip = ip,
				UserAgent = userAgent,
				SessionID = sessionId,
				Fingerprint = request.Fingerprint,
			};
			await helper.Insert_Async(operationLog);
			return true;
		}

		public async Task<Page<MemberLoginLogDto>> GetMemberLoginLogPage(Request<GetMemberLoginListDto> request)
		{
			var helper = GetReadHelper();

			return await helper.Where<MemberLoginLogDto>()
					.Where(q => q.Name == request.OperatorName)
					.IfSet(request.Data.Message).Where(q => q.Message.Contains(request.Data.Message))
					.IfSet(request.Data.Ip).Where(q => q.Message == (request.Data.Ip))
					.IfNotNull(request.Data.StartTime).Where(q => q.CreateTime >= request.Data.StartTime)
					.IfNotNull(request.Data.EndTime).Where(q => q.CreateTime < request.Data.EndTime.Value.AddDays(1))
					.Page_Async(request.Data.PageIndex, request.Data.PageSize);
		}

		public async Task<Page<MemberOperationLogDto>> GetMemberOperationLogPage(Request<GetMemberOperationListDto> request)
		{
			var helper = GetReadHelper();
			var member = await helper.FirstOrDefault_Async<DbSysMember>(request.OperatorId);

			return await helper.Where<MemberOperationLogDto>()
					.Where(q => q.MemberId == request.MainMemberId)
					.IfSet(request.Data.Name).Where(q => q.Name == member.Name)
					.IfSet(request.Data.Message).Where(q => q.Message.Contains(request.Data.Message))
					.IfSet(request.Data.Ip).Where(q => q.Message == (request.Data.Ip))
					.IfNotNull(request.Data.StartTime).Where(q => q.CreateTime >= request.Data.StartTime)
					.IfNotNull(request.Data.EndTime).Where(q => q.CreateTime < request.Data.EndTime.Value.AddDays(1))
					.Page_Async(request.Data.PageIndex, request.Data.PageSize);
		}

		#endregion 日志

		#region 子账号管理

		/// <summary>
		/// 添加子账号
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async ValueTask<bool> AddSubAccount(Request<DbSysMember> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.Name == null) { throw new ArgumentException("Data.Name is null"); }
			if (request.Data.Password == null) { throw new ArgumentException("Data.Password is null"); }
			if (request.Data.Name.Length < 4) { throw new ArgumentException("Data.Name length less than 4 bits"); }

			var helper = GetWriteHelper();
			var count = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Name == request.Data.Name).SelectCount_Async();
			if (count > 0) { request.Message = "有相同用户名!"; return false; }

			var db = DbSysMember.CreateBy(request);
			db.Salt = RandomUtil.GetRandomString(12);
			db.Password = CreatePassword(request.Data.Name, db.Salt, request.Data.Password);
			await helper.Insert_Async(db);

			await WriteOperationLog($"添加子账号[{request.Data.Name}]", request);
			return true;
		}

		/// <summary>
		/// 修改子账号
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async ValueTask<bool> EditSubAccount(Request<DbSysMember> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.MainMemberId != request.OperatorId) { request.Message = "失败，当前用户无权修改！"; return false; }

			var helper = GetWriteHelper();
			var Member = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Id == request.Data.Id).FirstOrDefault_Async();
			if (Member == null) { request.Message = "失败，不存在该用户"; return false; }
			if (Member.MainMemberId != request.MainMemberId) { request.Message = "失败，不存在该用户"; return false; }
			Member.EditBy(request);
			await helper.Update_Async(Member);

			await WriteOperationLog($"修改子账号[{request.Data.Name}]", request);
			return true;
		}

		/// <summary>
		/// 删除子账号
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async ValueTask<bool> DeleteSubAccount(Request<MemberIdDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.Id == null) { throw new ArgumentException("Data.Id is null"); }
			if (request.MainMemberId != request.OperatorId) { request.Message = "失败，当前用户无权修改！"; return false; }
			if (request.OperatorId == request.Data.Id) { request.Message = "不能自己删除自己！"; return false; }

			var helper = GetWriteHelper();
			var Member = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Id == request.Data.Id).FirstOrDefault_Async();
			if (Member == null) { request.Message = "失败，不存在该用户"; return false; }
			if (Member.MainMemberId != request.MainMemberId) { request.Message = "失败，不存在该用户"; return false; }

			Member.IsDelete = true;
			Member.DeleteTime = DateTime.Now;
			await helper.Update_Async(Member);

			await WriteOperationLog($"删除子账号[{Member.Name}]", request);
			return true;
		}

		public async ValueTask<bool> FrozenItem(Request<MemberIdDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.Id == null) { throw new ArgumentException("Data.Id is null"); }
			if (request.MainMemberId != request.OperatorId) { request.Message = "失败，当前用户无权修改！"; return false; }
			if (request.OperatorId == request.Data.Id) { request.Message = "冻结自己！"; return false; }

			var helper = GetWriteHelper();
			var Member = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Id == request.Data.Id).FirstOrDefault_Async();
			if (Member == null) { request.Message = "失败，不存在该用户"; return false; }
			if (Member.MainMemberId != request.MainMemberId) { request.Message = "失败，不存在该用户"; return false; }

			Member.IsFrozen = true;
			Member.ModifyTime = DateTime.Now;
			await helper.Update_Async(Member);

			await WriteOperationLog($"冻结子账号[{Member.Name}]", request);
			return true;
		}

		public async ValueTask<bool> RecoveryItem(Request<MemberIdDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.Id == null) { throw new ArgumentException("Data.Id is null"); }
			if (request.MainMemberId != request.OperatorId) { request.Message = "失败，当前用户无权修改！"; return false; }
			if (request.OperatorId == request.Data.Id) { request.Message = "恢复自己！"; return false; }

			var helper = GetWriteHelper();
			var Member = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Id == request.Data.Id).FirstOrDefault_Async();
			if (Member == null) { request.Message = "失败，不存在该用户"; return false; }
			if (Member.MainMemberId != request.MainMemberId) { request.Message = "失败，不存在该用户"; return false; }

			Member.IsFrozen = false;
			Member.ModifyTime = DateTime.Now;
			await helper.Update_Async(Member);

			await WriteOperationLog($"恢复子账号[{Member.Name}]", request);
			return true;
		}

		/// <summary>
		/// 获取 子账号
		/// </summary>
		/// <param name="name"></param>
		/// <param name="groupId"></param>
		/// <param name="isFrozen"></param>
		/// <param name="page"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public Task<Page<MemberDto>> GetSubAccountPage(Request<GetMemberListDto> request)
		{
			if (request == null) { throw new ArgumentNullException(nameof(request)); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }

			var helper = GetReadHelper();
			return helper.Where<MemberDto>(q => q.IsDelete == false)
					.Where(q => q.MainMemberId == request.MainMemberId)
					.Where(q => q.Id != request.MainMemberId)
					.IfTrue(request.Data.GroupId != null).Where(q => q.GroupId == (request.Data.GroupId))
					.IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
					.IfSet(request.Data.TrueName).Where(q => q.TrueName.Contains(request.Data.TrueName))
					.IfSet(request.Data.Phone).Where(q => q.Phone.Contains(request.Data.Phone))
					.IfSet(request.Data.Email).Where(q => q.Email.Contains(request.Data.Email))
					.IfSet(request.Data.Department).Where(q => q.Department.Contains(request.Data.Department))
					.IfSet(request.Data.JobNo).Where(q => q.JobNo.Contains(request.Data.JobNo))
					.IfSet(request.Data.Field).OrderBy(request.Data.Field, request.Data.Order)
					.Page_Async(request.Data.PageIndex, request.Data.PageSize);
		}

		/// <summary>
		/// 获取管理员
		/// </summary>
		/// <returns></returns>
		public Task<List<DbSysMember>> GetSubAccountAll(int mainMemberId)
		{
			var helper = GetReadHelper();
			return helper.Select_Async<DbSysMember>("where MainMemberId=@0 and IsDelete=0", mainMemberId);
		}

		public Task<DbSysMember> GetSubAccountById(int mainMemberId, int id)
		{
			var helper = GetReadHelper();
			return helper.FirstOrDefault_Async<DbSysMember>("where MainMemberId=@0 and Id=@1 and IsDelete=0", mainMemberId, id);
		}

		public Task<Page<MemberLoginLogDto>> GetSubAccountLoginLogPage(Request<GetMemberLoginListDto> request)
		{
			var helper = GetReadHelper();
			return helper.Where<MemberLoginLogDto>()
					.Where(q => q.MainMemberId == request.MainMemberId)
					.Where(q => q.MemberId != request.MainMemberId)
					.IfSet(request.Data.Name).Where(q => q.Name.Contains(request.Data.Name))
					.IfSet(request.Data.Message).Where(q => q.Message.Contains(request.Data.Message))
					.IfSet(request.Data.Ip).Where(q => q.Message == (request.Data.Ip))

					.IfNotNull(request.Data.StartTime).Where(q => q.CreateTime >= request.Data.StartTime)
					.IfNotNull(request.Data.EndTime).Where(q => q.CreateTime < request.Data.EndTime.Value.AddDays(1))
					.Page_Async(request.Data.PageIndex, request.Data.PageSize);
		}

		public async ValueTask<bool> ChangeSubAccountPassword(Request<MemberChangePwdDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.MemberId == 0) { throw new ArgumentException("Data.MemberId is null"); }
			if (request.Data.OldPassword == null) { throw new ArgumentException("Data.Password is null"); }
			if (request.Data.NewPassword == null) { throw new ArgumentException("Data.NewPassword is null"); }
			if (request.Data.ConfirmPassword == null) { throw new ArgumentException("Data.RepeatPassword is null"); }

			if (request.Data.NewPassword != request.Data.ConfirmPassword) { request.Message = "两次密码不一致！"; return false; }

			var helper = GetWriteHelper();
			var Member0 = await helper.FirstOrDefault_Async<DbSysMember>("where id=@0 and isdelete=0", request.OperatorId);
			if (Member0 == null) { request.Message = "用户名或密码错误！"; return false; }
			if (Member0.Id != Member0.MainMemberId) { request.Message = "用户名或密码错误！"; return false; }
			var oldp = CreatePassword(Member0.Name, Member0.Salt, request.Data.OldPassword);
			if (oldp != Member0.Password) { request.Message = "用户名或密码错误！"; return false; }

			var Member = await helper.FirstOrDefault_Async<DbSysMember>("where id=@0 and isdelete=0", request.Data.MemberId);
			if (Member == null) { request.Message = "用户名或密码错误！"; return false; }

			var newp = CreatePassword(Member.Name, Member.Salt, request.Data.NewPassword);
			await helper.Update_Async<DbSysMember>("set Password=@0,ModifyTime=@1 where id=@2", newp, DateTime.Now, request.Data.MemberId);

			await WriteOperationLog("修改密码成功！", request);
			return true;
		}

		#endregion 子账号管理

		#region 管理员操作

		public async ValueTask<bool> CheckAdminDefaultPassword()
		{
			var helper = GetReadHelper();
			var Member = await helper.FirstOrDefault_Async<DbSysMember>("where Name='admin' and IsDelete=0");
			return Member.Password == CreatePassword(Member.Name, Member.Salt, "admin");
		}

		/// <summary>
		/// 管理员登录
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<DbSysMember> MemberLogin(LoginReq<MemberLoginDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.TName == null) { throw new ArgumentException("Data.UserName is null"); }
			if (request.Data.TPwd == null) { throw new ArgumentException("Data.Password is null"); }
			request.OperatorTrueName = request.Data.TName;

			var helper = GetWriteHelper();
			request.Fingerprint = request.Data.Fingerprint;
			var loginCount = await helper.Count_Async<DbSysMemberLoginLog>("where (Name=@0 or Ip=@1) and CreateTime>@2", request.Data.TName, request.Ip, DateTime.Now.AddMinutes(-30));
			if (loginCount > 10) {
				request.Message = "登录超过上限，请稍后登录!";
				await WriteLoginLog("登录超过上限，请稍后登录!", false, request);
				return null;
			}
			var Member = await helper.FirstOrDefault_Async<DbSysMember>("where (Name=@0 OR Phone=@0) and IsDelete=0", request.Data.TName);
			if (Member == null) {
				request.OperatorName = request.Data.TName;
				request.Message = "用户名不存在或密码错误!";
				await WriteLoginLog($"用户名[{request.Data.TName}]不存在!密码:{request.Data.TPwd}", false, request);
				return null;
			}
			if (Member.Password != CreatePassword(Member.Name, Member.Salt, request.Data.TPwd)) {
				request.OperatorName = request.Data.TName;
				request.Message = "用户名不存在或密码错误!";
				await WriteLoginLog($"用户名[{request.Data.TName}]密码错误!密码:{request.Data.TPwd}", false, request);
				return null;
			}
			request.OperatorId = Member.Id;
			request.OperatorName = request.Data.TName;
			request.OperatorTrueName = Member.TrueName;
			request.MainMemberId = Member.MainMemberId;
			await WriteLoginLog("登录成功", true, request);
			return Member;
		}

		/// <summary>
		/// 修改密码
		/// </summary>
		/// <param name="request"></param>
		/// <param name="errMsg"></param>
		/// <returns></returns>
		public async ValueTask<bool> ChangePassword(Request<MemberChangePwdDto> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }
			if (request.Data.MemberId == 0) { throw new ArgumentException("Data.MemberId is null"); }
			if (request.Data.OldPassword == null) { throw new ArgumentException("Data.Password is null"); }
			if (request.Data.NewPassword == null) { throw new ArgumentException("Data.NewPassword is null"); }
			if (request.Data.ConfirmPassword == null) { throw new ArgumentException("Data.RepeatPassword is null"); }

			if (request.Data.NewPassword != request.Data.ConfirmPassword) { request.Message = "两次密码不一致！"; return false; }

			var helper = GetWriteHelper();
			var Member = await helper.FirstOrDefault_Async<DbSysMember>("where id=@0 and isdelete=0", request.Data.MemberId);
			if (Member == null) { request.Message = "用户名或密码错误！"; return false; }


			var oldp = CreatePassword(Member.Name, Member.Salt, request.Data.OldPassword);
			if (oldp != Member.Password) { request.Message = "用户名或密码错误！"; return false; }

			var newp = CreatePassword(Member.Name, Member.Salt, request.Data.NewPassword);
			await helper.Update_Async<DbSysMember>("set Password=@0,ModifyTime=@1 where id=@2", newp, DateTime.Now, request.Data.MemberId);

			await WriteOperationLog("修改密码成功！", request);
			return true;
		}

		public async ValueTask<bool> EditMember(Request<DbSysMember> request)
		{
			if (request == null) { throw new ArgumentNullException("request"); }
			if (request.Data == null) { throw new ArgumentException("Data is null"); }

			var helper = GetWriteHelper();
			var Member = await helper.Where<DbSysMember>(q => q.IsDelete == false).Where(q => q.Id == request.OperatorId).FirstOrDefault_Async();
			if (Member == null) {
				await WriteOperationLog("失败，不存在该用户！", request);
				request.Message = "失败，不存在该用户"; return false;
			}
			Member.TrueName = request.Data.TrueName;
			Member.Phone = request.Data.TrueName;
			Member.Email = request.Data.Email;

			await helper.Update_Async(Member);

			await WriteOperationLog("修改会员信息！", request);
			return true;
		}

		private string CreatePassword(string username, string salt, string password)
		{
			return DataUtil.CreatePassword(username, salt, password);
		}

		public Task<DbSysMember> GetMemberById(int id)
		{
			var helper = GetReadHelper();
			var whereHelper = helper.Where<DbSysMember>(q => q.IsDelete == false);
			return whereHelper.Where(q => q.Id == id).FirstOrDefault_Async();
		}

		#endregion 管理员操作

		#region 菜单操作

		/// <summary>
		/// 是否有权限
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="menuCode"></param>
		/// <param name="btnCode"></param>
		/// <returns></returns>
		public async ValueTask<bool> IsPass(int MemberId, string menuCode, string btnCode)
		{
			var helper = Config.ReadHelper;
			var sql = @"SELECT COUNT(1) FROM Sys_Member t1
INNER JOIN Sys_MemberGroup_Menu t3 ON t3.GroupId=t1.GroupId
WHERE t1.IsDelete=0 AND t1.Id=@0 AND t3.MenuCode=@1 AND t3.ButtonCode=@2
";
			return (await helper.Count_Async(sql, MemberId, menuCode, btnCode)) > 0;
		}

		public Task<List<DbSysMemberMenu>> GetTopMenu(int MemberId)
		{
			var helper = Config.ReadHelper;
			var sql = @"SELECT DISTINCT t4.* FROM Sys_Member t1
INNER JOIN Sys_MemberGroup_Menu t3 ON t3.GroupId=t1.GroupId
INNER JOIN Sys_MemberMenu t4 ON t4.Id=t3.MenuId
WHERE t1.IsDelete=0 and t4.IsDelete=0 AND t3.ButtonCode='navbar' AND t1.Id=@0 AND t4.ParentId=0
ORDER BY t4.OrderNum ASC
";
			return helper.Select_Async<DbSysMemberMenu>(sql, MemberId);
		}

		/// <summary>
		/// 获取侧边菜单
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="menuId"></param>
		/// <returns></returns>
		public async Task<TreeMemberMenuDto> GetSidebarMenu(int MemberId, int menuId, string menuIds)
		{
			var helper = Config.ReadHelper;
			var sql = @"SELECT DISTINCT t4.* FROM Sys_Member t1
INNER JOIN Sys_MemberGroup_Menu t3 ON t3.GroupId=t1.GroupId
INNER JOIN Sys_MemberMenu t4 ON t4.Id=t3.MenuId
WHERE t1.IsDelete=0 and t4.IsDelete=0 AND t3.ButtonCode='navbar' AND t1.Id=@0 And t4.ParentIds like @1
ORDER BY t4.OrderNum ASC
";
			var menus = await helper.Select_Async<DbSysMemberMenu>(sql, MemberId, menuIds + "%");
			menus.RemoveAll(q => q.Buttons.Split('|').Contains("navbar") == false);
			TreeMemberMenuDto dto = new TreeMemberMenuDto();
			dto.Id = menuId;
			BuildTreeMemberMenu(dto, menus);
			return dto;
		}

		private bool BuildTreeMemberMenu(TreeMemberMenuDto treeMemberMenu, List<DbSysMemberMenu> menus)
		{
			var tmenus = menus.Where(q => q.ParentId == treeMemberMenu.Id).ToList();
			foreach (var menu in tmenus) {
				TreeMemberMenuDto dto = new TreeMemberMenuDto() {
					Id = menu.Id,
					Name = menu.MenuName,
					Url = menu.Url,
				};
				treeMemberMenu.Items.Add(dto);
				var isopen = BuildTreeMemberMenu(dto, menus);
				if (isopen) { dto.IsOpen = true; }
				if (dto.IsOpen) { treeMemberMenu.IsOpen = true; }
			}
			return treeMemberMenu.IsOpen;
		}

		/// <summary>
		/// 获取菜单
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<IButtonPass> GetButtonPassByMemberId(int id, string menuCode)
		{
			var helper = Config.ReadHelper;
			var sql = @"SELECT t1.Id MemberId,t3.Name MemberName,t1.GroupId,t2.Name GroupName,t4.MenuId,t4.MenuCode,t5.MenuName,t4.ButtonId,t4.ButtonCode,t6.ButtonName
FROM Sys_Member t1
INNER JOIN Sys_MemberGroup t2 ON t2.Id=t1.GroupId
INNER JOIN Sys_Member t3 ON t3.Id=t1.Id
INNER JOIN Sys_MemberGroup_Menu t4 ON t4.GroupId=t1.GroupId
INNER JOIN Sys_MemberMenu t5 ON t4.MenuId=t5.Id
INNER JOIN Sys_MemberMenuButton t6 ON t4.ButtonId=t6.Id
WHERE t2.IsDelete=0 AND t3.IsDelete=0 AND t5.IsDelete=0 AND t6.IsDelete=0 AND t1.Id=@0 AND t5.MenuCode=@1
";
			var list = await helper.Select_Async<MemberRelationshipDto>(sql, id, menuCode);
			MemberRelationshipDto relationship = new MemberRelationshipDto();
			relationship.Items = list;
			relationship.MemberId = id;
			return relationship;
		}

		#endregion 菜单操作

		#region 管理组

		/// <summary>
		/// 获取所有管理组
		/// </summary>
		/// <returns></returns>
		public Task<List<DbSysMemberGroup>> GetMemberGroupAll()
		{
			var helper = GetReadHelper();
			return helper.Where<DbSysMemberGroup>(q => q.IsDelete == false)
					.OrderBy(q => q.OrderNum)
					.OrderBy(q => q.Name)
					.Select_Async();
		}

		#endregion 管理组

		public Task<DbSysSettingValue> GetSettingValueByCode(string code)
		{
			var helper = GetReadHelper();
			return helper.FirstOrDefault_Async<DbSysSettingValue>("where Code=@0 and IsDelete=0", code);
		}

		public async ValueTask<bool> SetTime(DateTime dateTime)
		{
			var helper = GetWriteHelper();
			var count = await helper.Update_Async<DbSysSettingValue>("set Value=@0 where Code='time' and IsDelete=0", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
			return count > 0;
		}

		public async ValueTask<DateTime> GetTime()
		{
			var helper = GetReadHelper();
			var settingValue = await helper.FirstOrDefault_Async<DbSysSettingValue>("where Code='time' and IsDelete=0");
			if (settingValue != null) {
				if (DateTime.TryParse(settingValue.Value, out DateTime dt)) {
					return dt;
				}
			}
			return DateTime.Now;
		}

		public async ValueTask<DateTime> GetRealTime()
		{
			var dt = await GetTime();
			if (dt > DateTime.Now) {
				return dt;
			}
			return DateTime.Now;
		}
	}
}