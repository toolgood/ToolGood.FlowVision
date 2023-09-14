using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using ToolGood.FlowVision.Commons;
using ToolGood.FlowVision.Commons.Exceptions;
using ToolGood.FlowVision.Commons.Extensions;
using ToolGood.FlowVision.Commons.Jsons;
using ToolGood.FlowVision.Commons.My;
using WebMarkupMin.AspNet.Common.Compressors;
using WebMarkupMin.AspNetCore7;
using WebMarkupMin.Core;
using WebMarkupMin.NUglify;

namespace ToolGood.FlowVision
{
	public class Program
	{
		public static void Main(string[] args)
		{
			bool ret;
			System.Threading.Mutex mutex = new System.Threading.Mutex(true, "ToolGood.FlowVision", out ret);
			if (ret) {
#if DEBUG
				System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
#endif
				Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

				var builder = WebApplication.CreateBuilder(args);
				builder.Logging.AddFilter("System.Net.Http.HttpClient.Default.ClientHandler", q => false);
				builder.Logging.AddFilter("System.Net.Http.HttpClient.Default.LogicalHandler", q => false);
				builder.Logging.AddFilter("Microsoft.AspNetCore.Watch.BrowserRefresh.BrowserRefreshMiddleware", q => false);

				builder.Services.AddAntiforgery(options => {
					options.HeaderName = "__RequestVerificationToken";
					options.FormFieldName = "__RequestVerificationToken";
				});
#if DEBUG
				builder.Services.AddResponseCompression(options => {
					options.Providers.Add<BrotliCompressionProvider>();
					options.Providers.Add<GzipCompressionProvider>();
					options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml", "application/x-font-truetype" });
				});
#endif
				builder.Services.AddMemoryCache();
				builder.Services.AddSession(options => {
					options.Cookie.Name = "sid";
					options.IdleTimeout = TimeSpan.FromHours(8);
					options.IOTimeout = TimeSpan.FromSeconds(1);
					options.Cookie.IsEssential = true;
					options.Cookie.HttpOnly = true;
					options.Cookie.Path = "/";
					//options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None; //�Ա������ ajax ������Ч
					//options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
				});
				builder.Services.Configure<CookiePolicyOptions>(options => {
					options.CheckConsentNeeded = context => false;
					//options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None; //�Ա������ ajax ������Ч
					//options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
				});
				builder.Services.AddHttpContextAccessor();
				builder.Services.AddHttpClient();

				// Add services to the container.
				builder.Services.AddRazorPages();
				builder.Services.AddControllers(options => {
					options.Filters.Add<HttpGlobalExceptionFilter>();
				}).AddJsonOptions(options => {
					//options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // ����ѭ������
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping; //�ر��ַ�ת��
                    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss")); // ʱ���Զ���ת����
                    options.JsonSerializerOptions.Converters.Add(new BoolJsonConverter());
                    options.JsonSerializerOptions.AllowTrailingCommas = true; // β�涺��
                    options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;// ����ע��
                    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;// ����������
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // �շ�����
                    options.JsonSerializerOptions.WriteIndented = false; // ������
                    options.JsonSerializerOptions.IncludeFields = false;// ��֧���ֶ�(Field)
                    options.JsonSerializerOptions.IgnoreReadOnlyProperties = false;//��������ֻ������
                });
#if DEBUG
				builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
#endif
				builder.Services.RegisterAssemblyInterfaces("ToolGood.FlowVision.Applications", null, LifeStyle.PerLifetimeScope);

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
				}).AddHttpCompression(options => {
					options.CompressorFactories = new List<ICompressorFactory> {
					new BuiltInBrotliCompressorFactory( new BuiltInBrotliCompressionSettings{ Level= CompressionLevel.Fastest}),
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
				MyIoc.SetServiceProvider(app.Services);

#if DEBUG
				Config.Configuration = app.Services.GetService<IConfiguration>();
				if (!app.Environment.IsDevelopment()) {
					app.UseExceptionHandler("/Error");
				}
				app.UseResponseCompression();
				app.UseResponseCaching();
				app.UseStaticFiles();
#else
				app.UseExceptionHandler("/Error");
#endif

				app.UseRouting();

				app.UseAuthorization();
				app.UseCookiePolicy();
				app.UseSession();

				app.MapControllers();
#if !DEBUG
				app.UseWebMarkupMin();
#endif
				app.MapRazorPages();
				app.Run();
			} else {
				Console.WriteLine("����������,����������!");
			}
		}
	}
}