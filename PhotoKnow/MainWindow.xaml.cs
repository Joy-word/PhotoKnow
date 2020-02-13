using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoKnow {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        /// <summary>
        /// 拉授权，使用HttpClientPostAsync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnGetToken_Click(object sender, RoutedEventArgs e) {
            var grant_type = "client_credentials";
            var client_id = "******";
            var client_secret = "******";

            var url = "https://aip.baidubce.com/oauth/2.0/token";
            var content = $"grant_type={grant_type}&client_id={client_id}&client_secret={client_secret}";
            var postUrl = $"{url}?{content}";

            tbRequest.Text += "\n" + content;

            var result = await HttpClientHelper.HttpClientPostAsync(url, content);

            //List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            //paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            //paraList.Add(new KeyValuePair<string, string>("client_id", client_id));
            //paraList.Add(new KeyValuePair<string, string>("client_secret", client_secret));
            //var result = HttpClientHelper.HttpClientPost(url, new FormUrlEncodedContent(paraList));

            tbResponse.Text += "\n" + result;
        }

     
        private string access_token = "";
        private string filePath = "";
        private void btnPostToken_Click(object sender, RoutedEventArgs e) {
            //24.3f37a194aa1008f75bc336bb5fa271f1.2592000.1583164863.282335-18368648
            if (tbToken.Text!= "") {
                access_token = tbToken.Text;
            }
            else {
                MessageBox.Show("error");
            }
        }

        private void btnGetImage_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择文件";
            dialog.Filter = "图片文件(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            if (dialog.ShowDialog() == true) {
                filePath = dialog.FileName;
                var tempPath = System.IO.Path.GetTempPath();
                File.Copy(filePath, System.IO.Path.Combine(tempPath, System.IO.Path.GetFileName(filePath)),true);
                img.Source = new BitmapImage(new Uri(System.IO.Path.Combine(tempPath, System.IO.Path.GetFileName(filePath)), UriKind.Absolute));
            }
        }


        /// <summary>
        /// 使用HttpClientPost
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFruits_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(access_token)) {
                MessageBox.Show("accesstoken为空，先授权");
                return;
            }
            if (string.IsNullOrEmpty(filePath)) {
                MessageBox.Show("未加载图片");
                return;
            }
            var url = "https://aip.baidubce.com/rest/2.0/image-classify/v1/classify/ingredient" + "?access_token=" + access_token;

            string base64 = GetFileBase64(filePath);
            var data = $"image={HttpUtility.UrlEncode(base64)}&top_num=5&time_stamp={GetTimeSpan()}";
            var result = HttpClientHelper.HttpClientPost(url, data);
            tbResponse.Text = result;
        }

        /// <summary>
        /// 使用HttpRequestHelper.HttpPostAsync
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnWine_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(access_token)) {
                MessageBox.Show("accesstoken为空，先授权");
                return;
            }
            if (string.IsNullOrEmpty(filePath)) {
                MessageBox.Show("未加载图片");
                return;
            }
            var url = "https://aip.baidubce.com/rest/2.0/image-classify/v1/redwine" + "?access_token=" + access_token;
            string base64 = GetFileBase64(filePath);
            string str = "image=" + HttpUtility.UrlEncode(base64) + "&time_stamp=" + GetTimeSpan();
            var result =await HttpRequestHelper.HttpPostAsync(url, str);

            tbResponse.Text = result;
        }

        private static string GetFileBase64(string fileName) {
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }

        private static string GetTimeSpan() {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds;
            return timeStamp.ToString();
        }
    }
}
