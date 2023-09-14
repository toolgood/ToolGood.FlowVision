#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-error_marker.js")]
        public IActionResult __inputControl_ace_ext_error_marker_js()
        {
            if (SetResponseHeaders("AE7FDE0D66D4A45F88E55687075656F0") == false) { return StatusCode(304); }
            const string s = "G5IAAGRwXqonQlpKBwdf1MmBy3c80P3gWZhhRAEFHrIkRBeijV4xG+9DeHYG7qCwugHm8NpyuCx6aXaDZrGqJPZS4bvdE9TIkHrcBQTA0j2Dw2Z5nrnBa9LLtgq8PS8IKsOKCv/wD4MQ";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif