using MM.Helper.Models;
using MM.Helper.Net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MM.Helper.Data
{
    /// <summary>
    /// 验证类
    /// </summary>
    public class Check
    {
        private static Https http = new Https();
        
        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="v">验证模型</param>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns>验证通过返回null，否则返回错误提示</returns>
        public string Param(ParamModel v, string key, object value) {
            if (v.NotEmpty != null)
            {
                if (value == null || value.ToString() == "")
                {
                    return string.Format(v.NotEmpty.Message, key);
                }
            }
            if (value == null)
            {
                return null;
            }
            var err = false;
            var msg = "";
            var type = v.DataType.Format.ToLower();
            try
            {
                switch (type)
                {
                    case "array":
                        var list = value.ToArr();
                        if (v.SubParam != null && v.SubParam.Count > 0 && list.Count > 0)
                        {
                            msg = Param(v.SubParam, list[0].ToDict());
                        }
                        break;
                    case "object":
                        var dict = value.ToDict();
                        if (v.SubParam != null && v.SubParam.Count > 0 && dict.Count > 0)
                        {
                            msg = Param(v.SubParam, dict);
                        }
                        break;
                    case "string":
                        // 验证字符串长度范围
                        var str = value.ToString();
                        var l = v.StrLen;
                        if (l != null)
                        {
                            var len = str.Length;
                            var max = l.Max;
                            var min = l.Min;
                            if (min < 0 && max > 0)
                            {
                                // 当min小于0时，只判断max
                                if (max < len)
                                {
                                    msg = string.Format(l.Message_max, key, max);
                                }
                            }
                            else if (min > 0 && max <= 0) {
                                // 当max小于等于0时，只判断min
                                if (min > len)
                                {
                                    msg = string.Format(l.Message_min, key, min);
                                }
                            }
                            else if (max < len || len < min) {
                                msg = string.Format(l.Message, key, min, max);
                            }
                        }

                        // 验证字符串后缀名
                        var e = v.Extension;
                        if (e != null)
                        {
                            var fs = e.Format.Split('|');
                            str = str.ToLower();
                            var yes = false;
                            foreach (var o in fs) {
                                if (str.EndsWith(o))
                                {
                                    yes = true;
                                }
                            }
                            if (!yes)
                            {
                                msg = string.Format(e.Message, key, e.Format);
                            }
                        }

                        // 正则验证
                        var x = v.Regex;
                        if (x != null)
                        {
                            var text = x.Format;
                            if (!string.IsNullOrEmpty(text))
                            {
                                var rx = new Regex(text);
                                if (!rx.IsMatch(str))
                                {
                                    msg = string.Format(x.Message, key);
                                }
                            }
                        }
                        break;
                    case "int":
                    case "long":
                    case "float":
                    case "decimal":
                    case "double":
                        // 验证数字类型
                        var n = value.ToString();
                        if (type == "int" && !int.TryParse(n, out var n_int))
                        {
                            err = true;
                        }
                        else if (type == "long" && !long.TryParse(n, out var n_long))
                        {
                            err = true;
                        }
                        else if (type == "double" && !double.TryParse(n, out var n_double))
                        {
                            err = true;
                        }
                        else if (type == "float" && !float.TryParse(n, out var n_float))
                        {
                            err = true;
                        }
                        else if (type == "decimal" && !decimal.TryParse(n, out var n_decimal))
                        {
                            err = true;
                        }
                        else
                        {
                            // 验证数值范围
                            var num = value.ToDecimal();
                            var c = v.Range;
                            if (c != null)
                            {
                                var max = c.Max;
                                var min = c.Min;
                                if (min < 0 && max > 0)
                                {
                                    // 当min小于0时，只判断max
                                    if (max < num)
                                    {
                                        msg = string.Format(c.Message_max, key, max);
                                    }
                                }
                                else if (min > 0 && max <= 0)
                                {
                                    // 当max小于等于0时，只判断min
                                    if (min > num)
                                    {
                                        msg = string.Format(c.Message_min, key, min);
                                    }
                                }
                                else if (min > num || num > max)
                                {
                                    msg = string.Format(c.Message, key, min, max);
                                }
                            }
                        }
                        break;
                    case "bool":
                        var blStr = value.ToString().ToLower();
                        if (blStr != "true" && blStr != "false" && blStr != "1" && blStr != "0")
                        {
                            err = true;
                        }
                        break;
                    case "dateTime":
                    case "date":
                    case "time":
                        var d = v.DateTime;
                        if (DateTime.TryParse(value.ToString(), out var time))
                        {
                            var minT = d.Min;
                            var maxT = d.Max;
                            if (string.IsNullOrEmpty(minT) && !string.IsNullOrEmpty(maxT))
                            {
                                // 当min小于0时，只判断max
                                if (maxT.ToTime() < time)
                                {
                                    msg = string.Format(d.Message_max, key, maxT);
                                }
                            }
                            else if (!string.IsNullOrEmpty(minT) && string.IsNullOrEmpty(maxT))
                            {
                                // 当max小于等于0时，只判断min
                                if (minT.ToTime() > time)
                                {
                                    msg = string.Format(d.Message_min, key, minT);
                                }
                            }
                            else if (minT.ToTime() > time || time > maxT.ToTime()) {
                                msg = string.Format(d.Message, key, minT, maxT);
                            }
                        }
                        else
                        {
                            err = true;
                        };
                        break;
                    default:
                        err = true;
                        break;
                }
            }
            catch (Exception)
            {
                err = true;
            }
            if (err)
            {
                return string.Format(v.DataType.Message, key);
            }
            else if(!string.IsNullOrEmpty(msg))
            {
                return msg;
            }

            // 远程验证
            var r = v.Remote;
            if (r != null)
            {
                var url = r.Url;
                if (!string.IsNullOrEmpty(url))
                {
                    var jobj = new JObject
                    {
                        { key, value.ToJson() }
                    };

                    var jsonStr = http.Post(url, jobj.ToString());
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        try
                        {
                            jobj = JObject.Parse(jsonStr);
                            if (jobj.TryGetValue("data", out var kA))
                            {
                                if (kA.HasValues)
                                {
                                    var kB = kA.Value<bool?>("bl");
                                    //参数不正确
                                    if (kB == false)
                                    {
                                        if (jobj.TryGetValue("msg", out var tk))
                                        {
                                            msg = tk.ToString();
                                        }
                                        else
                                        {
                                            msg = string.Format(r.Message, key);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            msg = "序列化远程结果失败！\n" + ex.ToString();
                        }
                    }
                }
            }
            return msg;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        /// <param name="checkDt">验证模型字典</param>
        /// <param name="paramDt">参数字典</param>
        /// <returns>验证通过返回null，否则返回错误提示</returns>
        public string Param(Dictionary<string, ParamModel> checkDt, Dictionary<string, object> paramDt)
        {
            var msg = "";
            foreach (var o in checkDt) {
                var key = o.Key.ToLower();
                var v = o.Value;
                if (paramDt.ContainsKey(key))
                {
                    var val = paramDt[key];

                    // 判断是否相同
                    if (v.Identical != null) {
                        var field = v.Identical.Field;
                        if (!paramDt.ContainsKey(field) || val != paramDt[field])
                        {
                            msg = string.Format(v.Identical.Message, o.Key, field);
                            break;
                        }
                    }

                    // 判断是否不同
                    if (v.Different != null)
                    {
                        var field = v.Different.Field;
                        if (paramDt.ContainsKey(field) && val == paramDt[field])
                        {
                            msg = string.Format(v.Different.Message, o.Key, field);
                            break;
                        }
                    }

                    msg = Param(v, o.Key, val);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        break;
                    }
                }
                else if(v.NotEmpty != null)
                {
                    msg = string.Format(v.NotEmpty.Message, o.Key);
                    break;
                }

                if (v.Filter)
                {
                    paramDt.Remove(key);
                }
            }
            return msg;
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        public ParamModel New() {
            return new ParamModel() { };
        }

        /// <summary>
        /// 验证模型示例
        /// </summary>
        /// <returns>返回验证模型示例</returns>
        public ParamModel Demo() {
            return new ParamModel() {
                Description = "这是一个测试的参数模型",
                CheckPath = "./test.param.json",
                Filter = false,
                DataType = new DataTypeModel() { Format = "string" },
                DateTime = new DateTimeModel(),
                Different = new DifferentModel() { Field = "username" },
                Extension = new ExtensionModel() { Format = "xls|xlsx|csv" },
                Identical = new IdenticalModel() { Field = "password_confirm" },
                NotEmpty = new NotEmptyModel() { },
                Range = new RangeModel() { Min = 0, Max = 100 },
                Remote = new RemoteModel() { Url = "/api/user_check" },
                Regex = new RegexModel() { Format = "[a-zA-Z0-9]+" },
                StrLen = new StrLenModel() { Min = 0, Max = 255 }
            };
        }
    }
}
