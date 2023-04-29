#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowWork.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/css/modules/code.css")]
        public IActionResult __layui_css_modules_code_css()
        {
            if (SetResponseHeaders("2F53EEDD3FB05E2F5ED9177DE08A7C52") == false) { return StatusCode(304); }
            const string s = "G7cGAOScmtP6ZXs9cemQ9D3DDvhQkM8BNbv/z+UA4EhO/K8bz6MAi9beeBOoIRBTtnS46OjUGHh4Pjj0aq7//AsjXu7I16il98QP+l+aHqT3e1BlJvOr0dNp1rWdXc4XL1+IND2QScPs4EGgkcMpJBX+Qfk8mqV2uaRscbVgW9Q20uiCAHAngiCoQLDP1HZhWcQorrjiEj8qioJl+erHRKfBXX5hvux5vQHNihwcBB0Q9rAl0IVLlorhqUztwqiEQQqaN38rmVEhvmLe9D7qgAsTw14C0wFCNOFgLE0Tty8a0YjmdBN45z8FAirBuhj+eNIwH5sbfM4ZF5Gokl82XcVjMBjr3lG7QXlgpKPKiuGBPF0XXI1ChRTT/XsOOeQMtQ/EhxWBhNUC7UHqkSNB4Iy4soEz7DGzrM6mVSq3WKq084G8bb8qKkuZSK14l4hqD5+9o6bIEQssRBgs1+owO4mlq6pSJWsBraX9404NiA9drIO9lzSnPNnuuRt2cVmkMpWplAz3DkeFKlShsMmMIrEtcwEdwBVThB2EZEnEdpGaQRU7f05D4ssIkoMvXAXVAKK3onXWqlaZuofRXBr3xwd8HER4ClIRFkhWrUZsShQYNisZqoCWQtiDwAXZZ4QF2poCGCc=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif