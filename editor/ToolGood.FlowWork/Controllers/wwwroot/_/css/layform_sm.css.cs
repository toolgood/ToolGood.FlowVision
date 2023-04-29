#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowWork.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/css/layform_sm.css")]
        public IActionResult __css_layform_sm_css()
        {
            if (SetResponseHeaders("B92380F16FEA66217541F9B7800BA748") == false) { return StatusCode(304); }
            const string s = "G50FAORcqq8vSdpfKOAlC9S8Pp0eh4LnCc4TZWqbIRFqeks0aBwRmngzmYg21eujrYbdcptNHQ3GaHZTKHAU8wPe9+PHUB2L427ChXgY7wfmFvOeuQQ7VR4qHsb4UXRJKynfg7qSStyNvibKcA2g8QSi3/BAxwA6zINrjRm8tfZ+4qvWLUEn4micHQoooDCEfG/4AJZ0jvvL2u/t7EfuCEYeRuain1+xL4zxn6vOD2CpYaMJ7kB71Qaq8NP1o6dn1sPc9OationuSpiPLJD2EL+p3m0bBhmFJ4Cas0Ng65IyJ0QtWbkqoAe+DR34dtM/IpOy4xN4FdEesal8S4K3nIcx2u2HV0hT3IsIl6bp1MeDO0GrzMZjSuLOVI4gJsLL6ry7wBa2sAXmDpbgziSTqEIIQKS9uKI01ZGjAR7DCiKe/yY7eZyxf6lY/O0dwVk6piRI8jFUIi88EoV4scTdUD3vSdczPaPoXdgxBKFSZpmqRDKiqKiUJMSiiFOPF9vsW14te10tmi7QQFNBmODE98fagbXtXaSBYt50nsopPWSLeV9DdfDQu+nz2mbXf/2JFTmiA+zTQ6rVKe5MvgA=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif