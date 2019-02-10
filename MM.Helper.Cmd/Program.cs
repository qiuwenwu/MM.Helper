using MM.Helper.Base;
using MM.Helper.Data;
using System;
using System.Collections.Generic;

namespace MM.Helper.Cmd
{
    class Program
    {
        static readonly string[] arr1 = new string[] { "abns", "bbb", "teset" };
        static readonly string[] arr2 = new string[] { "ABNS", "bbb", "test" };
        static readonly List<string> list1 = new List<string>() { "abns", "bbb", "teset" };
        static readonly List<string> list2 = new List<string>() { "ABNS", "bbb", "test" };
        static readonly List<TestModel> list1_M = new List<TestModel>() { new TestModel() { Name = "张三", Passowrd = "asd", Age = 10, Height = 1.25m }, new TestModel() { Name = "李四", Passowrd = "123", Age = 19, Height = 1.75m } };
        static readonly List<TestModel> list2_M = new List<TestModel>() { new TestModel() { Name = "张三", Passowrd = "asd" }, new TestModel() { Name = "王五", Passowrd = "123" } };

        static Arr _Arr = new Arr();
        
        static void Main(string[] args)
        {
            //var count = _Arr.Sum(list1_M, "Height");
            //Console.WriteLine(count);
            Check_password();
            Console.ReadLine();
        }

        public static void Check_password()
        {
            var help = new Param();
            var dict = help.DemoDict();
            var paramDt = new Dictionary<string, object>()
            {
                { "username", "admin" },
                { "password", "asd123+=" }
            };
            dict["username"].Remote = null;
            var msg = help.Check(dict, paramDt);
            Console.WriteLine(paramDt.ToJson() + msg);
            Console.WriteLine(!string.IsNullOrEmpty(msg));
        }
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

        /// <summary>
        /// 身高
        /// </summary>
        public decimal Height { get; set; } = 0;
    }
}
