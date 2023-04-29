using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jint;

namespace ToolGood.WwwRoot
{
    public class JsEncryption : IDisposable
    {
        private Engine engine;

        public JsEncryption()
        {
            engine = new Engine(options => {
                //options.TimeoutInterval(TimeSpan.FromSeconds(0.5));
                //options.AllowDebuggerStatement(true);
                options.DebugMode(true);
            });
            //"";
            var ass = typeof(JsEncryption).Assembly;
            {
                var tStr = File.ReadAllText("WwwRoot/dict/typedarray.js");
                //string resourceName = "ToolGood.WwwRoot.dict.typedarray.js";
                //Stream sm = ass.GetManifestResourceStream(resourceName);
                //byte[] bs = new byte[sm.Length];
                //sm.Read(bs, 0, (int)sm.Length);
                //sm.Close();
                //var tStr = Encoding.UTF8.GetString(bs);

                engine.Execute(tStr);
            }
            {
                var tStr = File.ReadAllText("WwwRoot/dict/ug.js");
                //string resourceName = "ToolGood.WwwRoot.dict.ug.js";
                //Stream sm = ass.GetManifestResourceStream(resourceName);
                //byte[] bs = new byte[sm.Length];
                //sm.Read(bs, 0, (int)sm.Length);
                //sm.Close();
                //var tStr = Encoding.UTF8.GetString(bs);

                engine.Execute(tStr);
            }


            engine.Execute(@"function jsEncryption(tpl){
			return myUg(tpl);	
	}");
        }
        public string BuildTemplate(string fun )
        {
            try {
                //engine.ResetTimeoutTicks();
                var value = engine.Invoke("jsEncryption", fun).ToString();
                return GetSrcText(value);
            } catch (Exception ex) { 
            }
            return null;
        }
        private string GetSrcText(string value)
        {
            StringBuilder s = new StringBuilder();
            var xx = false;
            for (int i = 0; i < value.Length; i++) {
                var c = value[i];
                if (xx) {
                    xx = false;
                    if (c == 'r') {
                        s.Append("\r");
                    } else if (c == 'n') {
                        s.Append("\n");
                    } else if (c == 't') {
                        s.Append("\t");
                    } else {
                        s.Append(c);
                    }
                } else if (c == '\\') {
                    xx = true;
                } else {
                    s.Append(c);
                }
            }
            return s.ToString();
        }
        public void Dispose()
        {
            engine = null;
        }

    }
}
