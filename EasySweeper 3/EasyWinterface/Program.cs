using System;
using System.Windows.Forms;
using EasySweeper_3;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;

namespace EasyWinterface {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            var ocr = new PixelMatchOCR();

            //look for winterface
            var asynchTask = new Task(() => {
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
                        PostToImgur(fi.FullName, "303907803ca83e2");
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

        public static string PostToImgur(string imagFilePath, string apiKey) {
            byte[] imageData;
            FileStream fileStream = File.OpenRead(imagFilePath);
            imageData = new byte[fileStream.Length];
            fileStream.Read(imageData, 0, imageData.Length);

#if TEST
            Console.WriteLine("converting");
#endif

            var base64str = Convert.ToBase64String(imageData);
            string uploadRequestString = "image=" + Uri.EscapeDataString(base64str + "&key=" + apiKey);

#if TEST
            Console.WriteLine("done converting");
#endif
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://api.imgur.com/2/upload");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ServicePoint.Expect100Continue = false;
            StreamWriter streamWriter = new StreamWriter(webRequest.GetRequestStream());
            streamWriter.Write(uploadRequestString);
            streamWriter.Close();

            WebResponse response = webRequest.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(responseStream);

#if TEST
            Console.WriteLine("writing to imgur");
#endif
            return responseReader.ReadToEnd();
        }

        public static void updateDB(List<string> list) {
            //TODO
        }
    }
}

    
