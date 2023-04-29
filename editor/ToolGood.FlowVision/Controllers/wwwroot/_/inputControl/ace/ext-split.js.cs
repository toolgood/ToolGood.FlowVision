#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-split.js")]
        public IActionResult __inputControl_ace_ext_split_js()
        {
            if (SetResponseHeaders("54EC539C992B7DC7A151994C082D68D5") == false) { return StatusCode(304); }
            const string s = "GzwOAGQz1Xp9FY5HQ6yxjPONgq3lmr2t0BJk81YGNSBcUnS01nqgSW1jyC0imkSbzdxjao34lvBO6kTRkBrL4LQGxTPYYWXvZO9iy+dlHJI5/OYuNfA8h45PY1YrDt0ud/uBHVKfP6T1ec4jZP0QZUMQng8s9ot3yYwVJ9f4TwAZYe7iX8rSsdJF+1W4lJTF/cCLL7zQCxrcun3hqpim1lxziFopiXfh3KJ5gAUI3/MA9+8+3+MhrE7Re/6VhT0t2VkI4bnBWcgi/N+LIHwEu/98GIdZiLyjKG8JWYCJXFqHCW5Xct/hyfN3f3+ii+at1fL1s+e0eAw7a7NYTMJKArWjMmeT3UKqpRYW9ODCp8slOQeuLvTtB53OtuPI1KKUysK2FNvGp7u7CxDttQEZQIqKy7eF1HVZvNuNgP4uycg/YX+zTGGdpPO2TQVgai7ZWHqroWYBmlzcK69TjsZiNNexDlEry8GdXJcODlDqufEW1kF52F9xxxSqSdBGy7hJW8qKT0buOjH6VaV4g+XxctEMPJRfm8vFeHJoJKeRLVZRDFBI+Fj1c5wtZm8skIBbA6AEjh4gl0pYJwpEj4RRyj4zjPuy9QIoobC9yGLLdMPkxWHWgzYDlCmFVTydPP5Ck3ovVxdgW81Ht9pyVRtiRutXbfiAXlpmi+pBtThzbO4srEh3GbhNvZcHYLUbBgP8t3fFx47YcVIm1jXZQgFLsQnlm7IR/EiD1IBbPqCFx3iw1Hze2Jsl4KXJj4N65itMPBSuvB3BA1emuH/xbPaJICZz791PVZmvbjV7mFI5jg23iwMq5YrFt4ANJu+cAuPkh5pLP92rsrD7aaOag6yDWsLQW0IkyfbAsM31sNeH2vWw14fKwraqFk22Rh4uKOf1WZ/Hdlvt1BrcGvN6LzAJ/M7f8vU6R+1eRekGVtF+YowSMkcbPC002SWCxiqZVup2na8DeS8sA0V1AHOlbGg728zaIQs3XFYBVr2RhI9V9hI2bB3AQg8IC/I+d+wBMNHDug/S5WkO56rvK0GZNFiXymnRKq7NGRc2AAcEoT4UXubeVnFdiAyQCTv8fWA9ajI5CsRE9U+U45+hbkiSAWp1lZ80jv44uZV7Q7gv6x/WJwSxur/8pHF8l3bJ/ouyYS9hdvw+KuySIOb2eAK0MOvz0D2LFknSxL0ANnhAm3JsgJTKQqvVSxJq1F+TyIhkPwwPrdx5lxAu5IvZD2zlD4aSd5zFdTKi7jBEMgFqXfsOhc1A53oBwGQrNFdUHNZv2P7ujugSb+ss3WVJnQpqYGYpAZrt0mckde1RWSRVL1ehXEQRpsRiA5myd0/qaX3wcjrYUwR/xWmzNRRMqNjGDmoWesSOnbD20Gf1ifbnIrFZugiNFo1dgesbm88h4joB+0nDRx0bHrvoapTmbjw5kmuwPJJbvFzfwL2R3ePnMaZzqJfPwUxu2sL/qs/qlXpv4r1flt1TZ+G7W+/0FatGmyYIbRwGn8Lhq0poLHSiDlCaoDn0tFp8skNQSO1zCGcyTM/ZKUiAQyEuqFgAE3QazZ5OkNbJ/DeDjJr2cGvXI+e++ghL5PL6f27N1TWSdep3NL2orr0PyqxmMMHkAQ==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif