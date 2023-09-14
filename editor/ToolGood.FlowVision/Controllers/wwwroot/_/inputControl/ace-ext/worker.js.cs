#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace-ext/worker.js")]
        public IActionResult __inputControl_ace_ext_worker_js()
        {
            if (SetResponseHeaders("A208725EDA43B9888ABD7C360EC12D92") == false) { return StatusCode(304); }
            const string s = "Gz0LAOS0lfX6grVCru2K6ezeoNrzeI3m1ekZOWA6lUDRsss5Bnz8fi6k5cW+1Xgyccz0PvpGEvXQRTxpInTxRqRiIpxNfBbhNRn8YyNrlVOokkYe1PQ3OY8Q2PYVqmgFFTYB/3kkwUzGIOPUxx42niyTnuIZ9e0a1qliIpM0slwMPDdhJT7knifLZQ27q4HlaxS9sm3v2s8/a9G0Bg427zu7xbPsOzvxSwAS6URU39J9pjE9T3ai6OLRE1jTGsCJiG5u4T4G/9Rnii7l+6TRTnDIfaTo0sxDsEecoAiZTwpP1irRRNz7JZxKZLI89+5pw2j2vRY3rkNegig0ja3F3cMCq6HtXgCbuGGVtM5h3+3/Gqet+exjJfRzjP7N+Q/lKuQGP8/WgY+ER+iva9L/Q0r+Euymb9fQmXm9GBy9+k4KFGS0mAsxrpHKBr/4KIvO83pZTNLoh9AiEHrVY6UXd0jrxa1CWMDjjVCRMWKGTTzoQEfndpcZv07KbiO70avHiefQKc4soWMc1rmLmNQPT112fxcsBUov7in4luJP4f91zFYMU+RtasSMEzjEfXxSTW2gRgA5NyISh4bhawNI+n1YxXg6nuFganNo4Xd7RuBp3fDg489qj0BknIFpjTaTsohsDYCr1M0tuMeVpeYXkcnmH/PBtKYzBg7cy6xz32maqSbuYSJGJh4aL/hw5+7aFo2B4UIVXu0y1+nDXXs+1HM3jveRXHrEVmdMXu2675ZJUUnyPAPWat8I9CGuL5WEl+r3GNdozcsan0J0j6kaB2tZqPwwhKum6hXdnsFefQyif49Y8QiVTxWyTaL1aJmsnsmAmUT2dUvdc9KJz5jhR5tdmuhm3PeyXak+Ynbh9bpGTZTdU4VGvFkoV6BJI+j4vIrOkn60E0XLWBMOOHl2u5svoSbjHpNpGjsd/lWhSxceiYKQPZG2gs/T4Hz3JCCdk536gtaNoRuogdbkiGdW8+rHyhwYvMhmaUJbRbjOssGwpR00vhiDtckPfM++CkRf/GgjcfjWax3QNHgwcZmBR4COzKveNo2NpMh0Ovdh4gmxJjOIuzewqZN+0jNtfurcRXRrfFcfC5T9C/MFrPqfjPbhBo2HwwwamrHmDD24ReRw8okEV6UUWL8622oFCt8KIsnV0eM4vpU+cEKl6Pz1Or/ZoknsFn/NpnsrbOmFdXiwCtvgU5Dc0Y1P39xXsRo8dQpssQnaSuyc/Z0gTZQKWYVSYNQnatPYr8VIAaUUfFRwfhlpK73yAj/PYmH9xafwPs7JgIqcBlhKK5ZKoHeFk55JTnqmMfldkDGkDQw0CqPpD63xQ2hnvm/DcxD9GBZWDdGAe9eo9t+LWsTYa6l1vRpA5cAYUSsnBUZ3kpV80wj6D0+BT15JBVtGzWfLDH6eDfLYaQ6LkwKFzhjDwkoD9Ms0KH5Jg6laoBQxGlnN/6UAJbxUWmAbqiSnfiAO50EmccmksZLnOSlTahUecsRI7k8xy+bqebLZ2Xf1pmFg9zHxRfxc4Yu5cW7ow5xC1WHtYV0WLyPwZPVU1OQMK4b6Ypidj5dEwkt6GREDUnVGKfdenmR9kYpOZGcOjElOjPSAaw1K/W0O1NWAK3f6+iY98JKChR0Vh9EMCzgRxBw5GA2GU3ZvnTZO/BIiGjrkEOGlmiwDlFLA6gMnAA==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif