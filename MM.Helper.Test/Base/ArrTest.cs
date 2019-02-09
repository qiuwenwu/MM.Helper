using MM.Helper.Base;
using Serilog;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace MM.Helper.Test.Base
{
    /// <summary>
    /// 数组帮助类
    /// </summary>
    public class ArrTest
    {
        public ArrTest(ITestOutputHelper output)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.TestOutput(output, Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
        }

        public static Arr _Arr { get; set; } = new Arr();

        readonly string[] arr1 = new string[] { "abns", "bbb", "teset" };
        readonly string[] arr2 = new string[] { "ABNS", "bbb", "test" };
        readonly List<string> list1 = new List<string>() { "abns", "bbb", "teset" };
        readonly List<string> list2 = new List<string>() { "ABNS", "bbb", "test" };
        readonly List<TestModel> list1_M = new List<TestModel>() { new TestModel(){ Name = "张三", Passowrd = "asd", Age = 10, Height = 1.75m }, new TestModel() { Name = "李四", Passowrd = "123", Age = 19, Height = 1.65m }, new TestModel() { Name = "大大", Passowrd = "bbbs", Age = 21, Height = 1.85m } };
        readonly List<TestModel> list2_M = new List<TestModel>() { new TestModel() { Name = "张三", Passowrd = "asd" }, new TestModel() { Name = "王五", Passowrd = "123" } };


        #region 数组 
        /// <summary>
        /// 差集
        /// </summary>
        [Fact]
        private void Except()
        {
            var arr = _Arr.Except(arr1, arr2);
            Log.Debug(arr.ToJson());
            Assert.True(arr.Length > 0);

            var list = _Arr.Except(list1, list2);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);

            var list_M = _Arr.Except(list1_M, list2_M);
            Log.Debug(list_M.ToJson());
            Assert.True(list_M.Count > 0);
        }

        /// <summary>
        /// 交集
        /// </summary>
        [Fact]
        private void Intersect()
        {
            var arr = _Arr.Intersect(arr1, arr2);
            Log.Debug(arr.ToJson());
            Assert.True(arr.Length > 0);

            var list = _Arr.Intersect(list1, list2);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 并集
        /// </summary>
        [Fact]
        private void Union()
        {
            var arr = _Arr.Union(arr1, arr2);
            Log.Debug(arr.ToJson());
            Assert.True(arr.Length > 0);

            var list = _Arr.Union(list1, list2);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }
        #endregion


        #region 列表拓展
        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        private void GetStr()
        {
            var list = _Arr.GetStr(list1_M, "Name");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        private void GetInt()
        {
            var list = _Arr.GetInt(list1_M, "Age");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        private void Sum()
        {
            var height = _Arr.Sum(list1_M, "Height");
            Log.Debug(height.ToString());
            Assert.True(height > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        private void GetValues()
        {
            var list = _Arr.GetValues(list1_M, "Passowrd");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        private void Get()
        {
            var list = _Arr.Get(list1_M, "Passowrd", "asd");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值——第一个匹配对象
        /// </summary>
        [Fact]
        public void GetFirst()
        {
            var m = _Arr.GetFirst(list1_M, "Passowrd", "asd");
            Log.Debug(m.ToJson());
            Assert.True(m.Age > 0);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        [Fact]
        public void Set()
        {
            var bl = _Arr.Set(list1_M, "Name", new TestModel() { Name = "张三", Age = 33 });
            Log.Debug(list1_M.ToJson());
            Assert.True(bl);
        }

        /// <summary>
        /// 设置值——第一个匹配对象
        /// </summary>
        [Fact]
        public void SetFirst()
        {
            var bl = _Arr.SetFirst(list1_M, "Name", new TestModel() { Name = "张三", Age = 29 });
            Log.Debug(list1_M.ToJson());
            Assert.True(bl);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        [Fact]
        public void Del()
        {
            var bl = _Arr.Del(list1_M, "Name", "张三");
            Log.Debug(list1_M.ToJson());
            Assert.True(bl);
        }

        /// <summary>
        /// 删除值——第一个匹配对象
        /// </summary>
        [Fact]
        public void DelFirst()
        {
            var bl = _Arr.DelFirst(list1_M, "Name", "张三");
            Log.Debug(list1_M.ToJson());
            Assert.True(bl);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        [Fact]
        public void AddOrSet()
        {
            var count = list1_M.Count;
            _Arr.AddOrSet(list1_M, "Name", new TestModel { Name = "李白", Age = 28 });
            Log.Debug(list1_M.ToJson());
            Assert.True(count < list1_M.Count);
        }

        /// <summary>
        /// 判断值是否已存在
        /// </summary>
        [Fact]
        public void Has()
        {
            var bl = _Arr.Has(list1_M, "Name", "张三");
            Assert.True(bl);
        }

        /// <summary>
        /// 获取前几个集合
        /// </summary>
        [Fact]
        public void Take()
        {
            var list = _Arr.Take(list1_M, 2);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 转字符串
        /// </summary>
        [Fact]
        public void ToStr()
        {
            var ret = _Arr.ToStr(arr1);
            Log.Debug(ret);
            Assert.True(ret != null);
        }

        /// <summary>
        /// 分割数组
        /// </summary>
        [Fact]
        public void Split()
        {
            string str = "a,1,b,3";
            var list = _Arr.Split(str, ",");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);

            var list_M = _Arr.Split(list1_M, 2);
            Log.Debug(list_M.ToJson());
            Assert.True(list_M.Count > 0);
        }

        /// <summary>
        /// 分割数组
        /// </summary>
        [Fact]
        public void ToList()
        {
            var arr = new string[] { "123", "cce" };
            var list = _Arr.ToList(arr);
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 取成员数
        /// </summary>
        [Fact]
        public void Count()
        {
            var count = _Arr.Count(arr1);
            Log.Debug(count.ToString());
            Assert.True(count > 0);
        }
        #endregion
    }

    public class TestModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name     { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Passowrd { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age         { get; set; } = 0;

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height  { get; set; } = 0;
    }
}
