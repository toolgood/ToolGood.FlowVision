#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-themelist.js")]
        public IActionResult __inputControl_ace_ext_themelist_js()
        {
            if (SetResponseHeaders("815D50BFF92216F6CCB2AC330607A860") == false) { return StatusCode(304); }
            const string s = "G/YFAGQ1NV9fgTeN6uar68aahmHINpOwlsiUUsYaOKfgCEZy9A7Kpm+WSc/C3Y7nTtMd8OZ0NCgIVv0hySVpJVynIjz1s7IbcB1cjVs4Tl9jw3mKCWwOqwe+BAs47oumCBaCNAOPUNilBc+ts03ZDAYRTUxKdYI/aTPUlng8uXcB92YL7Wt7eQ4nPZWAUNgcTrwMmrgtVwpR2Jw1lERb+tSN+EIrujBCN0Qj7Kz21I9hQBeULgcV4urRu5ioVhx7Ee+UptiYW+r2Egj+iuN055Iz4lWCqMroSH+eSGMq3wxUKNWyG5emyHfk6x7w8svTrXlBHaKChfjfR9nts2g6ChU5rhEsuDPVOG2jYE96TiJYqB+SUf8ySuaOGo4XslBrdcpgjvTjqbp64B1YiJ+Synlvk2A2b7vQwbCSMVjoNvL9B0Q2wubYu7oFC12p+n2EGo/mnLiLGsGqgLJj7sKZVl97GBAstFom0YyA3qFWNJR6EgeZAaQyL9JJyEAZlST4hLAY5tHX5RRLSVqqQ6R1BDRI5n6/cC/amFOnLVhg0aZsbvPAog+MmN7CuK95dDGhN8LGau8re5fCcFJJGmO64kYfGfKKGogHJ0oiCWKs3aC5rw3TBCJfFgsNfLY59gP0CZSVhZ2jsqYapqw0Y5+f29FzPlK3lwirJ/s6Ip+HJJpDn3qnSh0nc8UtWBj+Okri96YuNoPrr5X5hrPZ0KmhPc53ivmc8+1iCli8Zba6FkrImPKJzOrerHb9RMK/Od8uLGPZfzOjnrxe2IIfZCmeOm1/c75b7O2NHWfZBfxNi0mVhv205FTsqdVFtsgosyqb/RMDRoatt54TMKWhs1ma9FE65lq7twdS/cM6weqq0cjQlGbAh1dX1x1dm7BKzhbZYj0D";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif