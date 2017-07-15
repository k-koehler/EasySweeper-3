using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Imgur.API;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;
using System.Threading;
using Squirrel;
using EasyAPI;

namespace EasyWinterface {
    class Tasks {

        private const string UpdateLink = "https://raw.githubusercontent.com/k-koehler/EasySweeper-3/master/EasySweeper%203/EasyWinterface/Releases/RELEASES";

        public static async void UpdateVersion()
        {
            _updateVersion();
        }

        private static void _updateVersion()
        {
            using (var wc = new WebClient())
            {
                var str = wc.DownloadString(new Uri(UpdateLink));
                var assembly = Assembly.GetExecutingAssembly();
                var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                var version = fvi.FileVersion;
                if (!str.Contains(version))
                    _promptUser();
            }
        }

        private static void _promptUser() {
            if (MessageBox.Show("Update Available!", "Would you like to download the new version of EasyWinterface?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes) {
                System.Diagnostics.Process.Start("https://github.com/k-koehler/EasySweeper-3/blob/master/EasySweeper%203/EasyWinterface/Releases/Setup.exe?raw=true");
            }
        }

        public static async Task<int?> updateDB(List<string> list, string url = "optional") {
            try
            {
                return await Database.AddFloor((Floor) list);
            }
            catch (DuplicateFloorException)
            {
                return 0;
            }
        }

>>>>>>> 3b41bed... Didn't do much with the database. Added an updater.
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
