#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace-ext/mirror.js")]
        public IActionResult __inputControl_ace_ext_mirror_js()
        {
            if (SetResponseHeaders("4452F70552F6CF00D00EB75D646CE59D") == false) { return StatusCode(304); }
            const string s = "G8EDAGQ1TV/fi7JIhGDXUNX93Uux6CtRHXWg5WwPXVPwjd9f8YK+axU0/G9aFOh4NBxXUKTPfK+5FuOuteAXHLGczYPwp2UfFgh5kGunzf+LX9uoe/NAP0IHwXWBgpxtm/cpUNi0MlcJJJU6Jj98j5Q2zBuxfu+qenytZluHX9SIcBmMOl6eZF+NySAw64giv8+2FlIK1qYgl5lMTseqO5Hm1PmVv13JyhReHpH+1CRWxJPFk+yrOeEvlTYkk9OVQghI4/WVjOIu5dO25C7JuUjN51Je5FpBVGpGb/KxWkEXEamrDUbjZhCGf6GkYwXwmCbjknsmJ4k6gv64/ou3hP8Bzpy323r+UmrPEyhGqZOsxubg6Xr0R9p6czL6UbqJkp6553PW6XkH6A8/uvELn1xuI7/iYVCbxHsgA2sf6g//RVVNpofcxPIwcxRFsiNWoId8+y8I7VzHqCN03uu6kTZ3/NkAlvthMgxq7DO2EHDBJTYxgtwg9na6c/16zJbcP+LzM4klVFvGnM+5zgJuaQOHBYYiOQlOu51xh0b91NagoBhemKA03NIGsQogmSpUMqmIZLy0skynt2JFbU2HkzqgF2RKpgm4LOiDCzMveQMXBA==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif