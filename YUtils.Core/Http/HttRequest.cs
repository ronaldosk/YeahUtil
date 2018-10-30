using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace YUtils.Core
{
    public class HttRequest
    {
        public static string proxyIp;
        public static int proxyPort;
        static WebProxy gProxy = null;
        public static string HttpGet(string Url, string postDataStr)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(proxyIp))
                    gProxy = new WebProxy(proxyIp, proxyPort);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : postDataStr));// + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                if(null!= gProxy)
                    request.Proxy = gProxy;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }
            catch(SystemException ex )
            {
                return ex.Message;
            }
        }
        static CookieContainer cookie = new CookieContainer();

        /// request.AllowAutoRedirect = false;设置重定向禁用，你就可以从headers的Location属性中获取重定向地址
        public static string HttpPost(string Url, string postDataStr)
        {
            if (!string.IsNullOrWhiteSpace(proxyIp))
                gProxy = new WebProxy(proxyIp, proxyPort);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            request.CookieContainer = cookie;
            if (null != gProxy)
                request.Proxy = gProxy;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }

    public class EncodeAndDecode
    {
        public static string URLEncode(string str, Encoding encode)
        {
            return HttpUtility.UrlEncode(str, encode);
        }
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string enstring = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                enstring = Convert.ToBase64String(bytes);
            }
            catch
            {
                enstring = source;
            }
            return enstring;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }
    }
}


