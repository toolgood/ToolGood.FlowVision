using Microsoft.AspNetCore.Mvc;
using ToolGood.FlowVision.Applications.Members;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Controllers;

namespace ToolGood.FlowVision.Pages.Members
{
	public class LoginModel : PageModelBaseCore
	{
		public string RsaExponent { get; private set; }
		public string RsaModulus { get; private set; }
		public string UserName { get; private set; }
		public string Password { get; private set; }

		private readonly IMemberApplication _memberApplication;

		public LoginModel(IMemberApplication memberApplication)
		{
			this._memberApplication = memberApplication;
		}

		public async Task<IActionResult> OnGet()
		{
			GetPrivateKey();

			#region ·ÀÖ¹ÍøÒ³ºóÍË--½ûÖ¹»º´æ

			Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate"); // HTTP 1.1.
			Response.Headers.Add("Pragma", "no-cache"); // HTTP 1.0.
			Response.Headers.Add("Expires", "-1"); // Proxies.

			#endregion ·ÀÖ¹ÍøÒ³ºóÍË--½ûÖ¹»º´æ

			DeleteCookie(CookieSetting.MemberCookie);
			DeleteCookie(CookieSetting.MemberCookieLogin);
			DeleteSession(SessionSetting.MemberSession);

			var rsa = RsaHelper.Instance;
			RsaExponent = rsa.RsaExponent;
			RsaModulus = rsa.RsaModulus;

			if (await _memberApplication.CheckAdminDefaultPassword()) {
				UserName = "admin";
				Password = "admin";
			}
			return Page();
		}

		private void GetPrivateKey()
		{
			var rsa = RsaHelper.Instance;
			RsaExponent = rsa.RsaExponent;
			RsaModulus = rsa.RsaModulus;
		}
	}
}