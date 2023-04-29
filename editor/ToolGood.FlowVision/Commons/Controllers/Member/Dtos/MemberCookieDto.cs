using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Commons.Controllers
{
	public class MemberCookieDto
	{
		/// <summary>
		/// 用户ID
		/// </summary>
		[JsonPropertyName("id")]
		public int UserId { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[JsonPropertyName("pwd")]
		public string PasswordHash { get; set; }

		/// <summary>
		/// 生成时间
		/// </summary>
		[JsonPropertyName("ct")]
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 过期时间
		/// </summary>
		[JsonPropertyName("et")]
		public DateTime ExpireTime { get; set; }
	}
}