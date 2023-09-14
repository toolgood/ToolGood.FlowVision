namespace ToolGood.FlowVision.Commons.My
{
	/// <summary>
	/// 对应asp.net的 HostingEnvironment
	/// </summary>
	public static class MyHostingEnvironment
	{
#if DEBUG
		public static bool IsDevelopment { get { return true; } }
#else
        public static bool IsDevelopment { get { return false; } }
#endif

		public static string EnvironmentName { get; set; }

		public static string ApplicationName { get; set; }

		public static string WebRootPath { get; set; }

		public static string ContentRootPath { get; set; }

		/// <summary>
		/// 获取文件物理路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string MapPath(string path)
		{
			return IsAbsolute(path) ? path : Path.Combine(ContentRootPath, path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));
		}

		/// <summary>
		/// 以WebRoot为根目录获取文件物理路径
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string MapWebRootPath(string path)
		{
			return IsAbsolute(path) ? path : Path.Combine(WebRootPath, path.TrimStart('~', '/').Replace("/", Path.DirectorySeparatorChar.ToString()));
		}

		private static bool IsAbsolute(string path)
		{
			return Path.VolumeSeparatorChar == ':' ? path.IndexOf(Path.VolumeSeparatorChar) > 0 : path.IndexOf('\\') > 0;
		}
	}
}