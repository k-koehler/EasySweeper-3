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
using System.Configuration;
using EasyAPI;
using System.Drawing;

namespace EasyWinterface {
    static class Tasks {

        private const string UpdateLink = "https://raw.githubusercontent.com/k-koehler/EasySweeper-3/master/EasySweeper%203/EasyWinterface/Releases/RELEASES";

        public static API Api { get; set; }

        public static async void UpdateVersion() {
            _updateVersion();
        }

        private static void _updateVersion() {
            using (var wc = new WebClient()) {
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
            try {
                return await Api.AddFloor((Floor)list);
            } catch (DuplicateFloorException) {
                return 0;
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

        internal static void LogLocal(Bitmap bmp, List<string> list, string url) {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "EasyWinterfaceData");
            Directory.CreateDirectory(path);
            if (bmp != null)
                bmp.Save(path + "\\" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bmp");
            _logLocalFile(path, list, url);
        }

        private static void _logLocalFile(string path, List<string> list, string url) {
                using (var tw = new StreamWriter(path + "\\easylog.txt", true)) {
                    tw.WriteLine(list[0] + "\n");
                    tw.WriteLine(list[1] + "\n");
                    tw.WriteLine("percent completed: " + list[2] + "\n");
                    tw.WriteLine("size: " + list[9] + "\n");
                    tw.WriteLine("difficulty: " + list[10] + "\n");
                    tw.WriteLine("complexity: " + list[11] + "\n");
                    tw.WriteLine("guide mode: " + list[12] + "\n");
                    tw.WriteLine("level mod: " + list[3] + "\n");
                    tw.WriteLine("player 1: " + list[4] + "\n");
                    tw.WriteLine("player 2: " + list[5] + "\n");
                    tw.WriteLine("player 3: " + list[6] + "\n");
                    tw.WriteLine("player 4: " + list[7] + "\n");
                    tw.WriteLine("player 5: " + list[8] + "\n");
                    tw.WriteLine("----------------------------------------------\n");
                    tw.Close();
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

        public static void ConfigureAPI(Action<bool> callback = null)
        {
            Guid key = new Guid(ConfigurationManager.AppSettings["EasyAPIKey"]);

            API.ConfigureInstance(key, (bool valid) =>
            {
                Api = valid ? API.GetInstance() : null;
                callback(valid);
            });
        }

        public enum CATEGORY { _5s, _4s, _3s, Duo, Solo, C1, Small, Med, InvalidFloor };

        public static CATEGORY DetermineCategory(Floor f) {

            if (f == null) {
                Console.WriteLine("null floor");
                return CATEGORY.InvalidFloor ;
            }

            if (f.Complexity == 1)
                return CATEGORY.C1;

            if (f.BonusPercentage != 13) {
                Console.WriteLine("Invalid bonus percentage: " + f.BonusPercentage);
                return CATEGORY.InvalidFloor;
            }

            //TODO implement difficulty in the db
            /*if (f.Difficulty != 11 || f.Difficulty != 55) {
                Console.WriteLine("Invalid difficulty: " + f.Difficulty);
                return CATEGORY.InvalidFloor;
            }*/

            if (f.Size == "Medium")
                return CATEGORY.Med;
            if (f.Size == "Small")
                return CATEGORY.Small;

            switch (f.Players.Count) {
                case 1:
                    return CATEGORY.Solo;
                case 2:
                    return CATEGORY.Duo;
                case 3:
                    return CATEGORY._3s;
                case 4:
                    return CATEGORY._4s;
                case 5:
                    return CATEGORY._5s;
                default:
                    return CATEGORY.InvalidFloor;
            }

            throw new ArgumentException("bad floor");
        }

        public static string CategoryString(CATEGORY c) {
            switch (c) {
                case CATEGORY._5s:
                    return "5 man";
                case CATEGORY._4s:
                    return "4s1l";
                case CATEGORY._3s:
                    return "Trio";
                case CATEGORY.Duo:
                    return "Duo";
                case CATEGORY.Solo:
                    return "Solo";
                case CATEGORY.Small:
                    return "Solo small";
                case CATEGORY.Med:
                    return "Solo med";
                case CATEGORY.C1:
                    return "C1";
                default:
                    return "Invalid Floor";
            }

            throw new ArgumentException("bad category enum");
        }
    }
}
