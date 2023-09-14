#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layer/skin/default/loading-1.gif")]
        public IActionResult __layer_skin_default_loading_1_gif()
        {
            if (SetResponseHeaders("1140BC5C7863F8E54A3C2B179E640758") == false) { return StatusCode(304); }
            const string s = "G7wCAGRBTtNJwAv4lWTLA4qh7jnU7pII8nx3S2yAA4JhoD6YG+JzLJb1HeqaahKSjqzACoVIMLq8gcat9///DwCM/9h6qibGyooGqkJ8AsgIAGD8REHFAiTgAQBgBVYApOyS/M7lW3zGCgYr1etdpmdSgb0mxUlNI8/NGr42NkrsIQ4hOmHb7QifDUuffjyDCMa0nOKDm2YXswKDep53whGzzRmjFVzpsBXSwUuZZtWr0GhfuezzCkHaiFPfusiPv0tDt0QRpg+FUueFtNkF8bMKteaPZHJM48XQznRMtzXONNV19qEO9aisKAMAxk8UVMcnAkACFMAGQELNye9ctrFPUkAFrHSItIQ/kWJ0cycz0MiVL20ljF/wsIsRdbqSAyHgAHIPQyXSEeISp8pJLxzaHK8+L9c5Y+M7HjXEvhMCoDAJ1JYWGCJQr7klLnGonMD+gaYoJGCAKQ0QipV9RIRLAw==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "image/gif");
        }
    }
}
#endif