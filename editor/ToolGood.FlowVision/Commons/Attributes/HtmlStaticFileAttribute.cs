using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolGood.FlowVision.Commons
{
	/// <summary>
	/// 生成Html静态文件
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class HtmlStaticFileAttribute : ActionFilterAttribute, IPageFilter
	{
		private static ConcurrentDictionary<string, PageCache> MemoryCache = new ConcurrentDictionary<string, PageCache>();

		private struct PageCache
		{
			public DateTime LastWriteTimeUtc;
			public byte[] Html;
			public byte[] GzipHtml;
			public byte[] BrHtml;

			public PageCache(DateTime dt, byte[] html, byte[] gzip, byte[] br)
			{
				LastWriteTimeUtc = dt;
				Html = html;
				GzipHtml = gzip;
				BrHtml = br;
			}
		}

		/// <summary>
		/// 页面更新参数，用于更新页面,更新文件 如 https://localhost:44345/?__update__
		/// </summary>
		public static string UpdateFileQueryString = "__update__";

		/// <summary>
		/// 页面测试参数，测试页面，不更新文件 如  https://localhost:44345/?__test__
		/// </summary>
		public static string TestQueryString = "__test__";

		/// <summary>
		/// 开发模式
		/// </summary>
		public static bool IsDevelopmentMode = false;

		/// <summary>
		/// 支持Url参数，不推荐使用
		/// </summary>
		public static bool UseQueryString = false;

		/// <summary>
		/// 页面缓存时间 单位：分钟
		/// </summary>
		public static int ExpireMinutes = 1;

		/// <summary>
		/// 静态文件保存路径, 如果为空，则默认放在 {dll文件夹}\html 文件夹下
		/// </summary>
		public static string OutputFolder;

		/// <summary>
		/// 使用GZIP压缩，会另生成一个单独的文件，以空间换时间，火狐、IE11 对于http://开头的地址不支持 br 压缩
		/// </summary>
		public static bool UseGzipCompress = false;

		/// <summary>
		/// 使用BR压缩，会另生成一个单独的文件，以空间换时间
		/// </summary>
		public static bool UseBrCompress = false;

		/// <summary>
		/// 指定方法，保存前会进行最小化处理, 推荐使用 WebMarkupMin
		/// </summary>
		public static event Func<string, string> MiniFunc;

		/// <summary>
		/// 当前页面缓存时间，（单位：分钟），最小值：1
		/// </summary>
		private int? CurrPageExpireMinutes { get; set; }

		/// <summary>
		/// 使用内存缓存
		/// </summary>
		public bool UseMemoryCache { get; set; }

		/// <summary>
		/// 生成Html静态文件
		/// </summary>
		public HtmlStaticFileAttribute()
		{ }

		/// <summary>
		/// 生成Html静态文件
		/// </summary>
		/// <param name="expireMinutes">当前页面缓存时间（单位：分钟），最小值：1</param>
		public HtmlStaticFileAttribute(int expireMinutes)
		{
			if (expireMinutes <= 0) {
				expireMinutes = 1;
			}
			CurrPageExpireMinutes = expireMinutes;
		}

		#region 用于 Pages

		public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
		{ }

		public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
		{
			if (IsDevelopmentMode == false && IsTest(context) == false && IsUpdateOutputFile(context) == false) {
				var result = GetStaticFileResult(context);
				if (result != null) {
					context.Result = result;
					return;
				}
			}
		}

		public void OnPageHandlerSelected(PageHandlerSelectedContext context)
		{ }

		#endregion 用于 Pages

		#region 用于 Views

		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (IsDevelopmentMode == false && IsTest(context) == false && IsUpdateOutputFile(context) == false) {
				var result = GetStaticFileResult(context);
				if (result != null) {
					context.Result = result;
					return;
				}
			}
			await base.OnActionExecutionAsync(context, next);
		}

		#endregion 用于 Views

		/// <summary>
		/// 尝试获取静态文件
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private ActionResult GetStaticFileResult(FilterContext context)
		{
			var filePath = GetOutputFilePath(context);
			var response = context.HttpContext.Response;
			// 从内存获取文件
			if (UseMemoryCache) {
				if (MemoryCache.TryGetValue(filePath, out PageCache page)) {
					var etag = page.LastWriteTimeUtc.Ticks.ToString();
					if (context.HttpContext.Request.Headers["If-None-Match"] == etag) {
						return new StatusCodeResult(304);
					}
					if (UseBrCompress || UseGzipCompress) {
						var sp = context.HttpContext.Request.Headers["Accept-Encoding"].ToString().ToLower().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
						if (UseBrCompress && sp.Contains("br") && File.Exists(filePath + ".br")) {
							response.Headers["Content-Encoding"] = "br";
							return new FileContentResult(page.BrHtml, "text/html");
						} else if (UseGzipCompress && sp.Contains("gzip") && File.Exists(filePath + ".gzip")) {
							response.Headers["Content-Encoding"] = "gzip";
							return new FileContentResult(page.GzipHtml, "text/html");
						}
					}
					return new FileContentResult(page.Html, "text/html");
				}
			}
			if (File.Exists(filePath)) {
				var fi = new FileInfo(filePath);
				var etag = fi.LastWriteTimeUtc.Ticks.ToString();
				if (context.HttpContext.Request.Headers["If-None-Match"] == etag) {
					return new StatusCodeResult(304);
				}
				if (UseMemoryCache) {
					var html = File.ReadAllText(filePath);
					SaveHtmlResultToMemoryCache(filePath, html);
				}

				response.Headers["Cache-Control"] = "max-age=" + (CurrPageExpireMinutes ?? ExpireMinutes) * 60;
				response.Headers["Etag"] = etag;
				response.Headers["Date"] = DateTime.Now.ToString("r");
				response.Headers["Expires"] = DateTime.Now.AddMinutes(CurrPageExpireMinutes ?? ExpireMinutes).ToString("r");

				if (UseBrCompress || UseGzipCompress) {
					var sp = context.HttpContext.Request.Headers["Accept-Encoding"].ToString().ToLower().Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
					if (UseBrCompress && sp.Contains("br") && File.Exists(filePath + ".br")) {
						response.Headers["Content-Encoding"] = "br";
						return new PhysicalFileResult(filePath + ".br", "text/html");
					} else if (UseGzipCompress && sp.Contains("gzip") && File.Exists(filePath + ".gzip")) {
						response.Headers["Content-Encoding"] = "gzip";
						return new PhysicalFileResult(filePath + ".gzip", "text/html");
					}
				}
				return new PhysicalFileResult(filePath, "text/html");
			}
			return null;
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			// 开发模式，已处理的，测试，不用保存到本地目录
			if (IsDevelopmentMode || context.Result is StatusCodeResult || context.Result is FileResult || IsTest(context)) {
				await base.OnResultExecutionAsync(context, next);
				return;
			}

			var filePath = GetOutputFilePath(context);
			var response = context.HttpContext.Response;
			if (!response.Body.CanRead || !response.Body.CanSeek) {
				using (var ms = new MemoryStream()) {
					var old = response.Body;
					response.Body = ms;

					await base.OnResultExecutionAsync(context, next);

					if (response.StatusCode == 200) {
						await SaveHtmlResult(response.Body, filePath);
					}
					ms.Position = 0;
					await ms.CopyToAsync(old);
					response.Body = old;
				}
			} else {
				await base.OnResultExecutionAsync(context, next);
				var old = response.Body.Position;
				if (response.StatusCode == 200) {
					await SaveHtmlResult(response.Body, filePath);
				}
				response.Body.Position = old;
			}
			//更新时，不添加页面缓存
			if (IsUpdateOutputFile(context) == false) {
				var fi = new FileInfo(filePath);
				var etag = fi.LastWriteTimeUtc.Ticks.ToString();
				context.HttpContext.Response.Headers["Cache-Control"] = "max-age=" + (CurrPageExpireMinutes ?? ExpireMinutes) * 60;
				context.HttpContext.Response.Headers["Etag"] = etag;
				context.HttpContext.Response.Headers["Date"] = DateTime.Now.ToString("r");
				context.HttpContext.Response.Headers["Expires"] = DateTime.Now.AddMinutes(CurrPageExpireMinutes ?? ExpireMinutes).ToString("r");
			}
		}

		/// <summary>
		/// 保存Html结束为静态文件
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		private async Task SaveHtmlResult(Stream stream, string filePath)
		{
			stream.Position = 0;
			var responseReader = new StreamReader(stream);
			var responseContent = await responseReader.ReadToEndAsync();
			if (MiniFunc != null) {//进行最小化处理
				responseContent = MiniFunc(responseContent);
			}
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));

			await File.WriteAllTextAsync(filePath, responseContent);

			if (UseGzipCompress || UseBrCompress || UseMemoryCache) {
				byte[] gzip = new byte[0];
				byte[] br = new byte[0];

				var htmlbs = Encoding.UTF8.GetBytes(responseContent);
				if (UseGzipCompress) {
					gzip = GzipCompress(htmlbs, false);
					await File.WriteAllBytesAsync(filePath + ".gzip", gzip);
				}
				if (UseBrCompress) {
					br = BrCompress(htmlbs, false);
					await File.WriteAllBytesAsync(filePath + ".br", br);
				}
				if (UseMemoryCache) {
					var pg = new PageCache(new FileInfo(filePath).LastWriteTimeUtc, htmlbs, gzip, br);
					MemoryCache[filePath] = pg;
				}
			}
		}

		/// <summary>
		/// 读取文件，保存到内存缓存内
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="html"></param>
		private void SaveHtmlResultToMemoryCache(string filePath, string html)
		{
			byte[] htmlbs = Encoding.UTF8.GetBytes(html);
			byte[] gzip = new byte[0];
			byte[] br = new byte[0];
			if (UseGzipCompress) {
				gzip = GzipCompress(htmlbs, false);
			}
			if (UseBrCompress) {
				br = BrCompress(htmlbs, false);
			}
			var pg = new PageCache(new FileInfo(filePath).LastWriteTimeUtc, htmlbs, gzip, br);
			MemoryCache[filePath] = pg;
		}

		/// <summary>
		/// 是否在测试
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private bool IsTest(FilterContext context)
		{
			return context.HttpContext.Request.Query.ContainsKey(TestQueryString);
		}

		/// <summary>
		/// 是否更新文件
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private bool IsUpdateOutputFile(FilterContext context)
		{
			return context.HttpContext.Request.Query.ContainsKey(UpdateFileQueryString);
		}

		/// <summary>
		/// 获取更新文件路径
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private string GetOutputFilePath(FilterContext context)
		{
			string dir = OutputFolder;
			if (string.IsNullOrEmpty(dir)) {
				dir = Path.Combine(Path.GetDirectoryName(typeof(HtmlStaticFileAttribute).Assembly.Location), "html");
				OutputFolder = dir;
			}

			var t = context.HttpContext.Request.Path.ToString().Replace("//", Path.DirectorySeparatorChar.ToString()).Replace("/", Path.DirectorySeparatorChar.ToString());
			if (t.EndsWith(Path.DirectorySeparatorChar)) {
				t += "index";
			}
			if (UseQueryString) {
				var list = new HashSet<string>();
				foreach (var item in context.HttpContext.Request.Query.Keys) {
					if (item != UpdateFileQueryString) {
						var value = context.HttpContext.Request.Query[item];
						if (string.IsNullOrEmpty(value)) {
							list.Add($"{list}_");
						} else {
							list.Add($"{list}_{value}");
						}
					}
				}
				t += Regex.Replace(string.Join(",", list), "[^0-9_a-zA-Z\u4E00-\u9FCB\u3400-\u4DB5\u3007]", "_");
			}

			t = t.TrimStart(Path.DirectorySeparatorChar);
			return Path.Combine(dir, t) + ".html";
		}

		/// <summary>
		/// Gzip压缩
		/// </summary>
		/// <param name="data">要压缩的字节数组</param>
		/// <param name="fastest">快速模式</param>
		/// <returns>压缩后的数组</returns>
		private byte[] GzipCompress(byte[] data, bool fastest = false)
		{
			if (data == null || data.Length == 0)
				return data;
			try {
				using (MemoryStream stream = new MemoryStream()) {
					var level = fastest ? CompressionLevel.Fastest : CompressionLevel.Optimal;
					using (GZipStream zStream = new GZipStream(stream, level)) {
						zStream.Write(data, 0, data.Length);
					}
					return stream.ToArray();
				}
			} catch {
				return data;
			}
		}

		/// <summary>
		/// Br压缩
		/// </summary>
		/// <param name="data">要压缩的字节数组</param>
		/// <param name="fastest">快速模式</param>
		/// <returns>压缩后的数组</returns>
		private byte[] BrCompress(byte[] data, bool fastest = false)
		{
			if (data == null || data.Length == 0)
				return data;
			try {
				using (MemoryStream stream = new MemoryStream()) {
					var level = fastest ? CompressionLevel.Fastest : CompressionLevel.Optimal;
					using (BrotliStream zStream = new BrotliStream(stream, level)) {
						zStream.Write(data, 0, data.Length);
					}
					return stream.ToArray();
				}
			} catch {
				return data;
			}
		}
	}
}