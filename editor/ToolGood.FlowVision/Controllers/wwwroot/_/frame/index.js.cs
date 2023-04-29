#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/frame/index.js")]
        public IActionResult __frame_index_js()
        {
            if (SetResponseHeaders("01C6B63DB74927D3C12F446280A92462") == false) { return StatusCode(304); }
            const string s = "G6sBAMSCuaW6PfYHXqehopmJqdkOQKa233ADCri7wJrF2cSSLrVAAw1qm7gI7gb5XXP0/RMzm+O60HCDuTMrIVS3TNfCtGOC3OicmGMbXh2zMZm+fz/JLTOh8xy7o0Rh/TRt91fQ2SnrWhLvK84hmlcqTDu+JK7XECys6D9rKc5ZbdgWBEjjKQ8ZFCZUzc6ZYFYwyN5JPTOImrn7ffMHY+3MSsjULdPGLAqCfJZTmIWN6xBkouEtVMwNizuUa1izB4JmxQoG";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif