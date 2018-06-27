using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;
using System.Configuration;
using System.Timers;
using System.Drawing;
using System.Globalization;

namespace TwitterImageBot
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("tr-TR");
            Timer _timer = new Timer();
            _timer.Interval = Convert.ToDouble(ConfigurationSettings.AppSettings["TimeInterval"]);
            _timer.Elapsed += new ElapsedEventHandler(timeElapsed);
            _timer.Start();
        }

        private void timeElapsed(object sender, ElapsedEventArgs args)
        {
            SendTweet();
        }

        public static void SendTweet()
        {
            GetPixelImageFile();

            string key = ConfigurationSettings.AppSettings.Get("twitterConsumerKey");
            string secret = ConfigurationSettings.AppSettings.Get("twitterConsumerSecret");
            string token = ConfigurationSettings.AppSettings.Get("twitterAccessToken");
            string tokenSecret = ConfigurationSettings.AppSettings.Get("twitterAccessTokenSecret");

            string message = "";


            var service = new TweetSharp.TwitterService(key, secret);
            service.AuthenticateWith(token, tokenSecret);
            using (var stream = new FileStream(@"C:\Images\Pixel.png", FileMode.Open))
            {
                var result = service.SendTweetWithMedia(new SendTweetWithMediaOptions
                {
                    Status = message,
                    Images = new Dictionary<string, Stream> { { "john", stream } }
                });
            }
        }

        private static void GetPixelImageFile()
        {
            byte R, G, B;
            Bitmap image = new Bitmap(@"C:\Images\RandomImage.png");
            Random change = new Random();
            int squareSize = 20;
            for (int i = 0; i < image.Width / squareSize; i++)
            {
                for (int j = 0; j < image.Height / squareSize; j++)
                {
                    R = Convert.ToByte(change.Next(0, 255));
                    G = Convert.ToByte(change.Next(0, 255));
                    B = Convert.ToByte(change.Next(0, 255));
                    Color color = new Color();
                    color = Color.FromArgb(R, G, B);
                    for (int k = 0; k < squareSize; k++)
                    {
                        for (int t = 0; t < squareSize; t++)
                        {
                            image.SetPixel((i * squareSize) + k, (j * squareSize) + t, color);
                        }
                    }
                }
            }
            image.Save(@"C:\Images\Pixel.png");
        }
        protected override void OnStop()
        {
        }
    }
}
