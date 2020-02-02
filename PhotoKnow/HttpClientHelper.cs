using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PhotoKnow {
    class HttpClientHelper {
        private static HttpClient httpClient;
        public static HttpClient HttpClient {
            get {
                if (httpClient == null) {
                    httpClient = new HttpClient();
                    httpClient.Timeout = TimeOut;
                }
                return httpClient;
            }
        }
        public static readonly TimeSpan TimeOut = new TimeSpan(0, 0, 10);

        public async static Task<string> HttpClientPostAsync(string url, string content, bool isDefult = true) {
            try {
                HttpContent httpContent = new StringContent(content);
                if (!isDefult) {
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                }

                HttpResponseMessage res = await HttpClient.PostAsync(url, httpContent);
                if (res.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public async static Task<string> HttpClientPostAsync(string url, FormUrlEncodedContent content) {
            try {
                //HttpContent httpContent = new StringContent(content);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                HttpResponseMessage res = await HttpClient.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }
        public async static Task<string> HttpClientPostAsync(string url, MultipartFormDataContent content) {
            try {
                //HttpContent httpContent = new StringContent(content);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");

                HttpResponseMessage res = await HttpClient.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public static string HttpClientPost(string url, FormUrlEncodedContent content) {
            try {
                Task<HttpResponseMessage> res = HttpClient.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public static string HttpClientPost(string url, MultipartFormDataContent content) {
            try {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data");

                Task<HttpResponseMessage> res = HttpClient.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public static string HttpClientPost(string url, string content, bool isDefult = true) {
            try {
                HttpContent httpContent = new StringContent(content);
                if (!isDefult) {
                    httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                }

                Task<HttpResponseMessage> res = HttpClient.PostAsync(url, httpContent);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK) {
                    string str = res.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

        public static string HttpClientGet(string url) {
            try {
                var responseString = HttpClient.GetStringAsync(url);
                return responseString.Result;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString());
                return null;
            }
        }

    }
}
