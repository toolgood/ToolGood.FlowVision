#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/login/memberLogin.js")]
        public IActionResult __login_memberLogin_js()
        {
            if (SetResponseHeaders("4B80D1D008B75DA8C41E6DE64077E85D") == false) { return StatusCode(304); }
            const string s = "GywLAOQyVXt95bWG2T1BTOlkcKV2RlzJOFMgB4Rkaxik1vr6X0KVl+9/Tn2lqBF9darCfmrZjTSwIkV3l7AcoupU+lXmmdwsGwPsgOTsLFablb9tasDEUwncQCMVSdRj4y/9QB1acvw2Cqmw8hiYpFGMc3fdDWphhRLGxqnbeQu1KKyyCQdWqEaqAyWsanMwj+c74XYx1FQYzzNIoTJ3PlyT8xDpvIwKg1pAf/PucuspEHNFgP+liZqKbmQqSmqeqnzPrY+mMwT8e4VHUNM1kPjH395fbxF6K6oEAhSQsHJx6OmXouWMeCfw887y3BqcS347so0JVIUcYsek7iTJOx/IeEP7x7y/KNCdJHHdyFRsWP28nrfI3DOzplckaSNKfrCpiVrs52w/s9xG+Mdybv8BAXOBwn8mlhOSpX+wkWU5phyr1YHemRPMdVPeja2psBJQn+dUExcb31jmGmR9HmmeqqQZ6x/T7Ku8hTC89ajccG906Eb5Yl+5ZUf/wa7hpc3lkcv9Q6cHhw6QDtG/fKZ8vmvm+fY3zy68eXVieGuNwqKjMw9GJ872b5wrHz9WfgivV68FCjnJuRJNv+ZQdqEp71ilQBlT8g/9l7RhVuG5N+as1VyYg9CehA2/ilFKGS29I31OMOaELRuGs1y97sKc81yl+mgicYQgv1PK4E4xzd5p03RIFpJJ2zAR2LB63VXIN3GuijBuddRmIH+Se50MzW+o/UnufZDGLOXc+Z6HGr+YTcVAfmbSDMFwFKc66YEYm0tCBPz7BYFI5CjkWPMSVkUY7zbSmM2Indxau86TpC2lVTXrqt0CglWOSztoCbO/4vZyNvns9xo/ubbV7J/ZqKbZ4neMhBoa58M+5VYiaw1CbhogdJga3ShBADpIkIA2otb07Xksrz8n+HCksKuAaZilijA+n8yLyJ9QMSM5sXDOHAq1O5YrbqnLAHEBYQSHqom1oKUbJUhtomzZvC6jKUrYWNBZcvhyX7nhwmjtzcHNO4mU6mIt8DjvjPOyx5e45keVtzYOTq/BdYm6oNxW17gWqgGoD5SVQdgdrSz++ed7npWDVYSs6n8+xx/TSdZBFUHprGP/0FGb5Szpsll/pVtf/Ph11OYgnzXFj99OxUEuNYufG2msgerTFiQ+VrrFJjNK26CyuJrkaicUNkDwWUSZMe/x7TlUNFODRgIIlnNCXuZYZOErhVyrkalJ+P/QLggFvaZceO3fzPYyBgGWpy2Qq9Y0Fzaouui/aLpVhXyZtpQGYYcXWTUs6GxCjRQqP+9hDgw//PTBBx/98AO8g0Y2ETomARJGdu7TPA9Ni10USHQQBkeelS8OjD3+wMzz7eWOe+XtjcNH94Yvrs883z5cvbt8tW5wY0u5+sjr1WshBBLKe/M8GFdjgFza/auxyH2aMNyUxhEFmDGgLIqERhJVs0z5n1HFkPl4dqMErf/jt1MxCYNKBYE/AusgOrmNhHClBUDkBJJYSAgrJ9y+3sfgxZ7Bs+PDVyf7Oy+Um56O9h+Zeb6m9cJkZcMBoL1gK4I3SJpbSRRgXeXIAQ==";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif