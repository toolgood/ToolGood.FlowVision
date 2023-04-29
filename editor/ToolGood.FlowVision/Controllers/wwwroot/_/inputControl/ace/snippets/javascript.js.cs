#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/snippets/javascript.js")]
        public IActionResult __inputControl_ace_snippets_javascript_js()
        {
            if (SetResponseHeaders("0AAA9B7EC9169559346C5C04492DE676") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif