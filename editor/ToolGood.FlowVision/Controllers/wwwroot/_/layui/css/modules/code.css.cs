#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/layui/css/modules/code.css")]
        public IActionResult __layui_css_modules_code_css()
        {
            if (SetResponseHeaders("54B228F9A553842CD6E65BEFBACE2C5D") == false) { return StatusCode(304); }
            const string s = "G+YDAOSaqtOqsvmM7/WkAWP2DDuGJ0IN1NrE5QD5gcc8X/469iqb24pyTDBPp+7YYfTwmgNKKZeJi78sp7ZJ8kbvszfjyMbadyZYMuN4gkpYjHwa3SkL8SPidr+8fGImWGIlfw5h8bSeN8pS5PcDXhv9QbLAA7RCEDx4N8HUWzAEZAFxS0D1Yo3a0bYsk9EQEhI+VLGG3MTQHvmKm8pb59Chw6bY45wrXr79DfPgaUj+aEVJlbyvdiApQBp3HOOmiAUnYUDIVnAJgOPcL0n0yTg5hADD8VOIElAfFELco5JMzgtNAWJfCZ2l5yn4WJY0/oSqApK8SH58Zj5CUO+OhOSTB9AmoWR/SuPHiY3T3hCb9kjSkvGtblhD2vrunx00BPVYmVjaOc6lxsWBTjFVIXFXFyaK2p61iigKEAwYMIhcnjnuuAsPm8GqetPgmwRFrEsuOA4slG5gBhUnAQMCLgSblXAD";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif