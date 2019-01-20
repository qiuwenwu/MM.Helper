using MM.Helper.Base;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System;
using Xunit.Abstractions;

namespace MM.Test.Base
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
        readonly List<TestModel> list1_M = new List<TestModel>() { new TestModel(){ Name = "张三", Passowrd = "asd", Age = 10 }, new TestModel() { Name = "李四", Passowrd = "123", Age = 19 } };
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
        public void GetStr()
        {
            var list = _Arr.GetStr(list1_M, "Name");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        [Fact]
        public void GetInt()
        {
            var list = _Arr.GetStr(list1_M, "Name");
            Log.Debug(list.ToJson());
            Assert.True(list.Count > 0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <returns>返回所有对应键值</returns>
        public decimal Sum<T>(IEnumerable<T> list, string key)
        {
            return list.Sum(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <returns>返回所有对应键值</returns>
        public List<object> GetValues<T>(List<T> list, string key)
        {
            return list.GetValues(key);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="value">判断值</param>
        /// <returns>返回所有对应键值</returns>
        public List<T> Get<T>(IEnumerable<T> list, string key, object value)
        {
            return list.Get(key, value);
        }

        /// <summary>
        /// 获取值——第一个匹配对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="value">判断值</param>
        /// <returns>返回所有对应键值</returns>
        public T GetFirst<T>(IEnumerable<T> list, string key, object value)
        {
            return list.GetFirst(key, value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="m">值</param>
        /// <returns>返回所有对应键值</returns>s
        public bool Set<T>(List<T> list, string key, T m)
        {
            return list.Set(key, m);
        }

        /// <summary>
        /// 设置值——第一个匹配对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="m">值</param>
        /// <returns>返回所有对应键值</returns>
        public bool SetFirst<T>(List<T> list, string key, T m)
        {
            return list.SetFirst(key, m);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="value">判断值</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Del<T>(List<T> list, string key, object value)
        {
            return list.Del(key, value);
        }

        /// <summary>
        /// 删除值——第一个匹配对象
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="value">判断值</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DelFirst<T>(List<T> list, string key, object value)
        {
            return list.DelFirst(key, value);
        }

        /// <summary>
        /// 添加或修改
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="m">值</param>
        /// <returns>成功返回true，失败返回false</returns>
        public void AddOrSet<T>(List<T> list, string key, T m)
        {
            list.AddOrSet(key, m);
        }

        /// <summary>
        /// 判断值是否已存在
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表</param>
        /// <param name="key">对应键</param>
        /// <param name="value">判断值</param>
        /// <returns>存在返回true，不存在返回false</returns>
        public bool Has<T>(IEnumerable<T> list, string key, object value)
        {
            return list.Has(key, value);
        }

        /// <summary>
        /// 拆分数组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">列表或数组</param>
        /// <param name="size">查分大小</param>
        /// <returns>返回二维数组</returns>
        public List<List<T>> Split<T>(IEnumerable<T> list, int size)
        {
            return list.Split(size);
        }

        /// <summary>
        /// 获取前几个集合
        /// </summary>
        /// <param name="list">列表1</param>
        /// <param name="index">索引</param>
        /// <returns>返回交集列表</returns>
        public List<T> Take<T>(IEnumerable<T> list, int index)
        {
            return list.Take(index).ToList();
        }

        /// <summary>
        /// 转字符串
        /// </summary>
        public string ToStr<T>(IEnumerable<T> list, string symbol = ",")
        {
            return list.ToStr(symbol);
        }

        /// <summary>
        /// 分割数组
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="symbol">分隔符</param>
        /// <returns>返回字符串</returns>
        public List<string> Split(string str, string symbol = ",")
        {
            return str.Split(symbol.ToCharArray()).ToList();
        }

        /// <summary>
        /// 分割数组
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>返回字符串</returns>
        public List<T> ToList<T>(IEnumerable<T> list)
        {
            return list.ToList();
        }

        /// <summary>
        /// 取成员数
        /// </summary>
        /// <param name="list">列表</param>
        /// <returns>返回成员数</returns>
        public int Count<T>(IEnumerable<T> list)
        {
            return list.Count();
        }
        #endregion
    }

    public class TestModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Passowrd { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; } = 0;
    }
}
