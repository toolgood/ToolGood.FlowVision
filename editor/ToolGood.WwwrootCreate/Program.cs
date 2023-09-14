using ToolGood.WwwRoot;

namespace ToolGood.WwwrootCreate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var root = GetRoot(Directory.GetCurrentDirectory());
            {
                WwwRootSetting setting = new WwwRootSetting();
                setting.NameSpace = "ToolGood.FlowWork.Controllers";
                setting.InFolderPath = Path.Combine(root, @"ToolGood.FlowWork\wwwroot");
                setting.OutFolderPath = Path.Combine(root, @"ToolGood.FlowWork\Controllers\wwwroot");
                //setting.CompressionType = "gzip";

                setting.ExcludeFileSuffixs.Add(".old.js");
                setting.ExcludeFileSuffixs.Add(".src.js");
                setting.ExcludeFileSuffixs.Add(".map");
                setting.BuildControllers();
            }
            {
                WwwRootSetting setting = new WwwRootSetting();
                setting.NameSpace = "ToolGood.FlowVision.Controllers";
                setting.InFolderPath = Path.Combine(root, @"ToolGood.FlowVision\wwwroot");
                setting.OutFolderPath = Path.Combine(root, @"ToolGood.FlowVision\Controllers\wwwroot");
                //setting.CompressionType = "gzip";

                setting.ExcludeFileSuffixs.Add(".old.js");
                setting.ExcludeFileSuffixs.Add(".src.js");
                setting.ExcludeFileSuffixs.Add(".map");
                setting.BuildControllers();
            }
            //{
            //    WwwRootSetting setting = new WwwRootSetting();
            //    setting.NameSpace = "ToolGood.FlowVision.Controllers";
            //    setting.InFolderPath = Path.Combine(root, @"MinSrc\ToolGood.FlowVision\wwwroot");
            //    setting.OutFolderPath = Path.Combine(root, @"MinSrc\ToolGood.FlowVision\Controllers\wwwroot");
            //    //setting.CompressionType = "gzip";

            //    setting.ExcludeFileSuffixs.Add(".old.js");
            //    setting.ExcludeFileSuffixs.Add(".src.js");
            //    setting.ExcludeFileSuffixs.Add(".map");
            //    setting.BuildControllers();
            //}
        }

        static string GetRoot(string path)
        {
            var type = typeof(Program).Namespace;
            while (Path.GetFileName(path)!= type) {
                path = Path.GetDirectoryName(path);
            }
            path = Path.GetDirectoryName(path);
            return path;
        }
    }
}