using System.Text;
using ToolGood.FlowWork.Commons.My;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Commons
{
	internal static class ProjectWorkCache
	{
		public static string PorjectName { get; set; }
		public static string PorjectCode { get; set; }

		private static ProjectWork _projectWork;

		public static ProjectWork GetProjectWork()
		{
			if (_projectWork == null) {
				lock (typeof(ProjectWorkCache)) {
					if (_projectWork == null) {
						Update();
					}
				}
			}
			return _projectWork;
		}

		public static void Update()
		{
				var files = Directory.GetFiles(Path.Combine(MyHostingEnvironment.ContentRootPath, "App_Data"), PorjectName + "_*.json").ToList();
				files.AddRange(Directory.GetFiles(Path.Combine(MyHostingEnvironment.ContentRootPath, "App_Data"), PorjectName + "_*.data"));
				foreach (var item in files.OrderByDescending(q => q)) {
					if (LoadJson(item)) { return; }
				}
				files = Directory.GetFiles(MyHostingEnvironment.ContentRootPath, PorjectName + "_*.json").ToList();
				files.AddRange(Directory.GetFiles(MyHostingEnvironment.ContentRootPath, PorjectName + "_*.data"));
				foreach (var item in files.OrderByDescending(q => q)) {
					if (LoadJson(item)) { return; }
				}
		}

		private static bool LoadJson(string item)
		{
			try {
				var jsonByte = System.IO.File.ReadAllBytes(item);
				if (jsonByte[0] == '{') {
					var json = Encoding.UTF8.GetString(jsonByte);
					var projectWork = ProjectWork.LoadJson(json);
					if (projectWork.Code == PorjectCode) {
						_projectWork = projectWork;
						return true;
					}
				} else {
					var publicKey = "<RSAKeyValue><Modulus>u3W3xI6mH9tr3A+sNZVhyIbQWFhePbPWdFeTtM39yR7kO4Akp6Dsb1NYKpKxSGjIwDv1TC6/IUwOgOYYSVa0pgfIujHPrYFO/LlDk6kPAyHluLimKFkHkze5FsY7YAqd2mExqdJ4Zfb1pXgIrVFgOs4o69p9vyBV6kWS0FBOnyyUK92bRYxeqS1raRfM3GUlIEaQW5ZIuJxQtFrfwSnsfDVhkp8rvFVt7I5aqawWeoJZu+/HZqQO/gz+BJ7ntlUWoPgfe13/U3kIOHMTc/Deczb5x3DeBv9XrwJ5+DrzrJV8jTdhiYeNcBNBYaKoHGS15chxt6no4sIDZYsI2N4ciQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
					var projectWork = ProjectWork.LoadJsonWithRsa(publicKey, jsonByte);
					if (projectWork.Code == PorjectCode) {
						_projectWork = projectWork;
						return true;
					}
				}
			} catch (Exception) { }
			return false;
		}
	}
}