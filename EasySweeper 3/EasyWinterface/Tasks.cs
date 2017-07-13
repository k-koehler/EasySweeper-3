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
using System.Threading;

namespace EasyWinterface {
    class Tasks {

        /*public static async Task UpdateVersion()
        {
            string updateURL = "https://github.com/k-koehler/EasySweeper-3/tree/master/EasySweeper%203/EasyWinterface/Releases";
            using (var mgr = new UpdateManager(updateURL)) {
                await mgr.UpdateApp();
            }
        }*/

        public static async Task<bool> updateDB(List<string> list, string url = "optional") {
            return await Storage.AddFloor((Floor)list);
        }

        public static async Task WinterfaceSearch(int SCAN_TIMEOUT, EWAppContext appContext) {

            var ocr = new PixelMatchOCR();
            var wintScanner = new WinterfaceScanner();

            try {
                while (true) {
                    var bmp = await wintScanner.ScanForWinterface(SCAN_TIMEOUT);
                    bmp.Save("temp.bmp");
                    var fi = new FileInfo("temp.bmp");
                    await Task.Delay(1000);
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
                        await updateDB(list);
                    }
                    await Task.Delay(24000); //4 minutes
                }
            } catch (System.EntryPointNotFoundException e) {
                Console.WriteLine(e.StackTrace);
            }
        }

        public static async Task<string> UploadImage(string location) {
            var client = new ImgurClient("303907803ca83e2", "7cb9b347002e53227ea79bfb2ea37e344feae6c9");
            await Task.Delay(1000);
            var endpoint = new ImageEndpoint(client);
            IImage image = null;
            using (var fs = new FileStream(location, FileMode.Open)) {
                var task = endpoint.UploadImageStreamAsync(fs);
                if (await Task.WhenAny(task, Task.Delay(45000)) == task) {
                    image = await task;
                    if (image == null) {
                        throw new ImgurException("imgur error");
                    }
                    return image.Link;
                } else {
                    throw new TimeoutException("timeout error imgur");
                }
            }
        }

        internal static async Task TrackForegroundWindow(List<IntPtr> _windowBuffer, TimeSpan _bufferLength, CancellationTokenSource cts) {
            List<Tuple<IntPtr, DateTime>> internalList = new List<Tuple<IntPtr, DateTime>>();
            while (true) {
                if (cts.IsCancellationRequested) {
                    _checkAge(ref internalList, ref _windowBuffer, DateTime.Now - _bufferLength);
                    break;
                }
                IntPtr foregroundWindow = WinterfaceOperations.GetForegroundWindow();
                if (!_windowBuffer.Contains(foregroundWindow)) {
                    internalList.Add(new Tuple<IntPtr, DateTime>(foregroundWindow, DateTime.Now));
                    _windowBuffer.Add(foregroundWindow);
                    _checkAge(ref internalList, ref _windowBuffer, DateTime.Now - _bufferLength); //2 seconds
                } else {
                    for (int i= 0; i<internalList.Count; ++i) {
                        if (internalList[i].Item1 == foregroundWindow) {
                            internalList[i] = new Tuple<IntPtr, DateTime>(foregroundWindow, DateTime.Now);
                            break;
                        }
                    }
                }
                await Task.Delay(200);
            }
        }

        private static void _checkAge(ref List<Tuple<IntPtr, DateTime>> internalList, ref List<IntPtr> _windowBuffer, DateTime dt) {
            foreach(var windowDate in internalList) {
                if (windowDate.Item2.CompareTo(dt) < 0) //older
                    _windowBuffer.Remove(windowDate.Item1);
            }
        }
    }
}
