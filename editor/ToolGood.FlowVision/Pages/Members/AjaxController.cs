using Microsoft.AspNetCore.Mvc;
using System.Text;
using ToolGood.Datas;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;
using ToolGood.FlowVision.Commons.Utils;
using ToolGood.FlowVision.Dtos.Members.Changes;
using ToolGood.RcxCrypto;

namespace ToolGood.FlowVision.Pages.Members
{
	public class AjaxController : ApiControllerCore
	{
		private readonly IMemberApplication _MemberApplication;

		public AjaxController(IMemberApplication adminApplication)
		{
			_MemberApplication = adminApplication;
		}

		#region 验证码

		[HttpGet("/Members/Account/VerifyCode")]
		public IActionResult VerifyCode()
		{
			#region 防止网页后退--禁止缓存

			Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
			Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0.
			Response.Headers.Add("Expires", "-1"); // Proxies.

			#endregion 防止网页后退--禁止缓存

			Random r = new Random();
			string code = r.Next(10000, 100000).ToString();
			SetSession(SessionSetting.MemberLoginCode, code);
			VerifyCodeUtil vimg = new VerifyCodeUtil();
			//vimg.FontSize = 30;
			var bytes = vimg.CreateImageBytes(code);
			var AcceptEncodings = Request.Headers["Accept-Encoding"].ToString().Replace(" ", "").Split(',');
			if (AcceptEncodings.Contains("br")) {
				Response.Headers["Content-Encoding"] = "br";
				bytes = CompressionUtil.BrCompress(bytes, true);
			} else if (AcceptEncodings.Contains("gzip")) {
				Response.Headers["Content-Encoding"] = "gzip";
				bytes = CompressionUtil.GzipCompress(bytes, true);
			}
			return File(bytes, "image/jpg");
		}

		#endregion 验证码

		[IgnoreAntiforgeryToken]
		[HttpPost("/members/ajax/Account/Login")]
		public async Task<IActionResult> Login([FromBody] LoginReq<MemberLoginDto> request)
		{
			var rsa = RsaHelper.Instance;
			if (request.CheckSign(rsa.PrivateKey, rsa.RsaModulus, rsa.RsaExponent, out string msg) == false) { return Error(msg); }
			if (request.DecryptData() == false) { return Error("数据错误!"); }

			if (CheckSession(SessionSetting.MemberLoginCode, request.Data.Vcode) == false) { return Error("验证码错误!"); }
			DeleteSession(SessionSetting.MemberLoginCode);

			var member = await _MemberApplication.MemberLogin(request);
			if (member == null) { return Error(request.Message); }

			SetSession(SessionSetting.MemberSession, new MemberSessionDto(member.Id, member.MainMemberId, member.Name, member.TrueName, member.ProjectIds));
			var setting = await _MemberApplication.GetSettingValueByCode("CookieTimes");//cookie 保存时间
			int mins = 0;
			if (setting != null) { if (int.TryParse(setting.Value, out mins) == false) { mins = 180 * 60; } }
			if (mins < 600) { mins = 180 * 60; }
			string cookie = SetAdminCookieDto(CookieSetting.MemberCookie, CreateAdminCookieDto(member, mins));

			MemberCacheHelper.SetMemberSessionId(member.Id, cookie);

			return Success();
		}

		private MemberCookieDto CreateAdminCookieDto(DbSysMember admin, int mins)
		{
			var dt = DateTime.Now;
			var exp = dt.AddMinutes(mins);
			MemberCookieDto userDto = new MemberCookieDto() {
				CreateTime = dt,
				ExpireTime = exp,
				UserId = admin.Id,
				//UserName = admin.Name,
				PasswordHash = HashUtil.GetMd5String(admin.Password),
			};
			return userDto;
		}

		[HttpGet("/Ajax/GetCurrTime")]
		public IActionResult GetCurrTime(double st)
		{
			var st2 = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(st);
			return Success((int)st2.TotalMilliseconds);
		}

		[HttpGet("/members/Logout")]
		public IActionResult Logout()
		{
			var userDto = GetAdminCookieDto();
			if (userDto != null) {
				MemberCacheHelper.SetMemberSessionId(userDto.UserId, "");
			}

			DeleteCookie(CookieSetting.MemberCookie);
			DeleteCookie(CookieSetting.MemberCookieLogin);
			DeleteSession(SessionSetting.MemberSession);
			return Redirect(UrlSetting.MemberLoginUrl);
		}

		private string SetAdminCookieDto(string key, MemberCookieDto dto)
		{
			var json = JsonUtil.SerializeObject(dto);
			var bytes = ThreeRCX.Encrypt(json, RsaHelper.Instance.CookiePassword);
			var hash = HashUtil.GetMd5String(bytes);
			var cookieStr = Base64Util.ToBase64ForUrlString(bytes) + "." + hash;
			SetCookie(key, cookieStr, dto.ExpireTime);

			HttpContext.Response.Cookies.Append(CookieSetting.MemberCookieLogin, "1", new Microsoft.AspNetCore.Http.CookieOptions() {
				Path = "/",
				Expires = dto.ExpireTime,
				IsEssential = true,
				HttpOnly = false,
			});
			return cookieStr;
		}

		private MemberCookieDto GetAdminCookieDto()
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
	}
}