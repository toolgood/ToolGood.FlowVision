#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-hardwrap.js")]
        public IActionResult __inputControl_ace_ext_hardwrap_js()
        {
            if (SetResponseHeaders("76AF19B0A619DD6ADDA137A0FDB66009") == false) { return StatusCode(304); }
            const string s = "G9QGAOTc2/7/67cSQ5jbkepd3tLKW7ZNPQmvHZSzQDOAS2qv1ta/ZGIktr9FVJNrM2mQqLTPSBKT1KH9UBqRZTo7k4+SOHJ55rCy40gJ21ND19z0Ph7MdhS4E40HDhMo6DpGzkmgeIlDeSaBSGkr5IcwvT9/eKT2MTh/FAdsf3COCphhEiVRlTL7PosuWlLxi34Oex6DHibv1EKFBiiXbzPAdLZcRRN0evY9qTVmwHQ/LAM60zyoj1u1TzXc1f73GoIkodF0pV4lQDYNEJYcv9cAy8eYCFjMGsmsm8mcpFjKBe9eH6RUxeQV7zYHqUYhmQzopFy4/3v3uyomA5LJtQMOAizw8dQP/PC2IIWhpbllKUln8eRUZR95MDiJ3wDAVvPuNeuBc+ukdLvXh49V4UWP5SIRxMdVard7gzgG2nP0Q7We51RmO5msO/JPCvd70EfKowUuUGJkH/KPlo8+fA4TKgAvJuvJyn76I/GRFmaNbIYR9nnxQWXNe9Vv8YJZUxh+ixfA/kKFvT5CJuhEKfkYsKBDixHHjrem77y/shjSR8o/+ECKAYsyAz86gTPPquAJ3zkgQVnTPOzTKzeHFtitD0h5Pj7QpfKK0cnxN8joNIUBUOyDqC0A9nXd0XOiyjt1kbLZ/97oTCmrAlLywvRSqihQW70BjMDSCMlTn0xhTqLZp3rZoBCAZxOjKA/7VC+6mVSLStRndOZZ3bSydk5KcfkfNR3S3u83TwZtTx+r0ahFgwlArjcYP9LVGT4bDLF2RFEJQF6tsF+toMXmbS3bKCWNYVpAcaDbPwVg5rqe59yqKSoo/ysZPZbOOxV0H19ebBh0sC9kjPAhEeeU2YejsLBv0JaPCfxtySboMiQIvck6lZP5dBclUvSJ9ctbkzVTGIiJ9fLOgWwr7godPHkR6GUJQ5xr7UrWy2eb8hf0nG3HKmeZIb/FS+s1xwsmTziRluL72C42M3RlQblKSrCXXiw//RmG+DXHMiqY57PlypughNaFZyLo32w4EroYZAazAP1lgNNdoTyVgC6uUU61J+V0uS9Fgb0Wp5Pl4W+2Yzslym2xSqACTOHjfSMAFxEkHYMS22DQ2ZdX6gUWaGHqOfeQzzgF4uR2sZln6LLm12J4hsK+BqaNJoAapLnaifolo26IhynfRoqu+o80RsT/H6nPQspIqIAyfgheI6VC5Gkw7wAzzAoA";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif