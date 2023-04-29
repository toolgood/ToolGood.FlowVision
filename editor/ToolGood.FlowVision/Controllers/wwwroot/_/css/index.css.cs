#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/css/index.css")]
        public IActionResult __css_index_css()
        {
            if (SetResponseHeaders("905971564D97ACA8D15983589838CDE0") == false) { return StatusCode(304); }
            const string s = "GyQHAOSyrXp9s+JMKsx4Z+0pevcTa6DZSW2soGEllZ+/nGsc6l/NYdlp2e3GftdYNNbydFvgCSeYYamqTJOQOPhgpWlie+mBaLoL1/UWu55Y7C7kfoKuQvOZ2i+1s3bWzvR76Jp9ldqgOhpV04jZbKbtfWypZVCaQwSFDKUCvWLvVL4BlPYn8a8eb/iBN2Sy8XHgFAa2xhM4aszghA+eckxGvpaxQBhNb5iaAmEbRuoKBKzZjFQg3DE/TD7PMX3wRHdmsPlKsMVgC2uSNT1Pev601BjjK0sEi1m+9ZOyugDuJx+k1fEPw9HvqTrSNf2DlQ31W9gepTv1VYG3NhTp9CsEB+XZfe1387rltLlWuvo8bgo8UPvYY6nlLFeHtyZwQ6VS8QNShcCBQ5aRUkJUxHy5LivKstaePXHGlJtAH7EKqa/rmjhf/LBzSEg2bOkfLwvTdfzQytIuu5zgA1Of1BJjRF8598/U/l6RW26lQeFAZQk4TEqPKBfxQzvje1sW7hJqbOJDjX8WTarOioW65JA9IN5olG5TMftMSGfQP+Eo5BOO/9CJCyQmUP9U5awUEM/UMKtEH9HDSjUYX1Uk/rCXoeL4EnCvM6WqSStmzuP473uEFAxiPtUddhJHFGgi7EA9qWsok6QmSOsBha035YZwNORnQrrQDJb+Mbl44pdYSmu4/2MrVQyI7xlgLXUzVQJJpTHh7IcpAtPUj661OEnmWMCPsgBK0Dy05tVDUdG42YZo9kw7bMzQw4J75jieKBKkSKDnPznsNRUt22kG";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif