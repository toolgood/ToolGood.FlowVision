#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-spellcheck.js")]
        public IActionResult __inputControl_ace_ext_spellcheck_js()
        {
            if (SetResponseHeaders("67190A8F08930B174373F6B50406715C") == false) { return StatusCode(304); }
            const string s = "G2QFAGQzXb2+QdekQWvCPsnoSu0UBUyxGVumZ+KaJm/yyp1b286TKrGDhtH8ZUvwTXj2gAtbc412tZmAmDxLp6TZYWUdKcJ4qgM92CBvdb1eNXwI+BcMH3gMGPSwjckyMGzirKwVmC157ScD3WmwZ9is/khKn8bQ+jn84VEIH8bARmcoWatsyU8Nmt04VV4CgnMClxCnxtxq8x0c7I2G8nwcZmtNkv6MzrtxqkyCs3Gaq3ESc6YHexG2xT5Ln0Ni5cKx8S2ay31g4Hx+stnaEemQsjDqPCopx/Q+Zq/0M4gXYi5rzj4GN1f7FtPs4zjMFbNLcc/ZTYY/JxC3xMBnPdjUghqUc9TiSsNHdREUB0jNFC8xzYYt0e3bqAK/D/Xw96EeAhdp+1BBXzm5gAQphZPLap/aBivItsN2WM4M7g+JeKia60KU/RxxlN6QvRvPZq99jq1gi4lhpcdZ3Afo4pXVFOnsg2Hmp35jSk2UXt0R91fm0h7922IdDcKTvsUgUijAxTKAxrcYXqDZuxYL1yQiNZlFFVxe+6liaQLMdiSXSbaEyQqmnoQZicd4+9Z8dUJlUVfXJKIGU+2Fg7quhfWk8UMSgQpE++M9X/9aPsyxZiUWRujFy6sekkFk/rqrBS7YEwNQp21sR9R1TYIOgjsvuScZugEUAkCZHMrRoWBBTM5dqAyIrTgjf83ROauNoFblZdMBCoBYV+OdjEjp9UIT7rGYxIAQxuZ3iuYNNxoKMLglGlWSy3xKI0HqGBBpo17dddSRVfYgnbc9APFmDfGXt8vyUol0tuNWY1vdpiIQJ0udGty+XYi4w6wL9ou+27cxwzqy8EAddUg=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif