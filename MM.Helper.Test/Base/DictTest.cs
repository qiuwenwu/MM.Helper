using MM.Helper.Base;
using Serilog;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MM.Helper.Test.Base
{
    /// <summary>
    /// 字典帮助类
    /// </summary>
    public class DictTest
    {
        public DictTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        static Dict _Dict = new Dict();

        readonly Dictionary<string, TestModel> dict1 = new Dictionary<string, TestModel>() {
            { "张三", new TestModel() { Age = 100 } },
            { "李四", new TestModel() { Age = 99 } }
        };
        readonly Dictionary<string, TestModel> dict2 = new Dictionary<string, TestModel>() {
            { "张三", new TestModel() { Age = 21 } },
            { "王五", new TestModel() { Age = 23 } }
        };

        /// <summary>
        /// 左合并
        /// </summary>
        [Fact]
        public void Left()
        {
            var dict = _Dict.Left(dict1, dict2);
            Log.Debug(dict.ToJson());
            Assert.True(dict.Count > 2);
        }

        /// <summary>
        /// 获取键列表
        /// </summary>
        [Fact]
        public void GetKeys() {
            var list = _Dict.GetKeys(dict1);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 1);
        }

        /// <summary>
        /// 右合并
        /// </summary>
        [Fact]
        public void Right()
        {
            var dict = _Dict.Right(dict1, dict2);
            Log.Debug(dict.ToJson());
            Assert.True(dict.Count > 1);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <typeparam name="T">返回</typeparam>
        /// <param name="dict1">字典</param>
        /// <param name="key">键</param>
        /// <param name="m">值</param>
        /// <returns>存在返回true，不存在返回false</returns>
        [Fact]
        public void AddOrSet()
        {
            _Dict.AddOrSet(dict1, "张三", new TestModel() { Age = 66 });
            Log.Debug(dict1.ToJson());
            Assert.True(dict1.Count > 1);
        }
    }
}
