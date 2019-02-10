using MM.Helper.Data;
using MM.Helper.Models;
using Serilog;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MM.Helper.Test.Data
{
    public class CheckTest
    {
        public CheckTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        public static Param help = new Param();
        readonly Dictionary<string, ParamModel> dict = help.DemoDict();

        /// <summary>
        /// 验证参数1
        /// </summary>
        [Fact]
        public void Check_all() {
            var paramDt = new Dictionary<string, object>()
            {
                { "username", "admin" },
                { "password", "asd123" },
                { "password_confirm", "asd123" },
                { "phone", "15817188815" },
                { "email", "573242395@qq.com" },
                { "icon", "/static/img/573242395.png" },
                { "age", 29 },
                { "birthday", "1991-04-01" }
            };
            dict["username"].Remote = null;
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数1
        /// </summary>
        [Fact]
        public void Check_username()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "username", "a" }
            };
        
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数1
        /// </summary>
        [Fact]
        public void Check_password()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "username", "admin" },
                { "password", "asd123+=" }
            };
            dict["username"].Remote = null;
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数4
        /// </summary>
        [Fact]
        public void Check_password_confirm()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "password", "asd123" },
                { "password_confirm", "asd1234" },
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数5
        /// </summary>
        [Fact]
        public void Check_passwordUsername()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "username", "admin" },
                { "password", "admin" }
            };
            dict["username"].Remote = null;
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数6
        /// </summary>
        [Fact]
        public void Check_phone()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "phone", "1581718881" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数7
        /// </summary>
        [Fact]
        public void Check_email()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "email", "158171888" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数8
        /// </summary>
        [Fact]
        public void Check_emailFormat()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "email", "@qq.com" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数8
        /// </summary>
        [Fact]
        public void Check_emailExtension()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "email", "1581855@33.com" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数9
        /// </summary>
        [Fact]
        public void Check_age()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "age", "29" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(string.IsNullOrEmpty(msg));
        }


        /// <summary>
        /// 验证参数10
        /// </summary>
        [Fact]
        public void Check_age_max()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "age", 199 }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数11
        /// </summary>
        [Fact]
        public void Check_birthday()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "birthday", "1991-04-01 00:00:00" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数12
        /// </summary>
        [Fact]
        public void Check_birthday_min()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "birthday", "1940-01-01 00:00:00" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数14
        /// </summary>
        [Fact]
        public void Check_icon()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "icon", "http://www.elins.cn" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数15
        /// </summary>
        [Fact]
        public void Check_icon_true()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "icon", "http://www.elins.cn/static/img/test.png" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数15
        /// </summary>
        [Fact]
        public void Check_age_more()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "age", "0|33|69|190" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(!string.IsNullOrEmpty(msg));
        }

        /// <summary>
        /// 验证参数15
        /// </summary>
        [Fact]
        public void Check_age_more_true()
        {
            var paramDt = new Dictionary<string, object>()
            {
                { "age", "16|33|69|130" }
            };
            dict.Remove("username");
            var msg = help.Check(dict, paramDt);
            Log.Debug(paramDt.ToJson() + msg);
            Assert.True(string.IsNullOrEmpty(msg));
        }
    }
}
