using MM.Helper.Base;
using Serilog;
using System;
using Xunit;
using Xunit.Abstractions;

namespace MM.Helper.Test.Base
{
    /// <summary>
    /// 颜色帮助类
    /// </summary>
    public class ColourTest
    {
        public ColourTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        static Colour _Colour = new Colour();

        /// <summary>
        /// 随机色
        /// </summary>
        [Fact]
        public void Rand()
        {
            // 色系 亮色/ 暗色（light / dark） 红色 / 绿色 / 蓝色（red / green / blue）
            string colour = "red";
            var ret = _Colour.Rand(colour);
            Log.Debug(ret);
            Assert.True(ret != null);
        }

        /// <summary>
        /// RGB转16进制色值
        /// </summary>
        [Fact]
        public void ToHx16()
        {
            int Red = 255;
            int Green = 0;
            int Blue = 0;
            var ret = _Colour.ToHx16(Red, Green, Blue);
            Log.Debug(ret);
            Assert.True(ret != null);
        }

        /// <summary>
        /// 16进制颜色值转RBG
        /// </summary>
        [Fact]
        public void ToRGB() {
            string value = "#eeefff";
            var ret = _Colour.ToRGB(value);
            Log.Debug(ret.ToJson());
            Assert.True(ret != null);
        }
    }
}
