using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using PhotoKnowEx.Views;
using PhotoKnowLib;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Text;
using System.Web;

namespace PhotoKnowEx.ViewModels {
    public class MainWindowViewModel : ViewModelBase {

        private string tokenText = "24.b6d1eb22ea02855ccf20637a49d26d4b.2592000.1588863078.282335-18368648";
        public string TokenText {
            get => tokenText;
            set => this.RaiseAndSetIfChanged(ref tokenText, value);
        }

        private string request;
        public string Request {
            get => request;
            set => this.RaiseAndSetIfChanged(ref request, value);
        }

        private string response;
        public string Response {
            get => response;
            set => this.RaiseAndSetIfChanged(ref response, value);
        }

        private Bitmap bitmap;
        public Bitmap Bitmap {
            get => bitmap;
            set => this.RaiseAndSetIfChanged(ref bitmap, value);
        }

        private string bitmapString;
        public string BitmapString {
            get => bitmapString;
            set => this.RaiseAndSetIfChanged(ref bitmapString, value);
        }


        public ReactiveCommand<Unit, Unit> GetTokenCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PostTokenCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GetImageCommand { get; set; }
        public ReactiveCommand<Unit, Unit> FruitsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> WineCommand { get; set; }
       

        public MainWindowViewModel() {

            GetTokenCommand = ReactiveCommand.CreateFromTask(async () => {
                var grant_type = "client_credentials";
                var client_id = "Wf9cy0jpfdzsI0F42UMRLusF";
                var client_secret = "KODhBvyeyUTtsoiRvQm3fc6iqcWvTw6z";

                var url = "https://aip.baidubce.com/oauth/2.0/token";
                var content = $"grant_type={grant_type}&client_id={client_id}&client_secret={client_secret}";
                var postUrl = $"{url}?{content}";

                Request += "\n" + content;

                var result = await HttpClientHelper.HttpClientPostAsync(url, content);

                //List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
                //paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                //paraList.Add(new KeyValuePair<string, string>("client_id", client_id));
                //paraList.Add(new KeyValuePair<string, string>("client_secret", client_secret));
                //var result = HttpClientHelper.HttpClientPost(url, new FormUrlEncodedContent(paraList));

                Response += "\n" + result;
            });

            PostTokenCommand = ReactiveCommand.Create(() => {
                if (TokenText != "") {
                    access_token = TokenText;
                }
                else {
                    Console.WriteLine("error");
                }
            }/*, TokenText.WhenAnyValue(item => item, item => item, (item1,item2) => !string.IsNullOrEmpty(item1) && !string.IsNullOrEmpty(item2))*/);

            GetImageCommand = ReactiveCommand.CreateFromTask(async() => {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.AllowMultiple = false;
                dialog.Title = "请选择文件";
                var result = await dialog.ShowAsync(App.Main);
                if(result != null && result.Length > 0) {
                    filePath = result[0];
                    Bitmap = new Bitmap(filePath);
                }
             
                //    img.Source = new Image(new Uri(System.IO.Path.Combine(tempPath, System.IO.Path.GetFileName(filePath)), UriKind.Absolute));

            });

            FruitsCommand = ReactiveCommand.CreateFromTask(async () => {
                if (string.IsNullOrEmpty(access_token)) {
                    Console.WriteLine("accesstoken为空，先授权");
                    return;
                }
                if (string.IsNullOrEmpty(filePath)) {
                    Console.WriteLine("未加载图片");
                    return;
                }
                var url = "https://aip.baidubce.com/rest/2.0/image-classify/v1/classify/ingredient" + "?access_token=" + access_token;

                string base64 = GetFileBase64(filePath);
                var data = $"image={HttpUtility.UrlEncode(base64)}&top_num=5&time_stamp={GetTimeSpan()}";
                var result = await HttpClientHelper.HttpClientPostAsync(url, data);
                Response = result;
            });

            WineCommand = ReactiveCommand.CreateFromTask(async() => {
                if (string.IsNullOrEmpty(access_token)) {
                    Console.WriteLine("accesstoken为空，先授权");
                    return;
                }
                if (string.IsNullOrEmpty(filePath)) {
                    Console.WriteLine("未加载图片");
                    return;
                }
                var url = "https://aip.baidubce.com/rest/2.0/image-classify/v1/redwine" + "?access_token=" + access_token;
                string base64 = GetFileBase64(filePath);
                string str = "image=" + HttpUtility.UrlEncode(base64) + "&time_stamp=" + GetTimeSpan();
                var result = await HttpClientHelper.HttpClientPostAsync(url, str);

                Response = result;
            });
        }

        private string access_token = "";
        private string filePath = "";

        private static string GetFileBase64(string fileName) {
            FileStream filestream = new FileStream(fileName, FileMode.Open);
            byte[] arr = new byte[filestream.Length];
            filestream.Read(arr, 0, (int)filestream.Length);
            string baser64 = Convert.ToBase64String(arr);
            filestream.Close();
            return baser64;
        }

        /// <summary>  
        /// 从文件读取 Stream  
        /// </summary>  
        public Bitmap FileToBitmap(string fileName) {
            // 打开文件  
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]  
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream  
            Stream stream = new MemoryStream(bytes);
            return new Bitmap(stream);
        }

        private static string GetTimeSpan() {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long timeStamp = (long)(DateTime.Now - startTime).TotalSeconds;
            return timeStamp.ToString();
        }
    }
}
