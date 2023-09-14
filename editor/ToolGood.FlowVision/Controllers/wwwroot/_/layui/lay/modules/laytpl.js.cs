#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/lay/modules/laytpl.js")]
        public IActionResult __layui_lay_modules_laytpl_js()
        {
            if (SetResponseHeaders("1C1008873C71F0D3A79A699D4D32E287") == false) { return StatusCode(304); }
            const string s = "GxQHAOR/Zyqiyu9h/UlVeqBG+zsuNLQOLU76nORblisftZVethAZwoN8E/fzzxcMU40uqmobbvbGw6IqixT+mTZTljZ1WEwfDjgAF5P6dgzF69Gbh8sm8tfN/2WP01MxiIKmrud2BdMEelMEW6wgJUj2qRgE08Qv/UqP5DLwd6LlZ/GL3feXXjoNDjDpxyMPr/Jqr52u7NyopjWcyHWej3l+t0H1ETTI9XY6SRt89xE2az/Pl5uiXW4npZtnAFSF6XpuVa0K4x1fyWqxW0yax33R82FxFwffOhh0Xfzd8lx+XJx8XBcXb58v/lxe3G6UxaXTsB37AGH8+5XCEC/QBxgOlDWe3NxigOCNj8cuWsCkeRi6gUWG9tz/WUXws3iNfRDfh6Eb/v/7Cza+9rxX/TXasQtMRNDtat5HOD+/jzeQfWWlnIK8BSU9KSj+FJVyKX332BEbful1STz57St/SuwDuWRL00cqTWkl2N3jw83wR7oUIpsfseqHIwjaxdDjgWLlR91QMGntaSdhC2kbCiegAVDXtJMJpMPp/SYbKDD29nxUcz7MeZzzduk0CDrzdhKRjRo9qBjSglcC1JAEFIx/Wh45z5dOQ57n+Qm0kEZ9xIU2WYcM2OLUUqsXEGf7AlADagIApQaDKWdT5K6C4zrPBdLgS3d6BZjSnN+mgrOQHwnmTNa/EulsOrI8ieEkeh/xdXYQVloysJmKG8ngJwCqzD55flYEWbKW5Cqktz4Vg2gpAyUz60sZGiMfIYVExAuKNSQPLQCpI6Uw6PsfALS+lMstLU3kMcoOcVKVL7e0DAzuCeF+ZqjuZZaoTFB+ocNcEhVkCW2gDI4jizEOfh+hqyY8eX4WJCBTQWWQkh2ePD/bzMbhdXow+2JfMQVq+Vn8aBsVHLTwQQQ6UG0ugnSazSIL0xMtB1Q4HThwZAGIyjy6+xW6wdS/QGbg9sBDf0IrsLetrS/loprbKJwR0HbiUMQCMNXkHSd+fJ8nK78YKz8BXKQ3sQ+6Yj1uMW3N6uRkjSMn7jAmLBxOvpTzzMQtiGAcyynAmbv95qYPRWTRdlH84wlswCS9nZ9FKR0mezD7ri29i3YIThVV8zwl+4F6Fid8KyqcirXbULV2m5TswTwRXJlrcwl2LyHMXGPQB0xoAQ==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif