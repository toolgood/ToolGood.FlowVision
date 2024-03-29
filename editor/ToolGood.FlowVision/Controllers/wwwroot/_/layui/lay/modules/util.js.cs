#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/lay/modules/util.js")]
        public IActionResult __layui_lay_modules_util_js()
        {
            if (SetResponseHeaders("A41E158136C1C4715A53EBF7D138A36D") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif