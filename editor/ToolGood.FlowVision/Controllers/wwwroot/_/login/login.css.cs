#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/login/login.css")]
        public IActionResult __login_login_css()
        {
            if (SetResponseHeaders("322122CC75B5BD5B194A8AA6F51CE164") == false) { return StatusCode(304); }
            const string s = "Gz8HAORay3p9mevAgOcEXnudj+8WaqPUIE0JJmwor9Y2ndASIe9xD56IYlk0wu29WCOZJPXpeGQ6qVolYbqGf32YGClY2ruyZJsF/+M50zP07ybSHhMN5mLcJPLEryVJuuClWLzkG/scF/va4/g/36RAV7rStSd6cMbuciuEBJP84j25AGn33DV6cCQoF65fr7+DPUE0gyJ/YYPr12tzXz+sRllQuWQ7xcFcQPDKJ+C/3nataoe9sDgZt179RF5Uoo96LxQ1Yz5Je4U4Y1omiNoiqppFI3pRypO6lupUhBSpsmZyIhVefJ+r3AjU8jbURYver2oZ0/Rg69OucV32M8ynvdC4LnSSsLRJmlcuJHgjsuqALq5obV+ptSzpN4ua4I0CEOdpBRjyoIEWuqOUrmo0WxDchEJaO72csUCitzi8W+ZF0fpEeOmTXF9iwkSDDh8Ag8dIwZtAQAWgxlHcImHhFWEjZzXiWcRmYwAMay5/7UEIlUmHtBo3VFcE8g/woi7fmo7XldU04mUK6PK+V9xw1VfH+u8+BXvRR5e7jqnKtwDHTdjNRjUKGqkZ01MEUH8pz7OJQINXAwiH51V5+VmOplc8J+kSaMRtmGmSgR8Ui3LGb1ZFg06W8kICDQ+JLcBK0w0Sa0NkECsxCeL8+kqAEVC0lFcruAhIX0W8Ds7YnZdMZNBJ8PHI4AY0Es2rWtVrJfkB7l1RzHiClUDDeN01rB2rUB+1qtgsi4j0hKJobRqu1zNNxkPgrbljL5C7srA5hPSwoOOWU3zCETIjerTsTsIRgTX/E47wY5nkO6r2Ts7UFzQUEAvaDZwGHCFKGTJU1YrooVFmL7KM6/O31qwOLsUW6A8zqoPgF/Ybi2o0bspjDw279yvGmLSgo6ibOxnRnxMvolgVGIYhPKDYDDx8ufGJtD6RmvkEDPDVcbKt9vH68O9glumgEbsWTnbQ5P5Q7hDpfCKlT7KvXHsA";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/css");
        }
    }
}
#endif