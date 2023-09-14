namespace ToolGood.FlowVision.Commons.Utils.Internals
{
	public interface ILogger
	{
		bool UseDebug { get; set; }
		bool UseInfo { get; set; }
		bool UseWarn { get; set; }
		bool UseError { get; set; }
		bool UseFatal { get; set; }
		bool UseSql { get; set; }

		void WriteLog(string type, string msg);

		void WriteLog(HttpContext HttpContext, LogType type, string msg);
	}
}