using System;
using System.Windows.Forms;
using EasySweeper_3;
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

            TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
            TrayIcon.BalloonTipText =
              "I noticed that you double-clicked me! What can I do for you?";
            TrayIcon.BalloonTipTitle = "You called Master?";
            TrayIcon.Text = "My fabulous tray icon demo application";


            //The icon is added to the project resources.
            //Here I assume that the name of the file is 'TrayIcon.ico'
            TrayIcon.Icon = Properties.Resources.TrayIcon;

            //Optional - handle doubleclicks on the icon:
            TrayIcon.DoubleClick += TrayIcon_DoubleClick;

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

        private void TrayIcon_DoubleClick(object sender, EventArgs e) {
            //Here you can do stuff if the tray icon is doubleclicked
            TrayIcon.ShowBalloonTip(10000);
        }

        private void CloseMenuItem_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Do you really want to close me?",
                    "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                Application.Exit();
            }
        }
    }

    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static string url = null;

        [STAThread]
        static void Main() {
            var ocr = new PixelMatchOCR();
            var appContext = new MyApplicationContext();

            //look for winterface
            var asynchTask = new Task(async () => {
                while (true) {
                    var bmp = MapOperations.captureWinterface(new CaptureDevice());
                    if (bmp == null) {
                        Thread.Sleep(200);
                        continue;
                    } else {
#if TEST
                        Console.WriteLine(MapOperations.GetActiveWindowTitle());
                        Console.WriteLine("winterface detected.");
                        bmp.Save("temp.bmp");
                        var fi = new FileInfo("temp.bmp");
                        Console.WriteLine("uploading...");
                        await UploadImage(fi.FullName);
                        Console.WriteLine("done uploading");
                        appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                        var url2 = String.Copy(url);
                        appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (Object o, EventArgs a)
                        {
                            if(url2 != null)
                                Process.Start(url2);
                        });
                        Console.WriteLine("url: " + url);
                        url = null;

#else
                        bmp.Save("temp.bmp");
                        var fi = new FileInfo("temp.bmp");
                        await UploadImage(fi.FullName);
                        appContext.TrayIcon.ShowBalloonTip(2000, "EasyWinterface", "Winterface uploaded. Click here.", ToolTipIcon.Info);
                        appContext.TrayIcon.BalloonTipClicked += new EventHandler(delegate (Object o, EventArgs a)
                        var url2 = String.Copy(url);
                        {
                            Process.Start(url2);
                        });
                        var list = ocr.readWinterface(bmp);
                        updateDB(list, url);
                        url = null;
#endif
                        Thread.Sleep(240000); //4 minutes

                    }
                }
            });

            asynchTask.Start();
            Application.Run(appContext);
        }

        public static async Task UploadImage(string location) {
            try {
                var client = new ImgurClient("303907803ca83e2", "4dac3d390864caa8e4f2782d61b023205e00f17d");
                var endpoint = new ImageEndpoint(client);
                IImage image;
                using (var fs = new FileStream(location, FileMode.Open)) {
                    image = await endpoint.UploadImageStreamAsync(fs);
                }
                url = image.Link;
            } catch (ImgurException imgurEx) {
                Debug.Write("An error occurred uploading an image to Imgur.");
                Debug.Write(imgurEx.Message);
            }
        }

        public static void updateDB(List<string> list, string url) {
            //TODO
        }
    }
}

    
