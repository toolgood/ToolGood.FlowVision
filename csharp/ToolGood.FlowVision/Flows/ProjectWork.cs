using Antlr4.Runtime;
using ToolGood.Algorithm.Internals;
using ToolGood.Algorithm;
using static ToolGood.Algorithm.mathParser;
using ToolGood.FlowVision.Commons;
using System.Text;
using ToolGood.FlowVision.Flows.Common;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using ToolGood.Algorithm.Enums;
using System.Collections.Generic;
using System;
using System.IO;

namespace ToolGood.FlowVision.Flows
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

        public Dictionary<string, string> FormulaList { get; set; }
        public Dictionary<string, FactoryWork> FactoryList { get; set; }
        public Dictionary<string, FactoryMachineWork> FactoryMachineList { get; set; }
        public Dictionary<string, FactoryProcedureWork> FactoryProcedureList { get; set; }
        public Dictionary<string, AppWork> AppList { get; set; }

        private Dictionary<string, ProgContext> ProgList = new Dictionary<string, ProgContext>();


        internal ProgContext CreateProgContext(string exp)
        {
            if (ProgList.TryGetValue(exp, out ProgContext temp)) {
                return temp;
            }
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
  
 
        public static ProjectWork LoadJsonWithRsa(byte[] bytes)
        {
            var publicKey = "<RSAKeyValue><Modulus>u3W3xI6mH9tr3A+sNZVhyIbQWFhePbPWdFeTtM39yR7kO4Akp6Dsb1NYKpKxSGjIwDv1TC6/IUwOgOYYSVa0pgfIujHPrYFO/LlDk6kPAyHluLimKFkHkze5FsY7YAqd2mExqdJ4Zfb1pXgIrVFgOs4o69p9vyBV6kWS0FBOnyyUK92bRYxeqS1raRfM3GUlIEaQW5ZIuJxQtFrfwSnsfDVhkp8rvFVt7I5aqawWeoJZu+/HZqQO/gz+BJ7ntlUWoPgfe13/U3kIOHMTc/Deczb5x3DeBv9XrwJ5+DrzrJV8jTdhiYeNcBNBYaKoHGS15chxt6no4sIDZYsI2N4ciQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            return LoadJsonWithRsa(publicKey, bytes);
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

    }

}
