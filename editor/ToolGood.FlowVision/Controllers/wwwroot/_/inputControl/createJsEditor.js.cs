#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/createJsEditor.js")]
        public IActionResult __inputControl_createJsEditor_js()
        {
            if (SetResponseHeaders("EE9FD88F9E778927D143B822215AEAAA") == false) { return StatusCode(304); }
            const string s = "G1QHAOSyWfn6Drqkruli4JJttDinS1ELDfQapLEkZjbhFPMvxN/HrO/+fm69kyolvrc/zBtJVJtYW0cs0ayJV9EWGImjQgkcKsv12pkipaSITTP2Cp4sanwUrfmbSg7WrV7sxw0G2FpoN+FcFxRTyUEaQIc9VorjmsIR8p6tkRB7CnfZkqTQBc06W9Js/paFAhTHnsLRhjpTmesXTp0VjjtvzYXVARCjZMaVPya3fGOXd7Khu/tulZzPogQra8IR3qQsPYxLp+fXU6MXbHvnbzJ6taUyi1Jc0kLSnbyg6/pg54D4fbYmixJkU5IJVB6xVTjh9DyLUpw7NuEu7Wo2WZSib+zyZtuWJ7isKfgsSrF0unYkJyEa1r2vKwIv6E42lEUp6j7YI4WzbYvwXbeZYI8zLbMowU6v38mGfJYmCXZsFNRXDlh0HGns8l72dskoBcWxI1OSI2cKiFOvZxzCw3gIrwLlcvkIlXnfY55OOD0XFFLLgBth63O99lxMaMB2xPB8jknRCcq6AVQfZMjcJyhzI62HWbC77UKwtvUClC7owgO0sbSu9BfWfH5mq9BMvZSJmsKNOmgvMNB6UDtRgsCOgs7EpZd+HV9+6/K7T4y/fbUiCIYYkEhQo/0dtHFClRVyfPmxnTd/GV95kVLI+MwXO2/9qMd6552fx1e+Gn98Z/zgG5JJNYXjuu3pTNRyPZE51MtkK5+nw9l34GR85C9i3H765/+/f3KSc9Z5DxSeJy+/8e6ln3/+799Pdx79mQhqp4gBz6kq39qIWxIpanwW31kjo8MetriSJhCp4zqCTwzxbi6jXMTzFVu9NH3b4plzs0Y5B66km4Wsbp7niTHKYrbg0BF3ei75I8k7jafaxMiuRqM7ykwcaD3gRvoSBRDIASbuKOhhABgG1ce6LAEwkJNVkoNyeOduKXaJaYB4oVsJkMSttEaKotGmJoG41cKWTbggRSVARP7g2gZ7JxuSCQxqod3EJLCM3VIcLALd1StnZ79tAuKGMe8qvJdiZy1iPG+SQAPeHSwDoMt5fa7LjlopVm25ISBeZYVECqF2wiunDZCAblBJvnKtiecdJ7gMDU/goa5YF3QhxxgqICZdNNKobaHdxIUYyAo0lcwJe7CXIlsHjQpAaklRoriSLjdnqNiUTgW0bEhAEDGgW0mtfV8hVGWdDLkQyHmieMUpnk6hJ2SgByWjowXs6aqvDg4cvXevDHkPqsr3J8nUSLE/ERhAMU8BZ1yp9u71D2APeo8LrKZivi5ggEGVQ2qkwHKaBIEiTZJEAA==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif