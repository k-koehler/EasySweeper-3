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
using System.Drawing;
using System.Configuration;

namespace EasyWinterface {

    class MyApplicationContext : ApplicationContext {
        //Component declarations
        public NotifyIcon TrayIcon;
        private ContextMenuStrip TrayIconContextMenu;
        private ToolStripMenuItem CloseMenuItem;

        public MyApplicationContext() {
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            InitializeComponent();
            TrayIcon.Visible = true;
        }

        private void InitializeComponent() {
            TrayIcon = new NotifyIcon();

            //The icon is added to the project resources.
            //Here I assume that the name of the file is 'TrayIcon.ico'
            TrayIcon.Icon = Properties.Resources.TrayIcon;


            //Optional - Add a context menu to the TrayIcon:
            TrayIconContextMenu = new ContextMenuStrip();
            CloseMenuItem = new ToolStripMenuItem();
            TrayIconContextMenu.SuspendLayout();

            // 
            // TrayIconContextMenu
            // 
            this.TrayIconContextMenu.Items.AddRange(new ToolStripItem[] {
            this.CloseMenuItem});
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new Size(153, 70);
            // 
            // CloseMenuItem
            // 
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new Size(152, 22);
            this.CloseMenuItem.Text = "Close EasyWinterface";
            this.CloseMenuItem.Click += new EventHandler(this.CloseMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            TrayIcon.ContextMenuStrip = TrayIconContextMenu;
        }

        private void OnApplicationExit(object sender, EventArgs e) {
            //Cleanup so that the icon will be removed when the application is closed
            TrayIcon.Visible = false;
        }

        private void CloseMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 


        [STAThread]
        static void Main() {
            var ocr = new PixelMatchOCR();
            var appContext = new MyApplicationContext();

            //look for winterface
            var asynchTask = new Task(async () => {
                while (true) {
                    var bmp = WinterfaceOperations.captureWinterface(new CaptureDevice());
                    if (bmp == null) {
                        Thread.Sleep(200);
                        continue;
                    } else {
#if TEST
                        Console.WriteLine("winterface detected.");
                        bmp.Save("temp.bmp");
                        var fi = new FileInfo("temp.bmp");
                        try {
                            Console.WriteLine("uploading image...");
                            var url = await UploadImage(fi.FullName);
                            Console.WriteLine("success!");
                            Console.WriteLine("url: " + url);
                            appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                            var url2 = String.Copy(url);
                            appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (Object o, EventArgs a)
                            {
                                if (url2 != null)
                                    Process.Start(url2);
                            });
                            var list = ocr.readWinterface(bmp);
                            updateDB(list, url);
                        } catch (Exception e) when (e is TimeoutException || e is ImgurException) {
                            appContext.TrayIcon.ShowBalloonTip(2000, "Error", "There was an error uploading your image to Imgur.", ToolTipIcon.Error);
                            var list = ocr.readWinterface(bmp);
                            updateDB(list);
                        }
#else
                        bmp.Save("temp.bmp");
                        var fi = new FileInfo("temp.bmp");
                        try {
                            var url = await UploadImage(fi.FullName);
                            appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                            var url2 = String.Copy(url);
                            appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (Object o, EventArgs a)
                            {
                                if (url2 != null)
                                    Process.Start(url2);
                            });
                            var list = ocr.readWinterface(bmp);
                            updateDB(list, url);
                        } catch (Exception e) when (e is TimeoutException || e is ImgurException) {
                            appContext.TrayIcon.ShowBalloonTip(2000, "Error", "There was an error uploading your image to Imgur.", ToolTipIcon.Error);
                            var list = ocr.readWinterface(bmp);
                            updateDB(list);
                        }
#endif
                        await Task.Delay(24000); //4 minutes

                    }
                }
            });

            asynchTask.Start();
            Application.Run(appContext);
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

        public static async void updateDB(List<string> list, string url="optional") {
            await Storage.AddFloor((Floor)list);            
        }

    }

}

    
