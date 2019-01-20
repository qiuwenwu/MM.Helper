using System;
using System.Text;

namespace MM.Helper.Base
{
    /// <summary>
    /// 编码帮助类
    /// </summary>
    public class Encode
    {
        #region web类
        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string UrlEncode(string str)
        {
            return str.UrlEncode();
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string UrlDecode(string str)
        {
            return str.UrlDecode();
        }

        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string HtmlEncode(string str)
        {
            return str.HtmlEncode();
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string HtmlDecode(string str)
        {
            return str.HtmlDecode();
        }

        /// <summary>
        /// Utf8编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string Utf8Encode(string str)
        {
            return str.Utf8Encode();
        }

        /// <summary>
        /// Utf8解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string Utf8Decode(string str)
        {
            return str.Utf8Decode();
        }
        #endregion


        #region 文件类
        /// <summary>
        /// Unicode编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string UnicodeEncode(string str)
        {
            return str.UnicodeEncode();
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string UnicodeDecode(string str)
        {
            return str.UnicodeDecode();
        }

        /// <summary>
        /// ASCII编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string AsciiEncode(string str)
        {
            return str.AsciiEncode();
        }

        /// <summary>
        /// ASCII解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string AsciiDecode(string str)
        {
            return str.AsciiDecode();
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str">被编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public string Base64Encode(string str)
        {
            return str.Base64Encode();
        }

        /// <summary>
        ///  Base64解码
        /// </summary>
        /// <param name="str">被解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public string Base64Decode(string str)
        {
            return str.Base64Decode();
        }
        #endregion

        /// <summary>
        /// 转换编码方式
        /// </summary>
        /// <param name="str">被转码的字符串</param>
        /// <param name="to_encoding">转换后的编码方式</param>
        /// <param name="from_encoding">当前的编码方式</param>
        /// <returns>转码后的字符串</returns>
        public string ToEncode(string str, string to_encoding = "gb2312", string from_encoding = "utf8")
        {
            return str.ToEncode(to_encoding, from_encoding);
        }

        /// <summary>
        /// 转换编码方式
        /// </summary>
        /// <param name="str">被转码的字符串</param>
        /// <param name="to_encoding">转换后的编码方式</param>
        /// <param name="from_encoding">当前的编码方式</param>
        /// <returns>转码后的字符串</returns>
        public string ToEncode(string str, Encoding to_encoding, Encoding from_encoding)
        {
            return str.ToEncode(to_encoding, from_encoding);
        }
    }
}
