using System.Text;
using ToolGood.FlowVision.Commons.Utils.Internals;
using ILogger = ToolGood.FlowVision.Commons.Utils.Internals.ILogger;

namespace ToolGood.FlowVision.Commons
{
	/// <summary>
	/// 日志操作
	/// </summary>
	public class LogUtil
	{
		private static List<ILogger> allLoggers = new List<ILogger>();

		public static bool UseDebug { get; set; } = true;
		public static bool UseInfo { get; set; } = true;
		public static bool UseWarn { get; set; } = true;
		public static bool UseError { get; set; } = true;
		public static bool UseFatal { get; set; } = true;
		public static bool UseSql { get; set; } = true;

		static LogUtil()
		{
			allLoggers.Add(new TextLogger());
		}

		private static void WriteLog(LogType type, string msg, HttpContext HttpContext)
		{
			if (type == LogType.Debug && UseDebug == false) return;
			if (type == LogType.Error && UseError == false) return;
			if (type == LogType.Fatal && UseFatal == false) return;
			if (type == LogType.Info && UseInfo == false) return;
			if (type == LogType.Warn && UseWarn == false) return;

			foreach (var logger in allLoggers) {
				logger.WriteLog(HttpContext, type, msg);
			}
		}

		public static void Debug(HttpContext HttpContext, string msg)
		{
			WriteLog(LogType.Debug, msg, HttpContext);
		}

		public static void Info(HttpContext HttpContext, string msg)
		{
			WriteLog(LogType.Info, msg, HttpContext);
		}

		public static void Warn(HttpContext HttpContext, string msg)
		{
			WriteLog(LogType.Warn, msg, HttpContext);
		}

		public static void Fatal(HttpContext HttpContext, string msg)
		{
			WriteLog(LogType.Fatal, msg, HttpContext);
		}

		public static void Error(HttpContext HttpContext, string msg)
		{
			WriteLog(LogType.Error, msg, HttpContext);
		}

		public static void Error(HttpContext HttpContext, string msg, Exception ex)
		{
			var dotNetVersion = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
			var OSVersion = Environment.OSVersion.ToString();
			msg = msg + ToErrMsg(ex, null, OSVersion + " " + dotNetVersion);
			WriteLog(LogType.Error, msg, HttpContext);
		}

		public static void Error(HttpContext HttpContext, Exception ex, string memberName = null)
		{
			var dotNetVersion = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
			var OSVersion = Environment.OSVersion.ToString();
			var msg = ToErrMsg(ex, memberName, OSVersion + " " + dotNetVersion);
			WriteLog(LogType.Error, msg, HttpContext);
		}

		/// <summary>
		/// 获取错误异常信息
		/// </summary>
		/// <param name="ex">异常</param>
		/// <param name="memberName">出现异常的方法名字</param>
		/// <param name="customMsg">自定义错误消息</param>
		/// <returns>错误异常信息</returns>
		private static string ToErrMsg(Exception ex, string memberName = null, string customMsg = null)
		{
			StringBuilder errorBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(memberName)) {
				errorBuilder.AppendFormat("CallerMemberName：{0}", memberName).AppendLine();
			}
			if (!string.IsNullOrWhiteSpace(customMsg)) {
				errorBuilder.AppendFormat("CustomMsg: {0}", customMsg ?? "").AppendLine();
			}
			errorBuilder.AppendFormat("Message：{0}", ex.Message).AppendLine();
			if (ex.InnerException != null) {
				if (!string.Equals(ex.Message, ex.InnerException.Message, StringComparison.OrdinalIgnoreCase)) {
					errorBuilder.AppendFormat("InnerException：{0}", ex.InnerException.Message).AppendLine();
				}
			}
			errorBuilder.AppendFormat("Source：{0}", ex.Source).AppendLine();
			errorBuilder.AppendFormat("StackTrace：{0}", ex.StackTrace).AppendLine();
			//if (WebUtil.IsHaveHttpContext()) {
			//    errorBuilder.AppendFormat("RealIP：{0}", WebUtil.GetRealIP()).AppendLine();
			//    errorBuilder.AppendFormat("HttpRequestUrl：{0}", WebUtil.GetHttpRequestUrl()).AppendLine();
			//    errorBuilder.AppendFormat("UserAgent：{0}", WebUtil.GetUserAgent()).AppendLine();
			//}
			return errorBuilder.ToString();
		}
	}
}