using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ToolGood.Bedrock;

namespace ToolGood.WwwRoot
{
    /// <summary>
    /// wwwroot 生成 *.cs 文件
    /// </summary>
    public class WwwRootSetting
    {
        /// <summary>
        /// 压缩类型 默认 br, 可选 gzip (火狐http不支持br压缩)
        /// </summary>
        public string CompressionType { get; set; } = "br";
        /// <summary>
        /// 输入文件夹路径
        /// </summary>
        public string InFolderPath { get; set; }

        /// <summary>
        /// 输出文件夹路径
        /// </summary>
        public string OutFolderPath { get; set; }

        /// <summary>
        /// 排除文件后缀
        /// </summary>
        public List<string> ExcludeFileSuffixs { get; set; } = new List<string>();

        /// <summary>
        /// 排除文件
        /// </summary>
        public List<string> ExcludeFiles { get; set; } = new List<string>();

        /// <summary>
        /// js加密,比较点慢 
        /// </summary>
        public List<string> JsEncryptionFiles { get; set; } = new List<string>();

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace { get; set; }

        /// <summary>
        /// 控制器名
        /// </summary>
        public string ControllerName { get; set; } = "WwwRootController";

        #region 模板
        private string brFirstTemplate = @"#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (SetResponseHeaders(""{FileHash}"") == false) { return StatusCode(304); }
            const string s = ""{FileContent}"";
            var bytes = UseCompressBytes(s);
            return File(bytes, ""{FileMime}"");
        }
        private bool SetResponseHeaders(string etag)
        {
            if (Request.Headers[""If-None-Match""] == etag) { return false; }
            Response.Headers[""Cache-Control""] = ""max-age=315360000"";
            Response.Headers[""Etag""] = etag;
            Response.Headers[""Date""] = DateTime.Now.ToString(""r"");
            Response.Headers[""Expires""] = DateTime.Now.AddYears(100).ToString(""r"");
            return true;
        }
        private byte[] UseCompressBytes(string s)
        {
            var bytes = Convert.FromBase64String(s);
            var sp = Request.Headers[""Accept-Encoding""].ToString().Replace("" "", """").ToLower().Split(',');
            if (sp.Contains(""br"")) {
                Response.Headers[""Content-Encoding""] = ""br"";
            } else  {
                using (MemoryStream stream = new MemoryStream(bytes)) {
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            bytes = resultStream.ToArray();
                        }
                    }
                }
                if (sp.Contains(""gzip"")) {
                    Response.Headers[""Content-Encoding""] = ""gzip"";
                    using (MemoryStream stream = new MemoryStream()) {
                        using (GZipStream zStream = new GZipStream(stream, CompressionMode.Compress)) {
                            zStream.Write(bytes, 0, bytes.Length);
                            zStream.Close();
                        }
                        bytes = stream.ToArray();
                    }
                }
            }
            return bytes;
        }
    }
}
#endif";
        private string gzipFirstTemplate = @"#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (SetResponseHeaders(""{FileHash}"") == false) { return StatusCode(304); }
            const string s = ""{FileContent}"";
            var bytes = UseCompressBytes(s);
            return File(bytes, ""{FileMime}"");
        }
        private bool SetResponseHeaders(string etag)
        {
            if (Request.Headers[""If-None-Match""] == etag) { return false; }
            Response.Headers[""Cache-Control""] = ""max-age=315360000"";
            Response.Headers[""Etag""] = etag;
            Response.Headers[""Date""] = DateTime.Now.ToString(""r"");
            Response.Headers[""Expires""] = DateTime.Now.AddYears(100).ToString(""r"");
            return true;
        }
        private byte[] UseCompressBytes(string s)
        {
            var bytes = Convert.FromBase64String(s);
            var sp = Request.Headers[""Accept-Encoding""].ToString().Replace("" "", """").ToLower().Split(',');
            if (sp.Contains(""gzip"")) {
                Response.Headers[""Content-Encoding""] = ""gzip"";
            } else  {
                using (MemoryStream stream = new MemoryStream(bytes)) {
                    using (GZipStream zStream = new GZipStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            bytes = resultStream.ToArray();
                        }
                    }
                }
                if (sp.Contains(""br"")) {
                    Response.Headers[""Content-Encoding""] = ""br"";
                    using (MemoryStream stream = new MemoryStream()) {
                        using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Compress)) {
                            zStream.Write(bytes, 0, bytes.Length);
                            zStream.Close();
                        }
                        bytes = stream.ToArray();
                    }
                }
            }
            return bytes;
        }
    }
}
#endif";
        private string Template = @"#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace {NameSpace}
{
    public partial class {ControllerName} : Controller
    {
        [HttpGet(""{FileUrl}"")]
        public IActionResult {FileMethod}()
        {
            if (SetResponseHeaders(""{FileHash}"") == false) { return StatusCode(304); }
            const string s = ""{FileContent}"";
            var bytes = UseCompressBytes(s);
            return File(bytes, ""{FileMime}"");
        }
    }
}
#endif";
        #endregion

        /// <summary>
        /// 生成控制器
        /// </summary>
        public void BuildControllers()
        {
            JsEncryption jsEncryption = new JsEncryption();
            if (string.IsNullOrEmpty(InFolderPath)) { throw new ArgumentNullException("FolderPath"); }
            if (string.IsNullOrEmpty(OutFolderPath)) { throw new ArgumentNullException("OutFolderPath"); }
            if (string.IsNullOrEmpty(NameSpace)) { throw new ArgumentNullException("NameSpace"); }
            if (string.IsNullOrEmpty(ControllerName)) { throw new ArgumentNullException("ControllerName"); }

            var setting = this;
            if (setting.InFolderPath.EndsWith("\\") == false) { setting.InFolderPath += "\\"; }
            if (setting.OutFolderPath.EndsWith("\\") == false) { setting.OutFolderPath += "\\"; }

            var jseFile = new List<string>();
            foreach (var item in JsEncryptionFiles) {
                jseFile.Add(Path.GetFullPath(item));
            }


            var files = Directory.GetFiles(setting.InFolderPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++) {
                var file = files[i];
                if (ExcludeFileSuffixs != null && ExcludeFileSuffixs.Count > 0) {
                    var isExclude = false;
                    foreach (var suffix in ExcludeFileSuffixs) {
                        if (file.EndsWith(suffix)) { isExclude = true; break; }
                    }
                    if (isExclude) { continue; }
                }
                if (ExcludeFiles != null && ExcludeFiles.Count > 0) {
                    if (ExcludeFiles.Contains(Path.GetFileName(file))) { continue; }
                }

                Console.WriteLine("Load:" + file);
                var bytes = File.ReadAllBytes(file);

                WwwRootFile wwwRootFile = new WwwRootFile() {
                    FilePath = file,
                    //FileContent = Convert.ToBase64String(bytes),
                    FileHash = HashUtil.GetMd5String(bytes),
                    FileName = Path.GetFileName(file),
                };
                
                if (jseFile.Contains(file) && file.EndsWith(".js", StringComparison.OrdinalIgnoreCase)) {
                    wwwRootFile.FileContent = wwwRootFile.GetFileContent(bytes, file, CompressionType);
                    if (string.IsNullOrWhiteSpace(wwwRootFile.FileContent) == false) {
                        var c = Encoding.UTF8.GetString(bytes);
                        var t = jsEncryption.BuildTemplate(c);
                        c = Convert.ToBase64String(Encoding.UTF8.GetBytes(t));
                        wwwRootFile.FileContent = c;
                    }
                } else {
                    wwwRootFile.FileContent = wwwRootFile.GetFileContent(bytes, file, CompressionType);
                }
                wwwRootFile.FileMime = wwwRootFile.GetFileMime(file);
                wwwRootFile.FileUrl = file.Substring(setting.InFolderPath.Length).Replace("\\", "/").TrimStart('/');
                wwwRootFile.FileMethod =
                    Regex.Replace(wwwRootFile.FileUrl,
                    @"[^0-9_a-zA-Z\u4E00-\u9FCB\u3400-\u4DB5]", "_");
                //wwwRootFile.FileMethod = wwwRootFile.FileMethod.Replace("@", "_");
                if ("0123456789".Contains(wwwRootFile.FileMethod[0])) {
                    wwwRootFile.FileMethod = "_" + wwwRootFile.FileMethod;
                }

                var txt = setting.Template;
                if (i == 0) {
                    if (CompressionType == "gzip") {
                        txt = setting.gzipFirstTemplate;
                    } else {
                        txt = setting.brFirstTemplate;
                    }
                }
                txt = txt.Replace("{NameSpace}", setting.NameSpace);
                txt = txt.Replace("{ControllerName}", setting.ControllerName);
                txt = txt.Replace("{FileUrl}", wwwRootFile.FileUrl);
                txt = txt.Replace("{FileMethod}", wwwRootFile.FileMethod);
                txt = txt.Replace("{FileHash}", wwwRootFile.FileHash);
                txt = txt.Replace("{FileContent}", wwwRootFile.FileContent);
                txt = txt.Replace("{FileMime}", wwwRootFile.FileMime);

                var outPath = file.Replace(setting.InFolderPath, setting.OutFolderPath) + ".cs";
                Directory.CreateDirectory(Path.GetDirectoryName(outPath));

                File.WriteAllText(outPath, txt);
                Console.WriteLine("Out:" + outPath);

            }
        }


    }
}
