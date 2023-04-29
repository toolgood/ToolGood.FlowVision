#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-whitespace.js")]
        public IActionResult __inputControl_ace_ext_whitespace_js()
        {
            if (SetResponseHeaders("71FEEE2050953884A04BEFEBC39A7643") == false) { return StatusCode(304); }
            const string s = "G7MLAOSyZfb6EmqiUoe2gN0b3OM975nshckWxsJogyVKanwEmFpr019CLWtvaqi1l4aIps+IWolqibkEqIyWb1OpQxuYnlRcrOzetYybVqf6xum1N6zD2LQ6xir+84HtxRjr2+g8hxjjsztOg45RbL7BHNKhsae4xj81+CVpkWGOp6CjwN60HJeXxkeGrIyVuvwUlKwejpp1y9/bo7bcsHGWVClgViuN7p/ROS+1lc6QyZa87f44Zc2SE7JgEuoqrpclS3GlWeX+Fj1qnLDFjqoaA1U1DpRhQxleaIG4/rOx0iq9kZjr9wGZspK3l5KTBEwnDdmKa0zf7MOr6s2rZL/ZhzpVrANLA9A5L02V1UTxnuOdHJIEG9r8Mp0P2qufP//7vz8//+n111BIT0adG257mb6JXqVQZbUatO0iXghT+fqFnuMLITX5TYMvpH7MYFmal3pZ/Eu9LDJUuqb3lliWDJIcsKt8TfKAPzt7Qx5K3l6EMJWJTrrJnbTu4xJi0iZJ3UXOkYJa99QsWnYmas6d2IRXzAAdzaF1XhcZKt1Wka3YU1Z2zkumvORt/l6rnXGiu2RAJsp3sqcJJ+qqvN6pTwv1CXbftSyLnCgDKKaUegwV10LIKaFQcZ2OgNOjU0aNQkj7zEnJ69/gCCUrkpnlMVcfCCFbctHvGnB47JMcdlK2RPmy9Nsh/WBZbHi3ufoEhGSTLYy5joBz2xfxnmOb07QrFP3jkOQ7av3o8haKizPHKFtLVom9g0vjI09WnTT/ZKwOMsu4nIZqIll6WJZ5zeqzq7YXwqqg+XXQT67j5+YQpFFtTxRHMWBnI85Sz83hybzTMgeoA2jWkhV7c372jRmMPT2NTauphH2XxkfBUYzyM0vAnoI6af58GLyPBWyJhVC5ra/PI993m7zI0FBVo6dNXne+AR12OXImIVi91Xr8cvLB+d9cMGycFUJaFcCxntrvBpP+0k2WdyTbTybwR/mD6pz/umn7nJWh9/+bJvIVJ3ldTkJM6j+djK+8uxLZz5ReFqPGKfSS4IQViuty5nUnzV6vJAHQUwaoyVS+LthZlXdXnCjDgfp821M5bYdyShJw1FdTjR05tcjNbch0H5KHFHAi0kLIbqsGL1HjsVaI7rEVQnZE8AP6JDEq2F28xSYHFGgYFj473/f2J2O1nLDDrG+3lqzWS7c6GVmu0XiHrPzUc3N4Ym/sSUKV1agpxzXvtASc8I4jdqVfFulJQ8nLIpkOACT6t8QI6OXCqKCMan/oUTcsGT2gI6uOrsWBXC4VbWhe8ULzmivjTZThnYYGpd1jTCdHGu4Uo0JR2T5cybCLptiVrHo4ab6e/qn1Wtu/zJF72bHyAuT/t25wzstrqgF7ur7U2FJThXpZZFOFmkKg8lsMAGWb0KXq62WRl6rn+9Y4irEHKNsX1AkhXecIM+CLBnTK2PAIGGJn767FhJmQt8hWbAHWGm7cQ1n5PL4NriWrh/UJht3X+Of+BDpxTPOKBknkKY9c0RZ2rNq+24xFGig6IeQNABUF2faO3h+TFNCwQPDjkm3t95aHv/EwzwCQnarUHTQ2z2x3fB+16yJLFLdj9ngXiENaKHhHoFjfWACwxUBhi3nN/e8+nxt7DFTNtjnrIoZXQ4zHBQMVGdk4W8RfTUfSInMljFb3uNwybydAfdNtEVFVaA/ArGQIxllYV7ReOeBWdox79uYcMVODaBZWmdmu0MCM3+We0tHcVFH9lT3wvoSHimFPmV2klLIOB6963L1NSwMgU96nJK/aHj0mEf/Kb80rsVyZb5lP2q9acggbg6MkaJWKdYR9UZZ+KCWMFYpL6IfUfutarzC9zYB5LzqgyXFOWU2eK8yH5TCnO5w6z0oUu8P/uuVYCCKaXVd4gPw6IaSgQLFFWlhhlQA=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif