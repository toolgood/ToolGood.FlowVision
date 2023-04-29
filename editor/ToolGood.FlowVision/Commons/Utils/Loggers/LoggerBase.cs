using Microsoft.AspNetCore.Extensions;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ToolGood.FlowVision.Commons.Utils.Internals
{
	public abstract class LoggerBase : ILogger
	{
		public virtual bool UseDebug { get; set; } = false;
		public virtual bool UseInfo { get; set; } = true;
		public virtual bool UseWarn { get; set; } = false;
		public virtual bool UseError { get; set; } = true;
		public virtual bool UseFatal { get; set; } = true;
		public virtual bool UseSql { get; set; } = true;
		public virtual string Log_Prefix { get; set; } = "{yyyy}-{MM}-{dd} {HH}:{mm}:{ss}|{type}|{class}.{method} >>> \r\n";

		public virtual void WriteLog(LogType type, string msg)
		{
			if (type == LogType.Debug && UseDebug == false) return;
			if (type == LogType.Error && UseError == false) return;
			if (type == LogType.Fatal && UseFatal == false) return;
			if (type == LogType.Info && UseInfo == false) return;
			if (type == LogType.Sql && UseSql == false) return;
			if (type == LogType.Warn && UseWarn == false) return;

			var context = FormatLog(type.ToString(), msg, null);
			WriteLog(type.ToString(), context);
		}

		public abstract void WriteLog(string type, string content);

		public virtual void WriteLog(HttpContext HttpContext, LogType type, string msg)
		{
			var context = FormatLog(type.ToString(), msg, HttpContext);
			WriteLog(type.ToString(), context);
		}

		protected virtual string FormatLog(string type, string content, HttpContext HttpContext)
		{
			var Time = DateTime.Now;
			var log = new StringBuilder(Log_Prefix);
			log.Replace("{time}", (Time.Ticks / 1000).ToString());
			log.Replace("{yyyy}", Time.Year.ToString());
			log.Replace("{yy}", (Time.Year % 100).ToString("D2"));
			log.Replace("{MM}", Time.Month.ToString("D2"));
			log.Replace("{dd}", Time.Day.ToString("D2"));
			log.Replace("{HH}", Time.Hour.ToString("D2"));
			log.Replace("{hh}", Time.Hour.ToString("D2"));
			log.Replace("{mm}", Time.Minute.ToString("D2"));
			log.Replace("{ss}", Time.Second.ToString("D2"));
			log.Replace("{type}", type);

			if (Log_Prefix.Contains("{class}") || Log_Prefix.Contains("{method}")) {
				var mi = GetMethodInfo();
				log.Replace("{class}", mi.DeclaringType.FullName);
				log.Replace("{method}", mi.Name);
			}
			log.Replace("{threadId}", Thread.CurrentThread.ManagedThreadId.ToString());

			if (HttpContext != null) {
				var ip = HttpContext.GetRealIP();
				var request = HttpContext.Request;
				log.Append($"{ip}|{request.Method}|{request.Scheme}://{request.Host}{request.Path}{request.QueryString}\r\n");
				if (request.Method == "POST") {
					if (request.ContentType.ToLower().StartsWith("multipart/form-data;") == false || request.ContentType.ToLower().Contains("json") == false) {
						try {
							using var buffer = new MemoryStream();
							request.Body.Position = 0;
							request.Body.CopyTo(buffer);
							request.Body.Position = 0;
							var bs = buffer.ToArray();
							var post = Encoding.UTF8.GetString(bs);
							log.Append("Body:" + post + "\r\n");
						} catch { }
					}
				}
			}
			if (string.IsNullOrEmpty(content) == false) {
				log.Append(content);
			}
			log.Append("\r\n\r\n");
			return log.ToString();
		}

		protected MethodBase GetMethodInfo()
		{
			var st = new StackTrace();
			int index = 1;
			var mi = st.GetFrame(index++).GetMethod();
			var classType = mi.DeclaringType;
			while (MatchType(classType, mi)) {
				mi = st.GetFrame(index++).GetMethod();
				classType = mi.DeclaringType;
			}
			return mi;
		}

		private bool MatchType(Type type, MethodBase mi)
		{
			if (type == typeof(TextLogger) /*|| type == typeof(AssertUtil) */|| type == typeof(LogUtil)) {
				return true;
			}
			var ts = type.GetInterfaces();
			foreach (var item in ts) {
				if (item == typeof(ILogger)) {
					return true;
				}
			}
			//return mi.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(IgnoreLogAttribute)));
			return false;
		}
	}
}