using MM.Helper.Models;
using System.IO;

namespace MM.Helper
{
    /// <summary>
    /// 缓存对象
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// 运行路径
        /// </summary>
        public static string runPath = Directory.GetCurrentDirectory() + "\\";
        /// <summary>
        /// 运行路径
        /// </summary>
        public string RunPath { get { return runPath; } set { runPath = value; _Path = new PathModel(runPath); } }

        /// <summary>
        /// 路径模型
        /// </summary>
        internal static PathModel _Path = new PathModel(runPath);
        /// <summary>
        /// 路径模型
        /// </summary>
        public PathModel Path { get { return _Path; } }

        /// <summary>
        /// 模板主题风格
        /// </summary>
        internal static string _Theme = "default";
        /// <summary>
        /// 模板主题风格
        /// </summary>
        public string Theme { get { return _Theme; } set { if (!string.IsNullOrEmpty(value)) { _Theme = value; } } }
    }
}
