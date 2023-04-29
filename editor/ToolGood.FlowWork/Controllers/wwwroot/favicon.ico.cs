#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ToolGood.FlowWork.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("favicon.ico")]
        public IActionResult favicon_ico()
        {
            if (SetResponseHeaders("DD8CE84750A63DA2197B182B140C9342") == false) { return StatusCode(304); }
            const string s = "G70QAMR/RLf/i9CcggMnNCegvL/2Go7nam7Q/u+0+RPyMBLlkSZpGOlnrR5rtmHlxlgShURYHo7dq+oLphfYaikwYi2LcPi+rQV48uKrI1tD+pd0wkRykpNG41NXQ+7qFKmIyFwi0hCRleQYapqmUzb2NO+nKiFKI+qixRNqhWs4C1GLczVvcey9yWYXyWaIZA1paOgL9zeWMoqHxEB9v+Ytos6IZyTNev7v9NtequclW0wkmy0OTWlWsroztj540dP0BCLxNN3eFYy3HaI1yGRi5J/YiXdRlFp0qgUD0ZvaOM7mltxoV1myRSUX659FGk/WkNwqrxvKWMwp0fA0WyvNyUpcpTQnKM0a6tAzWRGZ7s/9v66sNKt7i6PU4kYlzvbEFkoz13Q9b9JKSQZLffKRLCGTSbzwWUOkWSusj5baQi4QcwXiciV+UGJYiREl/lPiLSX2VYeaZ9znLXZU4j0lBs/7M4aU+FGJmwLNItMDuXyoFUQZxfBYs4iMdpfF21iUpqo05yvxrxJeia+VeEmJL5UYU+IzJc5W4oTfEGKNflqJj5QYv/73mQuUxjZdJK/JPDJRLQ6DNtXfgtVrD5YowZlFlfhEiSklnldiJSVmVZrN9rFRJWbm5l+XKvGG0mypNLMGmpWVeECJCSU+8MRyY9W4ZdJCUgsYaOoifFt6VygozdpK/JLX+zxCXdymjEVd1Ko06ypxnxLfKfG9Eo8qzYbqojafnFcX9lHiHyV+98TWqTVxarmUFoeEz8LUQiYtZCJB5C3WH0e5f9XhQHVxhzqIdldEnYnzEbmKJ5ZXYpb9fZVOXjo3rUrspsTfSvziLTacqMZmMon7IX3i5+6J5d59zvBaEElaaROlWVCJ17GnX640if702NL51spY/phFJBuIJNxDm9le+x0THHoDcSZu/d7zxNxBLwb6W/RNTy3E5xXHIeVtu3hXiJSmHIgllHhmrPV7lahry3UZi1byoi5uDQ69StOnDm3ZHJIb72sT/WA2ENdgROI1JZb2NGXvCmaiUpK0f/x9xVg8i9IcaMupQ4/SrKU0xylxtxL/LxHmeE/TFdy1ZkaJudVhN3UcuNwF6rBXcGZBT9Oa9edFaTqV5mgl/uKuQ4tHlOZ4pVnDE33jVeQmk5iP3B6ysjTrz8a6lNiPPzDMITqcB1lnFs/mk1yaxJEnVlXiESX+5IH8qBK/K/Gwt9jEW7Rmc0pOnVkkd/A3XxwNKvGuJw5JLapZf17GqkVMCCcTFDyxmhIfKjGNXXiL4+IVlKbok5IoMbsS98QXPaLEN0p8kSe0n38yOLNwtpOWW8eeWMoTxyvxBJ/sVInPvcWWaRIX00RrYkVpKkpcGKuT35Q4XIn5gkNPauPCVN2HWFGJ7e8lHgoOFwSHJdWZpZXm7Dhs/lSHvdShrVk3MvFsFqbLE7N5i13jjZ9Qi9vUwvlVjBIzlHhOM+gjz9pjDbaRHmJcsx4lTtVX8efBmeVndpXlL5lPgjPL3t8YVZqLnokaPuueRpqcEWBAiduVmFTinXySGNgsSryYR9P7lZhDKx3ik6KnurwvTbcSJ21HfqnE6r4mLTO7ygssr0dsXGku0W/Md8VTY5SmocQtY+XzlhKzqdfqgUiCvyGsXOZXT5ygziwRCOuTOJqqG1GHVmUe1V98xtRcHZxZPjgsEdz4yY0o8bcn9k8tOoKLJE0QjS+bV4n9vYyNB+IansZKcEaCi6Cr8I6XGP8r8YHnSmBFpUG2uOSCM/MrcSe3YcSPir/hTNxjnph3iiUMxq6gxEVKvM0FdZpjnFkvuAi6JKqJBNsu6tCtxF6aAf/4nNyIEo+pM4tmc0guuEKsNGuOE/wyHjCTg2b3K8066gyyeeJVF/cGNGYwhlH9PSV2x6VRbRVfr6OQ/sm49lyooazOLOGJA5S4KZb2fzUI1UeeWlTKRY/Zrh7RHE7XIxNiTnURAmO8zgSH03jvaFCJm73FQd5iBU90jjrxOuw9FDGYhzoktSVpNiSXekifWYwXMMZ7pMHRE9WSZHNKztdltIsbPi5uDdWcNyBMs7oSd+H9+HeVZqnUmtaxSjHfrEXihaiuvI6LJdQgU/0FGe0pS6jlZbKvXZRm4bpmaVSJKz2oYbSz+5RNZ2RmXzE/WZOW8d4umUyKEvprZ9arxBnYiPg4OLPYaFdXHTv7G6pSC4D3496sUf+k82D7DZT4WYn/PXFYalGug2tV20WdKXpiHp+2M2sFZxZQF3X82xuJH0+fuE2JPXHH/eZptkwxiWtpov/lpElXgxSvFYNxsHdRux6hjW2qxKt6Kf+zEi8rsbOn6fG2KJ4oeWIPLO/U795is4lqjL5JQ2ow958ts7GFlHiz/rN+PjizhtLMoTRbHYOmHsF/L/UDDnFmbnVmde7g2vEtfO4tlhvqbm2ZqMbAZ7OIDJW7fXlaXdRbbyN+sVSJz7ULfqk3oz9X4mglDsYZOg9+zQ18U7e2/1eDbkdwkfxa6o2hpyHzGfRt1Z/LBxfNozQX3hMGtUduUIl3vMUOnnFvatHtLbbRDvhPC+lw3W5pcZ1azBPqLTk/R8spBulseZEsExns6Jbp/pa8J+reYmP9d3SFtzjcW6yQJnHHVB0y3V+Ygwg9zaqeOEKJK5Q4qR7gGyGJW9T6bDP+sp8HqbPMl4Q/yawSXCTZQItMJDE8yDZJLcpjlTivhIxVinzW/sjPmIO5vDJPqzWRxMhcJD7BvFDxo5wawkftz8jNOwPH+koymWAOSvK4lbG+kkxU4xX7Jbqu8WilJGkSV6u1f8aG8IXUzWljIHxKPwdXtnLpuVk139oUGUhFysMi5Z9E8G476V8MhfhsepJI7p7GTyIY7nNxoCmyIg==";
            var bytes = UseCompressBytes(s);
            return File(bytes, " image/vnd.microsoft.icon");
        }
        private bool SetResponseHeaders(string etag)
        {
            if (Request.Headers["If-None-Match"] == etag) { return false; }
            Response.Headers["Cache-Control"] = "max-age=315360000";
            Response.Headers["Etag"] = etag;
            Response.Headers["Date"] = DateTime.Now.ToString("r");
            Response.Headers["Expires"] = DateTime.Now.AddYears(100).ToString("r");
            return true;
        }
        private byte[] UseCompressBytes(string s)
        {
            var bytes = Convert.FromBase64String(s);
            var sp = Request.Headers["Accept-Encoding"].ToString().Replace(" ", "").ToLower().Split(',');
            if (sp.Contains("br")) {
                Response.Headers["Content-Encoding"] = "br";
            } else  {
                using (MemoryStream stream = new MemoryStream(bytes)) {
                    using (BrotliStream zStream = new BrotliStream(stream, CompressionMode.Decompress)) {
                        using (var resultStream = new MemoryStream()) {
                            zStream.CopyTo(resultStream);
                            bytes = resultStream.ToArray();
                        }
                    }
                }
                if (sp.Contains("gzip")) {
                    Response.Headers["Content-Encoding"] = "gzip";
                    using (MemoryStream stream = new MemoryStream()) {
                        using (GZipStream zStream = new GZipStream(stream, CompressionMode.Compress)) {
                            zStream.Write(bytes, 0, bytes.Length);
                            zStream.Close();
                        }
                        bytes = stream.ToArray();
                    }
                }
            }
            return bytes;
        }
    }
}
#endif