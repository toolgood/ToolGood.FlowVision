#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowWork.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/css/modules/layer/default/layer.css")]
        public IActionResult __layui_css_modules_layer_default_layer_css()
        {
            if (SetResponseHeaders("98D537E9ACC5DD9D748731B54BF36ED4") == false) { return StatusCode(304); }
            const string s = "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";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif