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

namespace EasyWinterface {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static string url = null;

        [STAThread]
        static void Main() {

            var ocr = new PixelMatchOCR();

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
                        await UploadImage(fi.FullName);
                        Console.WriteLine("url: " + url);

#else
                        var list = ocr.readWinterface(bmp);
                        updateDB(list);
#endif
                        Thread.Sleep(240000); //4 minutes

                    }
                }
            });

            asynchTask.Start();
            Application.Run();
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

        public static void updateDB(List<string> list) {
            //TODO
        }
    }
}

    
