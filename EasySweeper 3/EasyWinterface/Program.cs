using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Imgur.API.Authentication.Impl;
using Imgur.API.Models;
using System.Diagnostics;
using Imgur.API;
using Imgur.API.Endpoints.Impl;
using System.Configuration;
using System.Linq.Expressions;

namespace EasyWinterface {
    static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main() {
            var ocr = new PixelMatchOCR();
            var wintScanner = new WinterfaceScanner();
            var appContext = new EWAppContext();

            const int SCAN_TIMEOUT = 150; //ms

            //look for winterface
            var asynchTask = new Task(async () => {
                while (true) {
                    var bmp = await wintScanner.ScanForWinterface(SCAN_TIMEOUT);
                    bmp.Save("temp.bmp");
                    var fi = new FileInfo("temp.bmp");
                    try {
                        var url = await UploadImage(fi.FullName);
                        appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                        var url2 = string.Copy(url);
                        appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (object o, EventArgs a) {
                                Process.Start(url2);
                        });
                        var list = ocr.readWinterface(bmp);
                        updateDB(list, url);
                    } catch (Exception e) when (e is TimeoutException || e is ImgurException) {
                        appContext.TrayIcon.ShowBalloonTip(2000, "Error", "There was an error uploading your image to Imgur.", ToolTipIcon.Error);
                        var list = ocr.readWinterface(bmp);
                        //updateDB(list);
                    }
                    await Task.Delay(24000); //4 minutes
                }
            });

            asynchTask.Start();
            Application.Run(appContext);
        }

        public static async Task<string> UploadImage(string location)
        {
            try
            {
                var client = new ImgurClient("303907803ca83e2", "4dac3d390864caa8e4f2782d61b023205e00f17d");
                var endpoint = new ImageEndpoint(client);
                IImage image = null;
                using (var fs = new FileStream(location, FileMode.Open))
                {
                    var task = endpoint.UploadImageStreamAsync(fs);
                    if (await Task.WhenAny(task, Task.Delay(20000)) == task)
                    {
                        image = await task;
                        if (image == null)
                        {
                            throw new ImgurException("imgur error");
                        }
                        return image.Link;
                    }
                    else
                    {
                        throw new TimeoutException("timeout error imgur");
                    }
                }
            }
            catch (ImgurException imgurEx)
            {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
            return null;
        }

        public static async void updateDB(List<string> list, string url = "optional") {
            await Storage.AddFloor((Floor)list);
        }

    }

}


