#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/lay/all.js")]
        public IActionResult __layui_lay_all_js()
        {
            if (SetResponseHeaders("B034B5AD7386BAC65A568336AECC6516") == false) { return StatusCode(304); }
            const string s = "G3gAAGRwXqonulLjwNOOcWJ+6zbIhhMtg+Pw3xsR3VHtwL2L6Fw2ydzcofseYFFAbhEEECepnuNABXNeLJYERFgs9d5xB+4ixmTditiIA01zfzeFjhZD0HVaAA73d1M=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif