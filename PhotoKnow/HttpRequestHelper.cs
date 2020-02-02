using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhotoKnow {
    class HttpRequestHelper {

        //ContentType种类：
        //text/html ： HTML格式
        //text/plain ：纯文本格式
        //text/xml ：  XML格式
        //image/gif ：gif图片格式
        //image/jpeg ：jpg图片格式
        //image/png：png图片格式
        //----------------------------------
        //application/xhtml+xml ：XHTML格式
        //application/xml     ： XML数据格式
        //application/atom+xml  ：Atom XML聚合格式
        //application/json    ： JSON数据格式
        //application/pdf       ：pdf格式
        //application/msword  ： Word文档格式
        //application/octet-stream ： 二进制流数据（如常见的文件下载）
        //application/x-www-form-urlencoded ： <form encType =””>中默认的encType，form表单数据被编码为key/value格式发送到服务器（表单默认的提交数据的格式）

        //json格式：{data:"xxx"}
        //formdata格式：data1=xxx&data2=yyy

        private  static readonly int timeOut = 10 * 1000;
        public static string HttpPost(string url,string data, string contentType = "", CookieContainer cookieContainer = null) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (string.IsNullOrEmpty(contentType)) {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            //request.ContentType = "application/json";
            request.CookieContainer = cookieContainer;
            request.Timeout = timeOut;
            request.KeepAlive = true;

            Encoding encoding = Encoding.Default;
            byte[] postData = encoding.GetBytes(data);
            request.ContentLength = postData.Length;

            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(postData, 0, postData.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();


            byte[] utf8bytes = System.Text.Encoding.Default.GetBytes(retString);
            retString = System.Text.Encoding.UTF8.GetString(utf8bytes);
            return retString;
        }

        public static async Task<string> HttpPostAsync(string url, string data, string contentType = "", CookieContainer cookieContainer = null) {
            return await Task.Run(() => HttpPost(url, data, contentType, cookieContainer));
        }

        public static string HttpGet(string Url, string data) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (data == "" ? "" : "?") + data);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
    }
}
