#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/mode-text.js")]
        public IActionResult __inputControl_ace_mode_text_js()
        {
            if (SetResponseHeaders("5854CFBCBFB7CFAF9CC9350E9442A40E") == false) { return StatusCode(304); }
            const string s = "G4sAAGRwXqqDr5CW0kE75cClhQFFlIU3vNHp4CHbbEJ0IQp6Yjbeh/DcDFyEi+I0d/oxy50tvzbkm/xrkO92T/lYgH7M9i7XGo1Jk6cbYizc8msal201ZMZEUTYWg/zlL4UE";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif