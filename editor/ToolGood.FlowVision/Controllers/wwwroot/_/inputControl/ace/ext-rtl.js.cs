#if RELEASE
using Microsoft.AspNetCore.Mvc;
using System;
namespace ToolGood.FlowVision.Controllers
{
    public partial class WwwRootController : Controller
    {
        [HttpGet("_/inputControl/ace/ext-rtl.js")]
        public IActionResult __inputControl_ace_ext_rtl_js()
        {
            if (SetResponseHeaders("6E3C357E2C939FF2C97E2EFF8F9095CD") == false) { return StatusCode(304); }
            const string s = "G1YJAGQ103p9G6IwJERh4689mvvI9DenalOJdhUqoJgLHs//mnt9dTV5DWKaJLn8fxORdG/JJG1Acw2FTOhErqLPoXb3AyqCskvJCoeVfQEZCAPd0UXvRDO4H+A38DRwQJdDEa3gYF/GYyZw9LYxaRFA4lA4pi38cu4GNxu2Exwr3VSVNCj0yBfdJMNO7XQKcpO84pb0u1Tew1jMFMZesVKtqTDePqQxvQ08ZhJMdaP5Y2IyCaWcbdMkHBH9gbH3ft00Ju1jPpUTfaSoXw4kQVPhpslm8OP1Ey6O3JcTvThKLXJfCutZtS73ni0pG8ZVa71jGtsusxNSfbU/6NXYJ6wevvRtl7CSfi01VZqWk8jRWFtrZ2F5i2E7Mco6M7U7yd9ti5ZMUf+Fh6M9/MBDGP6vhzDQv3+h+L/nImOmqHeoVNUwDmW/Dzwihz3xUeCoMMZhEyd9iibxw4o+uLdNOXs+5uxIAAUqGUeIz1CjijEK3iu8h8SVRKFpGHNi8pzZ/ri1sYgRnzqql0eMxONj63Jpk3awk2VrcdgFeaZmbRfec3P45uOrpjEJxzLgbVnTfsiy/UVaYJkxqJ8cbci5Ipv+5KW869zRK94qXfRjuJLsLseBcq4uesUcriQvOpAQIw0UWMFZMSc/u4TDBfojxiKvwrDzCuGzfizqGR1/EKI/j/apaVKnwTHqiVH0NJN56O0nKypd9FlOW/YQ6lDulJ8hPtouFdIWoEYD2FlEjmjYTr2+RGUV4gnKHtjy5/ZXhEUp79OGJRmFWvDoizDsCEXL52ryPyYOe+ogU9T7sqnvGz0kHj/QtZvOiTt4oZJXz7Kuvu9S1NVHcPswdPCih8DYdbOjCw2dOkpg2+VfSTeaX1b2eYbdorWzEwrjF87XbrGeHXs0Jcx1Xz5SnJG2oeKG0Dr+lyueDSBafZ3FV2aJ32MY5l9u8cmE6XFoMgWflRQOUmPgJtF8TxftpkrKY80TM1ULsCpsYNgF3hK4o3Vwlq02gEsEEUI8kpBQAlP1gLeBGsApFcO80fPalC40gBO0IYyjnn2qIWs7hprFuAIQqR/ai3HVWowruqkGL3gXmYX0W9vDvgIeD2NQen3M2dh5dqJZ0rRckjcG3q2NNnrulaGvpOpUGFCcGdyiZZVVcGqCT6XFxRM/T2Pyi7WYSxeZZbDeqNnO9kVInZ2mdwLj1Uyf+QEvcxBernjr9UAl3hyP9R7Kw380KDQNkvE6CDrQ6fVNYxIKkcyd7WxnYwE=";
            var bytes = UseCompressBytes(s);
            return File(bytes, "text/javascript");
        }
    }
}
#endif