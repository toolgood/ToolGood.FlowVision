#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-textarea.js")]
        public IActionResult __inputControl_ace_ext_textarea_js()
        {
            if (SetResponseHeaders("4967E31213B957311917CEEB5D6B8710") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif