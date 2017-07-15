using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Imgur.API;
using Imgur.API.Endpoints.Impl;
using System.Configuration;
using System.Linq.Expressions;
using Squirrel;
using System.Threading;
using System.Configuration;
using EasyAPI;

namespace EasyWinterface {
    public static class Program {
    
        private const int _TIMEOUT = 100;

        const int SCAN_TIMEOUT = 150; //ms

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        public static void Main()
        {
            Tasks.UpdateVersion();
            var ocr = new PixelMatchOCR();
            var wintScanner = new WinterfaceScanner();
            var appContext = new EWAppContext();
            var cheatProtector = new CheatProtector();

            try {
                var ocr = new PixelMatchOCR();
                var wintScanner = new WinterfaceScanner();
                var appContext = new EWAppContext();
                var cheatProtector = new CheatProtector();

                const int SCAN_TIMEOUT = 200; //ms

            //look for winterface
            var asynchTask = new Task(async () => {
                while (true) {
                    var bmp = await wintScanner.ScanForWinterface(SCAN_TIMEOUT);
#if TEST
                    await Database.Test();
#endif

#if !TEST
                    if (!cheatProtector.isWinterfaceValid()) {
                        await Task.Delay(SCAN_TIMEOUT);
                        cheatProtector = new CheatProtector();
                        continue;
                    }
#endif
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
                        } catch (Exception e) when (e is TimeoutException || e is Imgur.API.ImgurException) {
                            appContext.TrayIcon.ShowBalloonTip(2000, "Error", "There was an error uploading your image to Imgur.", ToolTipIcon.Error);
                            var list = ocr.readWinterface(bmp);
                            await Tasks.updateDB(list);
                        }

                        cheatProtector = new CheatProtector();
                        await Task.Delay(6000); //1 minute
                    }
                });

                //var updateTask = Tasks.UpdateVersion();
                asynchTask.Start();
                Application.Run(appContext);
            } catch (System.EntryPointNotFoundException) { }
        }
    }

}


