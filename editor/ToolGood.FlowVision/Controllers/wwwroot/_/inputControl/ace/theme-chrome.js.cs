#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/theme-chrome.js")]
        public IActionResult __inputControl_ace_theme_chrome_js()
        {
            if (SetResponseHeaders("E28D16CB55D12000FF675BEAE3B58D97") == false) { return StatusCode(304); }
            const string s = "G5UKAORaqr2+e/IVa2YlASrtGldaR2JDGKt4EG5J7s/91JtqUo6EfPlJAUHhSFGSEULqOoeOhdxNqAlVPIRbATho1f+4X/UUVy3bg9aB6qk2tteS/tzNIwX4M8hA8LMAAzpuZ+eXAINx1ruBAnzTG4PtEj2PwW/Ma/AH6wl9eOljuzxSbnPnGm993C/Lw0Ety51A9RShQM+p+0RHf+dWXBvEqqe/Zuc9OVBPf1hRRx11rUY3TNP0jx0bQAOV3y7c/od359bZyUd9ovfCZZnuD2iAb48tggsqqqgCMcnlN6UQrJ5cYELovkRy3gh4eaUrLAcuXzrXTp0z9A207e9Mt+Y1R7gxaYih8zwtXk2eRZzPg4XSalWFUAoEkRUrbAz8pVzqe23nlDtNm6FA4DlD4FnI+qFqsFrGIIP+eYrAEFio78sO59YTMIGzedBwiYRU0YdeJxknh8ZHYAhlgcCXDAwsMM506Dwyu6v705bwVXRHLWUn7YFlaWgsZzUCFxxBCLHFsQtgULmWxsHoBBT8aaCmdgpLL2ax1ZNHdcmfQOu8odNhdnpBKhOYcKankRkC5xUCT/MJzoBEmKjkSqQFiezV8M4d1nP/LsC5fa8MPtqOSTNwVW1vYoa4ZcFyGbPvamhWI/BUIPCs3l5wHIdou0ICPALLENy9C03e+hOdbjWqlG/VYivGGJOC1Ieck9J2MmirFgglgsiZiw120RZouPoqRygQeGbupDm5XV00qBM5jVuXdI5xcQl75SqOIHi6HQziJp62wEIiF3AgTozAeTPVb4hqC0WGIESNwBmf9O6c6jfk4TJReaKBiG+PwIDdmaCtZB3SmC6wzIPVbpKtBcKNEaC63qr3dk/RYCeSgUXZpyhDYDErzygRnUoECUXcNgANrHSve92rA5QrQjo6zE4rOC9yhnBjKXkoDJdgboVePSjtQoufnTJRUwpFg4TTC0fyKnZ9EHwzZCUCzxG4qNClnJ0MgvOODB3dGRnYkRBY+GbKA3XikktZ0A4MYRzuUblS90STGmOI2Kqus4zV6rzLTpomH405A8cc/svODetAK68aOypDyXYybacWKjK0Xx68/XBgL5+aWUop33z8fP74s5FSysdSSvnwofwupXz4w3Tfp2d+hQ2P3395/3z8+i5LkiQ5y7pXwylJHhzk+MVonhw4T2C8/+DFh8/5Y7d5YYy5cycIoY6cMdCa54+jU/v/VrtXDuydaR3E8afEXNjauKqa1MNl+ShF2/pT4Bj8nBgM/4c=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif