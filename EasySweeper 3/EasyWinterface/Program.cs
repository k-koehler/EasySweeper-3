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
using Squirrel;

namespace EasyWinterface {
    static class Program {

        const int SCAN_TIMEOUT = 150; //ms

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main() {
            var asynchTask = Tasks.WinterfaceSearch(SCAN_TIMEOUT, appContext);
            var updateTask = Tasks.UpdateVersion();

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
                        await updateDB(list, url);
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

       

        public static async Task<bool> updateDB(List<string> list, string url = "optional") {
            return await Storage.AddFloor((Floor)list);
        }

    }

}


