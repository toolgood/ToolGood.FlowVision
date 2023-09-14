using Microsoft.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons.My;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.RcxCrypto;

namespace ToolGood.FlowVision.Commons.Controllers
{
	[AutoValidateAntiforgeryToken]
	public class MemberApiController : ApiControllerCore
	{
		public MemberSessionDto MemberDto { get; private set; }

		public override async void OnActionExecuting(ActionExecutingContext context)
		{
			var MemberApplication = MyIoc.Create<IMemberApplication>();

			#region 检测登录，cookie登录

			MemberDto = context.GetSession<MemberSessionDto>(SessionSetting.MemberSession);
			if (MemberDto == null) {
				var userDto = GetMemberCookieDto(context);
				if (userDto != null && userDto.ExpireTime > DateTime.Now) {
					if (MemberCacheHelper.CheckMemberSessionId(userDto.UserId, context.GetCookie(CookieSetting.MemberCookie))) {
						var Member = await MemberApplication.GetMemberById(userDto.UserId);
						if (null != Member) {
							bool pwd = HashUtil.GetMd5String(Member.Password) == userDto.PasswordHash;
							if (pwd) {
								MemberDto = new MemberSessionDto(Member.Id, Member.MainMemberId, Member.Name, Member.TrueName, Member.ProjectIds);
								context.SetSession(SessionSetting.MemberSession, MemberDto);
							}
						}
					}
				}
			}
			if (null == MemberDto) {
				if (context.HttpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase)) {
					var url = UrlSetting.MemberLoginUrl;
					context.Result = ActionResultUtil.JumpTopUrl(url, "cookie无效，请先登录！");
				} else {
					context.Result = ActionResultUtil.Error();
				}
				return;
			}

			#endregion 检测登录，cookie登录

			#region 检测菜单权限

			if (context.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDescriptor) {
				var mi = actionDescriptor.MethodInfo;
				var menus = mi.GetCustomAttributes<MemberMenuAttribute>(true);
				List<MemberMenuAttribute> MemberMenus = new List<MemberMenuAttribute>();
				if (menus.Count() > 0) {
					foreach (var item in menus) {
						var isPass = await MemberCacheHelper.MemberMenuButtonCache.GetOrAdd(MemberDto.Id + '-' + item.MenuCode + '-' + item.ButtonCode, async () => {
							return await MemberApplication.IsPass(MemberDto.Id, item.MenuCode, item.ButtonCode);
						});
						if (isPass) {
							MemberMenus.Add(item);
						}
					}
					if (MemberMenus.Count == 0) {
						context.Result = new RedirectResult(UrlSetting.MemberNoAccessUrl);
						return;
					}
					ViewData["MenuCode"] = MemberMenus[0].MenuCode;
					ViewData["ButtonCode"] = MemberMenus[0].ButtonCode;
				}
			}

			#endregion 检测菜单权限

			foreach (var item in context.ActionArguments) {
				if (item.Value is IRequest temp) {
					temp.Ip = GetRealIP();
					temp.UserAgent = GetUserAgent();
					temp.MainMemberId = MemberDto.MainMemberId;
					temp.OperatorId = MemberDto.Id;
					temp.OperatorName = MemberDto.Name;
					temp.OperatorTrueName = MemberDto.TrueName;
				}
			}
			base.OnActionExecuting(context);
		}

		private MemberCookieDto GetMemberCookieDto(ActionExecutingContext context)
		{
			var cookie = GetCookie(CookieSetting.MemberCookie);
			if (string.IsNullOrEmpty(cookie)) return null;
			var sp = cookie.Split(".");
			if (sp.Length != 2) return null;
			var bytes = Base64Util.FromBase64ForUrlString(sp[0]);
			var hash = HashUtil.GetMd5String(bytes);
			if (hash == sp[1]) {
				bytes = ThreeRCX.Encrypt(bytes, RsaHelper.Instance.CookiePassword);
				var json = Encoding.UTF8.GetString(bytes);
				try {
					var userDto = json.ToObject<MemberCookieDto>();
					if (userDto.ExpireTime < DateTime.Now || userDto.CreateTime > DateTime.Now) {
						return null;
					}
					return userDto;
				} catch { }
			}
			return null;
		}

		protected string DecryptKey(string rsaKey)
		{
			var rsaHelper = RsaHelper.Instance;
			return RsaUtil.PrivateDecrypt(rsaHelper.PrivateKey, rsaKey);
		}
	}
}