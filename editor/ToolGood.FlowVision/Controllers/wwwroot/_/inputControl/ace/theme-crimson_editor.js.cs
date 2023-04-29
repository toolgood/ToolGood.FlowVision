#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/theme-crimson_editor.js")]
        public IActionResult __inputControl_ace_theme_crimson_editor_js()
        {
            if (SetResponseHeaders("6FCF7F36ADC7DD0772F6A78B7A1CFB1B") == false) { return StatusCode(304); }
            const string s = "G/kKAORas3p9+9wLWtUqYJh0iQspA/Zy1JgKmcl7MrY6owZiJ8RDxJp4ZCQVbzTxRoiMkAjJ/tPvH8t1+V4UIKVL5mqVoSy0rAHFkTIUjc0yN95109D/vUi/IsKf0YkEt0UY0X4cfJgijLqh2bQU4cIHWqfzZuii33jOwQPiHkNyCpmbHim/vnONr0JmpukT7cOdW5kylKJCpq9Bpgz9tZsQyINYzOGCNGnSK0nmLYpiJ0ceWELQ21RWV3DN0lYdyMPpmu4LLIEzduOsSBpc6d1ZSFNH7/qQ1qweJc+4X5E7xJzmNCcowGm9SwwXT1q45Bl5q+O6RGiCTdJl7+oNXDgtbF2lSWfZEO36SiDyW1rK3Fsd8wVH6HEaIy48XZkuHWGdvqbDbvDNMsluCItN1RbEqg8jcGeu4Jf/V7zV8XyOMBMIoqwTWDg1t/3/RqKqEHhVI7DXTKe98of3+msEXjEELiue6yMLsOGkblZuLmYJ9W+weeoANjqwEiFKBEbhEi+HtoETanyIQkgAzihtOVEiXwhMNpBwQrLlpxv4YANqDOEwEiKt06ppovur2QKBC44ghKAdq39AAD78gbW5/HlvdVwuEHghEHi5YEKf4F1vCWGLuYhK4GKui/m6jjj2Y2c1Ai9qBM5mywP/cpvBwPmtGAJnAkFU1Q9EkSio1JTGq4UHsLUr/P3y+neEhcScFi4vtZui1OqSn332XZuOhqzlrETocYF8/M3u4tw/AKRNubCdfAbEu7o5RxC84LAjyAUCjUA5opKWK6bmEGQhZdZE/pZ1iSDEAoGzpBtrr8yaApwysQ1LSPm4BwZswM8qNDzkaXowDa1rcCoWAqHHjaz+lDLBbSltXU+8QRTkkkHOe1Bs7u0MQExG+sWwhIvGNKYxRNajoDJDAIfb9jmCkDlokoSaNJ9zIHwEg55kqoTDOsEYwjuEyhWDJ4LBPp4s7a0B68xdkdI01zfUh/S5Ndih+Vk2vo2jRgW1dJ2ylI+9XWk1UV2i+/Lg7Ycde/nUDlJK+ebj5/+PP1sppXwspZQPH8rvUsqHP6z+3k+5C2sfv//y/nn39V2Z53l+WepX7SHPH+xk98U2PN9xnsO4+uDFh8/VY79+Ya29cydKSmBecEfQuvSwurpVYkTnYaum6U50OFptlQd3p4+jLCuBKpOVy8LhSB9O00fW0eOSpxdCFSVXCQ==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif