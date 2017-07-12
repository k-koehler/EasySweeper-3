using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Imgur.API;
<<<<<<< HEAD
using Imgur.API.Endpoints.Impl;
using System.Configuration;
using System.Linq.Expressions;
using Squirrel;

namespace EasyWinterface {
    public static class Program {
        private const int _TIMEOUT = 100;

        const int SCAN_TIMEOUT = 150; //ms

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        public static void Main() {
            var ocr = new PixelMatchOCR();
            var wintScanner = new WinterfaceScanner();
            var appContext = new EWAppContext();

            const int SCAN_TIMEOUT = 350; //ms

            //look for winterface
            var asynchTask = new Task(async () => {
                while (true) {
                    var bmp = await wintScanner.ScanForWinterface(SCAN_TIMEOUT);
                    bmp.Save("temp.bmp");
                    var fi = new FileInfo("temp.bmp");
                    try {
                        var url = await Tasks.UploadImage(fi.FullName);
                        appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                        var url2 = string.Copy(url);
                        appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (object o, EventArgs a) {
                            Process.Start(url2);
                        });
                        var list = ocr.readWinterface(bmp);
                        await Tasks.updateDB(list, url);
                    } catch (Exception e) when (e is TimeoutException || e is ImgurException) {
                        appContext.TrayIcon.ShowBalloonTip(2000, "Error", "There was an error uploading your image to Imgur.", ToolTipIcon.Error);
                        var list = ocr.readWinterface(bmp);
                        await Tasks.updateDB(list);
                    }
                    await Task.Delay(6000); //1 minute
                }
            });

            asynchTask.Start();
            Application.Run(appContext);
        }
    }

}


