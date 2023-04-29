namespace ToolGood.FlowVision.Commons
{
	public interface IRequest
	{
		string Ip { get; set; }
		string UserAgent { get; set; }
		string Fingerprint { get; set; }

		int MainMemberId { get; set; }
		int OperatorId { get; set; }
		string OperatorName { get; set; }
		string OperatorTrueName { get; set; }
		string Message { get; set; }
	}
}