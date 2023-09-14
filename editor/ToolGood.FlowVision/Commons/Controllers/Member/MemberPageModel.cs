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
	public class MemberPageModel : PageModelBaseCore
	{
		public MemberSessionDto MemberDto { get; private set; }

		public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			var MemberApplication = MyIoc.Create<IMemberApplication>();

			#region 检测登录，cookie登录

			MemberDto = GetSession<MemberSessionDto>(SessionSetting.MemberSession);
			if (MemberDto == null) {
				var userDto = GetMemberCookieDto(context);
				if (userDto != null && userDto.ExpireTime > DateTime.Now) {
					if (MemberCacheHelper.CheckMemberSessionId(userDto.UserId, GetCookie(CookieSetting.MemberCookie))) {
						var Member = await MemberApplication.GetMemberById(userDto.UserId);
						if (null != Member && Member.IsFrozen == false) {
							bool pwd = HashUtil.GetMd5String(Member.Password) == userDto.PasswordHash;
							if (pwd) {
								MemberDto = new MemberSessionDto(Member.Id, Member.MainMemberId, Member.Name, Member.TrueName, Member.ProjectIds);
								SetSession(SessionSetting.MemberSession, MemberDto);
							}
						}
					}
				}
			}
			if (null == MemberDto) {
				if (context.HttpContext.Request.Method.ToUpper() == "GET") {
					var url = UrlSetting.MemberLoginUrl;
					context.Result = ActionResultUtil.JumpTopUrl(url, "cookie无效，请先登录！");
				} else {
					context.Result = ActionResultUtil.Error();
				}
				return;
			}

			#endregion 检测登录，cookie登录

			#region 检测菜单权限

			var menus = this.GetType().GetCustomAttributes<MemberMenuAttribute>(true);
			List<MemberMenuAttribute> MemberMenus = new List<MemberMenuAttribute>();
			if (menus.Count() > 0) {
				foreach (var item in menus) {
					var isPass = await MemberCacheHelper.MemberMenuButtonCache.GetOrAdd(MemberDto.Id + "-" + item.MenuCode + "-" + item.ButtonCode, async () => {
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
				ViewData["Title"] = MemberMenus[0].MenuCode;
			}

			#endregion 检测菜单权限

			base.OnPageHandlerExecuting(context);
		}

		private MemberCookieDto GetMemberCookieDto(PageHandlerExecutingContext context)
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

		protected void SetMemberCookie(MemberCookieDto dto)
		{
			var json = dto.ToJson();
			var bytes = ThreeRCX.Encrypt(json, RsaHelper.Instance.CookiePassword);
			var hash = HashUtil.GetMd5String(bytes);
			var cookieStr = Base64Util.ToBase64ForUrlString(bytes) + "." + hash;
			SetCookie(CookieSetting.MemberCookie, cookieStr, dto.ExpireTime);
		}

		protected string GetMemberCookieString(MemberCookieDto dto)
		{
			var json = dto.ToJson();
			var bytes = ThreeRCX.Encrypt(json, RsaHelper.Instance.CookiePassword);
			var hash = HashUtil.GetMd5String(bytes);
			var cookieStr = Base64Util.ToBase64ForUrlString(bytes) + "." + hash;
			return cookieStr;
		}

		protected void SetMemberCookie(MemberCookieDto dto, DateTime expireTime)
		{
			var json = dto.ToJson();
			var bytes = ThreeRCX.Encrypt(json, RsaHelper.Instance.CookiePassword);
			var hash = HashUtil.GetMd5String(bytes);
			var cookieStr = Base64Util.ToBase64ForUrlString(bytes) + "." + hash;
			SetCookie(CookieSetting.MemberCookie, cookieStr, expireTime);
		}

		protected MemberCookieDto GetMemberCookie()
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
				} catch (Exception) { }
			}
			return null;
		}

		protected void ClearMemberCookie()
		{
			DeleteCookie(CookieSetting.MemberCookie);
		}
	}
}