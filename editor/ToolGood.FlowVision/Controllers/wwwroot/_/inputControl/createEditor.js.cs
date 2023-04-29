#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/createEditor.js")]
        public IActionResult __inputControl_createEditor_js()
        {
            if (SetResponseHeaders("D57A3A07E2C5CA72DB70FC4F6A1B4184") == false) { return StatusCode(304); }
            const string s = "G1QMAOSyWfb6LnKSatsauMuwXE5OYUqGZugxSFxLTCwuxfwL8fe52bOnv1eBop4LaQTfyN60xqS5OjbFOmxsnFSgBZx+ynmFKvSv1qtIj9E541we6rHWFKgHLXsUSmFsaPiyyQRUCGg68AxpIQCpNWlcdI6Dzz2Iq7nUg8pEfVThbEUT7THc7cq+wSPcSGEKHLWuxFG7PNi6EgWI0XhEtutD4Wxg12C3oqdeXM+ZOz6PPGqXB98VIBBSZlzv1S5bV+ItxD7cyGgC5lFsdKupR2ZNJj2nAC66uaTgOPeOGLWeGd6i3PNehCUFaRUw9FBlpCcYDqH35KxUt3roDRepjLTHcLjGliR7WMvpo4Kp9c6OB42ASXEjnm9Mrn3t5neRxXv69hyyT6MEKmfDIVphmlwGczbdDVibGbme/c3WnGuwTKMEep1IuYtmeH0fXGuQVyNn0ygGsiXagOUhV4VjbLo0SqBjsuFuwxOyaZSAr938FteUx6icYFA9X7KZKMkriBHFHnx9EWiGd5HFNErA9MEdKtg1jfdedLsN7ijhPI1iWw+4wXgqthe3ZnEXWfRpEseDJ+46VLv5fQbfJaNEZaQZbYmMTAXUoRsRl8MVcDlcrTLOg4uT0fsf8XiMTRdu7URJ9Bm9f7/E2fMogT5e4BY3MGxNuAijpHblODjXeKGgytfjUYvDoolPkxsBwNCrNVXSRirK5ARK76UyykXcumLdS9s3DZw60zTJNVAluQnXhnmex2SIcTVT4Pg8LueOS69b00nLcN56zrojuySbWv1GA4WJoLrU6vPVYE2LqdUBFwHGmBcfH9tiMKnVLQYzDEoNA/TalKVXOJFlpYL2PkdN06PcK8UesR2UnplGorNT2JLOSlHUxk4wt2pUa05wgBIlSkUcQiYuuLvIoozVkNqlUx8pSXHNbNpHjiN1/Je5YgHzLfshS12X7+TIBAzQkgX8kqvyg9wlBlrmK0lKtMxKeCHjqc9PrZPVKmKC4SYTjBcQcBFgWqlEgrpaXHjp183Lb11894nNb18NCKdPDCAkpDb+TlzuIOQKuXn5sd03f9m88qKksM0zX+y+9SPu07vv/Lx55avNj+9sPvhGJE5P+Iph+a7QOIjLNquWl0Pb74KtzSN/CaP30z////2TC5kdOw8AiJMX33j3ws8///fvp7uP/iwEov+ZGOBMBpyV2Mlxb288Pf7c2e7BM8NbtkrVt1eKueWgv2bn9Oi1XShdk6fvKbyXYmpV4jytUIBVpcpiGRRwTgO0vuywk+KcK5dC6WH28GUoNG7R0LnNUgFn/JJpnfHsOkZlqKt4wEF7tClwXGU+VCiNpqglqX1meIujDKj8ySv5UWAf8lKkA+GSKlVypkRxRpXknM5YkS1VBTVkUag4YgTeSdi+XiGyyrEMuRBAeZzRDme0va16QUZ8UJLv6BT0cmXXBAVP3r9fhrxXWZVfEsfbVopLYgFBZcYjjzPtVPv3g4HfA9zyAqpt0S2EGtSQfRBrkMIvT5FKgEjiWCg=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif