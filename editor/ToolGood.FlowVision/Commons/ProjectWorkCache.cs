using System.Text;
using ToolGood.FlowVision.Applications.Projects;
using ToolGood.FlowVision.Commons.My;
using ToolGood.FlowWork.Flows;

namespace ToolGood.FlowWork.Commons
{
	internal static class ProjectWorkCache
	{
		public static string PorjectName { get; set; }
		public static string PorjectCode { get; set; }

		private static ProjectWork _projectWork;

		public static ProjectWork GetProjectWork(IMemberFlowApplication _memberFlowApplication, string projectCode)
		{
			if (string.IsNullOrEmpty(projectCode) || PorjectCode == projectCode) {
				if (_projectWork == null) {
					lock (typeof(ProjectWorkCache)) {
						if (_projectWork == null) {
							var projectWorkFile = _memberFlowApplication.GetProjectWorkFile(1);
							projectWorkFile.Wait();
							if (projectWorkFile.Result != null) {
								PorjectCode = projectWorkFile.Result.ProjectCode;
								_projectWork = ProjectWork.LoadJson(projectWorkFile.Result.Json);
							}
						}
					}
				}
				return _projectWork;
			} else {
				var projectWorkFile = _memberFlowApplication.GetProjectWorkFile(1, projectCode);
				projectWorkFile.Wait();
				if (projectWorkFile.Result != null) {
					return ProjectWork.LoadJson(projectWorkFile.Result.Json);
				}
			}
			return null;
		}

		public static void Update(IMemberFlowApplication _memberFlowApplication)
		{
			lock (typeof(ProjectWorkCache)) {
				var projectWorkFile = _memberFlowApplication.GetProjectWorkFile(1);
				projectWorkFile.Wait();
				if (projectWorkFile.Result != null) {
					_projectWork = ProjectWork.LoadJson(projectWorkFile.Result.Json);
				}
			}
		}

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
			try {
				var files = Directory.GetFiles(Path.Combine(MyHostingEnvironment.ContentRootPath, "App_Data"), PorjectName + "_*.json");
				foreach (var item in files.OrderByDescending(q => q)) {
					if (LoadJson(item)) { return; }
				}
			} catch (Exception) { }
			try {
				var files = Directory.GetFiles(MyHostingEnvironment.ContentRootPath, PorjectName + "_*.json");
				foreach (var item in files.OrderByDescending(q => q)) {
					if (LoadJson(item)) { return; }
				}
			} catch (Exception) { }
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