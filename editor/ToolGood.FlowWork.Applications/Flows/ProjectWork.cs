using Antlr4.Runtime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm.math;
using ToolGood.FlowWork.Applications.Flows.Common;
using ToolGood.FlowWork.Commons;
using static ToolGood.Algorithm.math.mathParser;

namespace ToolGood.FlowWork.Flows
{
    public sealed class ProjectWork
	{
		/// <summary>
		/// 名称
		/// </summary>
		public string Name { get; set; }

		public string Code { get; set; }

		public int ExcelIndex { get; set; }
		public bool NumberRequired { get; set; }
		public int Distance { get; set; }
		public int Area { get; set; }
		public int Volume { get; set; }
		public int Mass { get; set; }

		public Dictionary<string, string> FormulaList { get; set; }
		public Dictionary<string, FactoryWork> FactoryList { get; set; }
		public Dictionary<string, FactoryMachineWork> FactoryMachineList { get; set; }
		public Dictionary<string, FactoryProcedureWork> FactoryProcedureList { get; set; }
		public Dictionary<string, AppWork> AppList { get; set; }

		private Dictionary<string, ProgContext> ProgList = new Dictionary<string, ProgContext>();

		internal ProgContext CreateProgContext(string exp)
		{
			if (string.IsNullOrWhiteSpace(exp)) { return null; }
			
			var stream = new AntlrCharStream(new AntlrInputStream(exp));
			var lexer = new mathLexer(stream);
			var tokens = new CommonTokenStream(lexer);
			var parser = new mathParser(tokens);
			var antlrErrorListener = new AntlrErrorListener();
			parser.RemoveErrorListeners();
			parser.AddErrorListener(antlrErrorListener);

			var context = parser.prog();
			if (antlrErrorListener.IsError) { throw new Exception(antlrErrorListener.ErrorMsg); }
			ProgList[exp] = context;
			return context;
		}

		internal bool TryGetFormula(string name, out ProgContext context)
		{
			if (FormulaList.TryGetValue(name, out string exp)) {
				context = CreateProgContext(exp);
				return true;
			}
			context = null;
			return false;
		}

		//public static ProjectWork LoadJson2(string json)
		//{
		//    ProjectWork work = JsonConvert.DeserializeObject<ProjectWork>(json, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
		//    foreach (var item in work.FactoryList) {
		//        item.Value.Init(work);
		//    }
		//    foreach (var item in work.FactoryMachineList) {
		//        item.Value.Init(work);
		//    }
		//    foreach (var item in work.FactoryProcedureList) {
		//        item.Value.Init(work);
		//    }
		//    foreach (var item in work.AppList) {
		//        item.Value.Init(work);
		//    }
		//    return work;
		//}
		//public string SaveJson2()
		//{
		//    var settings = new JsonSerializerSettings();
		//    settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
		//    settings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
		//    settings.TypeNameHandling = TypeNameHandling.Auto;
		//    settings.DefaultValueHandling = DefaultValueHandling.Ignore;
		//    return JsonConvert.SerializeObject(this, settings);
		//}
		public static ProjectWork LoadJson(string json)
		{
			JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions();
			jsonSerializerSettings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
			jsonSerializerSettings.PropertyNameCaseInsensitive = true;
			jsonSerializerSettings.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			jsonSerializerSettings.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			jsonSerializerSettings.AllowTrailingCommas = true;
			jsonSerializerSettings.ReadCommentHandling = JsonCommentHandling.Skip;
			jsonSerializerSettings.NumberHandling = JsonNumberHandling.AllowReadingFromString;
			jsonSerializerSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			jsonSerializerSettings.WriteIndented = false;
			//jsonSerializerSettings.Converters.Add(new NodeWorkConverter());
			ProjectWork work = System.Text.Json.JsonSerializer.Deserialize<ProjectWork>(json, jsonSerializerSettings);

			foreach (var item in work.FactoryList) {
				item.Value.Init(work);
			}
			foreach (var item in work.FactoryMachineList) {
				item.Value.Init(work);
			}
			foreach (var item in work.FactoryProcedureList) {
				item.Value.Init(work);
			}
			foreach (var item in work.AppList) {
				item.Value.Init(work);
			}
			return work;
		}
		public void Init()
		{
			foreach (var item in this.FactoryList) {
				item.Value.Init(this);
			}
			foreach (var item in this.FactoryMachineList) {
				item.Value.Init(this);
			}
			foreach (var item in this.FactoryProcedureList) {
				item.Value.Init(this);
			}
			foreach (var item in this.AppList) {
				item.Value.Init(this);
			}
		}

		public string SaveJson()
		{
			JsonSerializerOptions jsonSerializerSettings = new JsonSerializerOptions();
			jsonSerializerSettings.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
			jsonSerializerSettings.PropertyNameCaseInsensitive = true;
			jsonSerializerSettings.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			jsonSerializerSettings.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
			jsonSerializerSettings.AllowTrailingCommas = true;
			jsonSerializerSettings.ReadCommentHandling = JsonCommentHandling.Skip;
			jsonSerializerSettings.NumberHandling = JsonNumberHandling.AllowReadingFromString;
			jsonSerializerSettings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			jsonSerializerSettings.WriteIndented = false;
			//jsonSerializerSettings.Converters.Add(new NodeWorkConverter());

			return System.Text.Json.JsonSerializer.Serialize(this, jsonSerializerSettings);
		}

		public byte[] SaveJsonWithRsa(string privateKey)
		{
			var json = this.SaveJson();
			using (var ms = new MemoryStream()) {
				using (var bw = new BinaryWriter(ms)) {
					byte[] bytes = null;
					string key = null;
					do {
						key = Guid.NewGuid().ToString();
						bytes = RsaUtil.PrivateEncrypt(privateKey, Encoding.UTF8.GetBytes(key));
					} while (bytes.Length != 256);
					bw.Write(bytes.Length);
					bw.Write(bytes);

					var bytes3 = CompressionUtil.BrCompress(Encoding.UTF8.GetBytes(json), true);
					var bytes2 = RcxCrypto.RCY.Encrypt(bytes3, key);
					bw.Write(bytes2.Length);
					bw.Write(bytes2);
				}
				return ms.ToArray();
			}
		}

		public static ProjectWork LoadJsonWithRsa(string publicKey, byte[] bytes)
		{
			using (var ms = new MemoryStream(bytes)) {
				using (var br = new BinaryReader(ms)) {
					var length = br.ReadInt32();
					var bs = br.ReadBytes(length);
					bs = RsaUtil.PublicDecrypt(publicKey, bs);
					var key = Encoding.UTF8.GetString(bs);

					length = br.ReadInt32();
					var bs2 = br.ReadBytes(length);
					bs2 = RcxCrypto.RCY.Encrypt(bs2, bs);
					bs2 = CompressionUtil.BrDecompress(bs2);
					var json = Encoding.UTF8.GetString(bs2);

					return LoadJson(json);
				}
			}
		}
	}
}