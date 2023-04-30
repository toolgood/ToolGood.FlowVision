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
            if (SetResponseHeaders("25A8F5FAB72AA379D1F9E92AE104EF58") == false) { return StatusCode(304); }
            const string s = "G+UOAOSyWfb6LnKSamUN3GVYLienMCVDM/QYWlxLTCwuxfwL8fe52bOnLg0o6pmQRnBLeqdWKpX+tVGempWOlR6oAeePMv5CL5RCmReJynDeBSlUyORnttEWYEYtexpIYSk0fNlsAsqErjrwBkk6aFRrNLDoHAefC4htYikHlZH6rsJRhRPjIdztyr6BI9xIYQsYta6EUbs82LoShBaj8Qip60PhKLBrfHdipl7cr5g7Pg88apcHv+VAEIZMuT+pXbauhFuQfbiRwQbIozjTjabeM2026VmF46CbSwyOc3FEqPXM8hbmwnsXlBgkKc2611WGZgLhEHiPjqR6VAMxXKcyNB7C4RraIOnjWk4eFYytdzQeNQIOiivx/GFy7Ws3vwsJ7unbc8A+jRJdOQqHcAVpcpmes+1ugNrO0PXsbyZ7roEyjRI91ImEu3AG1/fB1Qb5MHSURrFGKoEClIdcFY6x7dIo0R0jhbstT5DSKNG+dvNbXFMew3ICIen1JdtJIlkJ0Uzs2dcXAWdwFxKkUaJtH9yhgl3TiPe62ym4owjzNIrzesQN1mOxv761i7uQwKdJHA9C3HeodvP7MvyYjBKVoWGgEhg4FEgOPYi4XF+hL9dXq4xzdbGy8N5HPBxj2+mtmSgxn6X375d+1jxKdG8XuMYN1K0BFmFU1G4dB+caL5Su8vV01GJd1PFpcSNAs+7VGitJRkWanEOZvVhGubCtW9a9pL5p9KkzVZMcA1aSq3Btmed5HAxFHM0WMD4Py7nj0pvWdjJnGFuvWHdIS6SUzBeVLqyBalIy19s12RZSMgEWQU8xLz6/tIVgUzItBDsMSg2D7o0tS1HYgWWlVPuUo7bpQe6VYo/YDsrMbCO900vYkI6kKGpLE0CrWrWOCU6QokgpwyFk4oK7CwlkrIbSLp56oxyLe2rVvvE4k8f/mStWELeshwi7DncygIBOt0gOc3IVHmRuYqB1tiZJgdZpBS9gPPX5qXWxGkVMINxkg/VCB1gEN7VUokDdLi689Ovm5bcuvvvE5revRoTTJQZNJKC2/k5YHqDLLXLz8mO7b/6yeeVFSkGbZ77YfetHvx/vvvPz5pWvNj++s/ngG5K4POE7dst2h5mDfunZquTpUPb79Nbmkb+IUfvpn////skBzI7ZAw7EyYtvvHvh55//+/fT3Ud/JgK9F0wM+gwApxX28Xg2N56eZwbYFTuChjxbsvIF3cXzji5vvZoJ8XYOktXkGOXRM8tbZIJde6VYfBPaa3ZOjz5bhTI1osCBwnspZp8myuMKhCbVmjNYBqU5xzGs33TYSXHOlUuhzEikuBIWardwdPFGqTQXC9G0bhIe4xiWoc4ZOmBsAeNuuBOFMmCLWvJtnVneYgwDXSPSSnabHoJeinSscGKlWuUSojjDSnIeTl8hlUn5NUggFGjpNe8k0X5fIbLKsQy5EBrzOMMdznB7W/WE9PCgROlolO6BUHJNQMRX798vQ96rrMovieNtkuKSWOigMppIcIadav/+ZIDsgUCHF7raFt1CqEENWZ+BGilkeYFUQoskjoUC";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif