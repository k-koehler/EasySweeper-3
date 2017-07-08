using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using Squirrel;

namespace EasyWinterface {
    class Tasks {

        public static async Task UpdateVersion()
        {
            using (var mgr = new UpdateManager("C:\\Projects\\MyApp\\Releases")) {
                await mgr.UpdateApp();
            }
        }

        public static async void updateDB(List<string> list, string url = "optional") {
            await Storage.AddFloor((Floor)list);
        }

        public static async Task WinterfaceSearch(int SCAN_TIMEOUT, EWAppContext appContext)
        {

            var ocr = new PixelMatchOCR();
            var wintScanner = new WinterfaceScanner();

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
                    updateDB(list);
                }
                await Task.Delay(24000); //4 minutes
            }
        }

        public static async Task<string> UploadImage(string location) {
            try {
                var client = new ImgurClient("303907803ca83e2", "4dac3d390864caa8e4f2782d61b023205e00f17d");
                var endpoint = new ImageEndpoint(client);
                IImage image = null;
                using (var fs = new FileStream(location, FileMode.Open)) {
                    var task = endpoint.UploadImageStreamAsync(fs);
                    if (await Task.WhenAny(task, Task.Delay(20000)) == task) {
                        image = await task;
                        if (image == null) {
                            throw new ImgurException("imgur error");
                        }
                        return image.Link;
                    } else {
                        throw new TimeoutException("timeout error imgur");
                    }
                }
            } catch (ImgurException imgurEx) {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
            return null;
        }

    }
}
