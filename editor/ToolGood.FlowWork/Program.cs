using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ToolGood.FlowWork.Commons;
using ToolGood.FlowWork.Commons.Jsons;
using ToolGood.FlowWork.Commons.My;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace ToolGood.FlowWork
{
	public class Program
	{
		public static void Main(string[] args)
		{
#if DEBUG
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

			var builder = WebApplication.CreateBuilder(args);
			builder.Logging.AddFilter("Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware", q => false);

			// Add services to the container.
			builder.Services.AddRazorPages();
			builder.Services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;// WhenWritingNull忽略所有null属性  WhenWritingDefault忽略所有默认值属性
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // 忽略循环引用
                options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping; //关闭字符转义
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss")); // 时间自定义转换器
                options.JsonSerializerOptions.AllowTrailingCommas = true; // 尾随逗号
                options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;// 允许注释
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;// 带引号数字
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // 驼峰命名
                options.JsonSerializerOptions.WriteIndented = false; // 无缩进
                options.JsonSerializerOptions.IncludeFields = false;// 不支持字段(Field)
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;//忽略所有只读属性
            });
#if DEBUG
			builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
#endif

			builder.Services.AddWebMarkupMin(options => {
				options.AllowMinificationInDevelopmentEnvironment = true;
				options.AllowCompressionInDevelopmentEnvironment = true;
			}).AddHtmlMinification(options => {
				options.SupportedHttpMethods = new HashSet<string> { "GET", "POST" };
				HtmlMinificationSettings settings = options.MinificationSettings;
				settings.RemoveRedundantAttributes = true;
				settings.RemoveHttpProtocolFromAttributes = true;
				settings.RemoveHttpsProtocolFromAttributes = true;

				options.CssMinifierFactory = new NUglifyCssMinifierFactory();
				options.JsMinifierFactory = new NUglifyJsMinifierFactory();
				//}).AddXhtmlMinification(options => {
				//    options.SupportedHttpMethods = new HashSet<string> { "GET", "POST" };
				//    XhtmlMinificationSettings settings = options.MinificationSettings;
				//    settings.RemoveRedundantAttributes = true;
				//    settings.RemoveHttpProtocolFromAttributes = true;
				//    settings.RemoveHttpsProtocolFromAttributes = true;
				//    options.CssMinifierFactory = new KristensenCssMinifierFactory();
				//    options.JsMinifierFactory = new CrockfordJsMinifierFactory();
				//}).AddXmlMinification(options => {
				//    options.SupportedHttpMethods = new HashSet<string> { "GET", "POST" };
				//    XmlMinificationSettings settings = options.MinificationSettings;
				//    settings.CollapseTagsWithoutContent = true;
			}).AddHttpCompression(options => {
				options.CompressorFactories = new List<ICompressorFactory> {
					new BuiltInBrotliCompressorFactory(new BuiltInBrotliCompressionSettings{ Level= CompressionLevel.Fastest}),
					new DeflateCompressorFactory(new DeflateCompressionSettings { Level = CompressionLevel.Fastest }),
					new GZipCompressorFactory(new GZipCompressionSettings { Level = CompressionLevel.Fastest })
				};
			});

			var app = builder.Build();
			var env = app.Services.GetService<IWebHostEnvironment>();
			MyHostingEnvironment.ApplicationName = env.ApplicationName;
			MyHostingEnvironment.ContentRootPath = env.ContentRootPath;
			MyHostingEnvironment.EnvironmentName = env.EnvironmentName;
			MyHostingEnvironment.WebRootPath = env.WebRootPath;


			ProjectWorkCache.PorjectName = app.Configuration.GetValue<string>("FlowSetting:PorjectName");
			ProjectWorkCache.PorjectCode = app.Configuration.GetValue<string>("FlowSetting:PorjectCode");
			ProjectWorkCache.Update();

			// Configure the HTTP request pipeline.

#if DEBUG
			if (!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Error");
			}
			app.UseStaticFiles();
#else
            app.UseExceptionHandler("/Error");
#endif

			app.UseRouting();

			app.MapControllers();
#if !DEBUG
            app.UseWebMarkupMin();
#endif
			app.MapRazorPages();
			app.Run();
		}
	}
}