#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-statusbar.js")]
        public IActionResult __inputControl_ace_ext_statusbar_js()
        {
            if (SetResponseHeaders("07E8CF23F590E679E7ECD90508BFAD86") == false) { return StatusCode(304); }
            const string s = "GzcEAGSzZb6+rHbbShOtsv2WcXt/e/9ugRUwlxnjXEMYuhw43LAfYzXWsNLoB0hZH/BH4bQojCRcOqe3McuOAVVQOJG45bKyNxuC1ra3S94vWfNxqTQB/4LsgfOAwS5zTHkBhjH642DAqP1DqPY+jpT+QacG/nDewT2o3HBN60lT4Z0iiLxh4ooKwBHiQ2mzrZY7Wi87j7MaNjlvWBr4cAJiuVW59GLGZx3Ngdb2zybpMZyeHhMTIN5d8nUwqZflp12ygzr868VWRIG7ZVua87NUnjQVwVXibdCr+Q86DHl70Ij24+w12w99JrClTaowjWURSVYJZFw3HYeBXz49UdlKnBDqVqcGvgsciB/sUYvtz/d2raIm/67O4RTyFTjQVqq1uO4ird+vKnoMPNManj+PMh+XFgPPtxvcgLaTpiK6X3/KHlvp7fo+TD5MjTSWtX3ST7tkbIm4lTqOo05+kWR1TD5MzfPnPcL3Tx9gAWuDa2Ux5nLu3CCDqS/DAe8GCcuncc5XJD/SRtdOhCGPfNepMaSyR0DY4Sg2eUnx/DjKuftzkuKZdvCKj0sXUFODpDNoBwQMBdDWYycpnmFEhwDcEA/yQwjKH+JxylZEfsGOru/gzziHcZQ5zuibuWyXnD110kXpYpgQgLaNpPYaSsLYJ25kz9Pfa3KHjTZw6Rr/KDwT8BdE71IIZtIaDpivs8VD0Y86B7HqrM7w/DkTRfhSh/I8rThSpEZ122gjpBI=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif