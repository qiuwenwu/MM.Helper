using System;

namespace MM.Helper.Net
{
    /// <summary>
    /// 接口请求
    /// </summary>
    public class Api : Https
    {
        /// <summary>
        /// 接口请求主机地址
        /// </summary>
        public string Host { get; set; } = "http://localhost:8001/api/";

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="path">请求路径</param>
        /// <param name="param">请求参数</param>
        /// <returns>返回请求结果</returns>
        public T PostApi<T>(string path, object param)
        {
            string pm = null;
            if (param != null)
            {
                if (param is string)
                {
                    pm = (string)param;
                }
                else
                {
                    pm = param.ToJson();
                }
            }
            var html = Post(path, pm);
            if (string.IsNullOrEmpty(html))
            {
                return default(T);
            }
            else
            {
                return html.Loads<T>();
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="param">参数</param>
        /// <returns>返回请求结果</returns>
        public T GetApi<T>(string path, object param = null)
        {
            var html = "";
            if (param == null)
            {
                html = Get(path);
            }
            else
            {
                string query = "";
                var type = param.GetType();
                if (type.Name == "String")
                {
                    query = (string)param;
                }
                else
                {
                    foreach (var o in type.GetProperties())
                    {
                        var value = o.GetValue(param);
                        if (value != null)
                        {
                            var val = value.ToString();
                            if (val != "")
                            {
                                var key = o.Name;
                                // Console.WriteLine(key + " = " + val);
                                query += string.Format("&{0}={1}", key, UrlEncode(val));
                            }
                        }
                    }
                    if (query.StartsWith("&"))
                    {
                        query = query.Substring(1);
                    }
                }
                html = Get(path + "?" + query);
            }
            if (string.IsNullOrEmpty(html))
            {
                return default(T);
            }
            else
            {
                return html.Loads<T>();
            }
        }

        /// <summary>
        /// post通用接口
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="param">参数</param>
        /// <returns>返回响应结果</returns>
        public ResModel HttpPost(string path, object param)
        {
            var ret = PostApi<ResModel>(Host + path, param);
            if (ret == null)
            {
                ret = new ResModel() { Error = 10000, Msg = "服务器连接失败" };
            }
            return ret;
        }

        /// <summary>
        /// get通用接口
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="param">参数</param>
        /// <returns>返回响应结果</returns>
        public ResModel HttpGet(string path, object param = null)
        {
            var ret = GetApi<ResModel>(Host + path, param);
            if (ret == null)
            {
                ret = new ResModel() { Error = 10000, Msg = "服务器连接失败" };
            }
            return ret;
        }
    }

    /// <summary>
    /// 响应模型
    /// </summary>
    public class ResModel {
        /// <summary>
        /// 请求序号
        /// </summary>
        public int ID { get; set; }
        
        /// <summary>
        /// 错误提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 响应结果
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Error { get; set; }
    }
}

