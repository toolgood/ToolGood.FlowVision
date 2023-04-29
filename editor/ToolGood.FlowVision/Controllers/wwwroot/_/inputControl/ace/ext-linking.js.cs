#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-linking.js")]
        public IActionResult __inputControl_ace_ext_linking_js()
        {
            if (SetResponseHeaders("EF7B824A6ACBDF385B94603DB4470085") == false) { return StatusCode(304); }
            const string s = "GwUEAGRdaq9v6gfCkkWv31Tc3r+xUlPRYSpieiNi4prnX6oP+uYVVLHSCmqNtixLnL4tGnBEA5rOaUzGtX0KBK3l1Gnxb2U/ZAtfHY/5V8erKMsoc6AXWDvwGhDw7yZl7YFgnWZlxUA+oVnUlBvyuyQhzuGNlhykWSHFQU400QoO26/cRC/O+98peHFz1oOu49UV/1mcxmCDKZhJugXuUx+DfAeJfXQ9931MQsWzm7M+pyXLgdrscvqh7PaqDwtO1c0Gfz3MQ5ynLWdjdE+MfDEmug9eR7WwirJ89rgrCjgtXYCG3RR+KJOmJUtbauAj+VJ51XPTrUps8AqovxE/mmDFGs4lM8ASFSyTDotqEsB3L97vGJNnpzKFHR0w7AOgiBYPZe1oFbslVpBe5Iq1br9yU7xYcI5aJrqTCj/VEByT0LGWXNrkscXRXybwbkXDRhsEshvaoWdtqbmCg+zZS/QHk1jo+ubJSM3UcXBd1mnLQBGxtW8TgrQMQdRK58SdsR1NKgdGRpOKudKEw9X8iaTraF8gI4syKxMH/dtwCs3n6j2k7wV3CsYoiZQtevHctYyxFdGZxQtWrBYB";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif