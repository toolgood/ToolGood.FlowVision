using System.Text.Json.Serialization;

namespace ToolGood.FlowVision.Commons
{
	public class Request<T> : IRequest
		where T : class
	{
		/// <summary>
		/// 解密后的参数
		/// </summary>
		[JsonPropertyName("data")]
		public T Data { get; set; }

		[JsonIgnore]
		public string Ip { get; set; }

		[JsonIgnore]
		public string UserAgent { get; set; }

		public string Fingerprint { get; set; }

		[JsonIgnore]
		public int MainMemberId { get; set; }

		[JsonIgnore]
		public int OperatorId { get; set; }

		[JsonIgnore]
		public string OperatorName { get; set; }

		[JsonIgnore]
		public string OperatorTrueName { get; set; }

		[JsonIgnore]
		public string Message { get; set; }
	}
}