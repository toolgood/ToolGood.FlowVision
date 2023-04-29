#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/css/layform_sm.css")]
        public IActionResult __css_layform_sm_css()
        {
            if (SetResponseHeaders("7EB401BA56CA1F9F1C94E95DBC8FDE3F") == false) { return StatusCode(304); }
            const string s = "G5sFAOScqdOK7WQz0X1poHhaUr8cMl+p35Dn/1xAA6VfNAvaG24WbZEXFZb5/GancO/YzrYaPSKIf+iqjmvcw+b6+DHWh5dUzso7aqf7gbnF/Gemw93VBmtqp/BSdJlFAO9JXUlA7Xy9JhHnGkDyBKLfUOuOEZldRtlxNwQhxPXE1J30lsXqSM6JOeaYc0K+N3RE4Vim9pe334vFTFQ6nKidSIhBfsU+O4V/rjo/onADG03UDrR/2dAlgnT96O6Zzbi0A3+qYqK7EvwlC2Q8JOiq/7MNw5zCBuDK+WNi6+KyJiQGpNwVXAEtIfAx/SNSgIFPEFRMd6S2Nh0H3nJqp+B2EF0xSdTeQ/gkSUwft/JEFqUuHotA7STKAMQ8ePms7C4QuchFDrmLxGonQABUiABItB9WVKUGJ92ID7GCAOe/KU5BLWp8qVj46R0jWDpUJKjxNio4sSDK8GKpdk70vCfWLO6ZRP+2TqAInWX83CPSUQ3YCwUsjjr1eLEtpqO137tq0WR7hsYH0gQN3x/pRtJ1d5UGF8qm8wRM+qgWs77G+qB2kPPndciu//oT+nHMANinkdWzRO0EXg==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif