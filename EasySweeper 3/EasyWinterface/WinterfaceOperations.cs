using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Library which consists of all the operations we will perform on our Map Captures
/// </summary>

namespace EasyWinterface {
    public partial class WinterfaceOperations {

        /// <summary>
        /// capture the winterface
        /// create a new task when using this function
        /// </summary>
        /// <returns>a bitmap of the winterface once it successfully captures</returns>
        public static Bitmap captureWinterface(CaptureDevice dev, bool RedMap = false) {

            //if rs is not running, this is not valid
            //eventually fix so runescape must be foreground window
            const string PROC_NAME = "rs2client";
            var list = Process.GetProcessesByName(PROC_NAME);
            if (list.GetLength(0) == 0)
                return null;

#if !TEST
           if (GetActiveWindowTitle() != "RuneScape") {
                return null;
            }           
#endif

            //rec.Width != 0 -> dev.findRec found the winterface
            Rectangle rec = new Rectangle(0, 0, 0, 0);
            if (!RedMap) rec = dev.findRec(Properties.Resources.winterfaceBmp);
            else         rec = dev.findRec(Properties.Resources.wintRed);
            if (rec.Width == 0)
                return null;

            //garbage collect
            GC.Collect();

            //some consts which will be needed
            const ushort WINT_WIDTH = 499, WINT_HEIGHT = 334;
            //expand the found rec and return it
            rec.Width = WINT_WIDTH;
            rec.Height = WINT_HEIGHT;
            return CaptureDevice.cropBitmap(dev.foundBitmap, rec);
        }

        /// <summary>
        /// chops the winterface into OCR-Friendly sub-bitmaps
        /// </summary>
        /// <param name="winterface">an image of the winterface</param>
        /// <returns>list of sub-bitmaps which are the useful bits to use OCR on</returns>
        public static List<Bitmap> chopWinterface(Bitmap winterface) {

            //make sure the winterface has the right dimensions
            if (winterface.Width != 499 || winterface.Height != 334)
                throw new FormatException("Invalid winterface dimensions");

            //some random consts for your viewing pleasure
            var rectangleList = new List<Rectangle> {
                new Rectangle(new Point(30,30),   new Size(54,19)),  //timer                  -> 0
                new Rectangle(new Point(49,79),   new Size(55,11)),  //floor number           -> 1
                new Rectangle(new Point(307,166), new Size(37,14)),  //percentage completed   -> 2
                new Rectangle(new Point(310,186), new Size(33,14)),  //level mod              -> 3
                new Rectangle(new Point(356,60),  new Size(105,16)), //player 1               -> 4
                new Rectangle(new Point(355,110), new Size(105,49)), //player 2               -> 5
                new Rectangle(new Point(355,160), new Size(105,49)), //player 3               -> 6
                new Rectangle(new Point(355,210), new Size(105,49)), //player 4               -> 7
                new Rectangle(new Point(355,260), new Size(105,49)), //player 5               -> 8
                new Rectangle(new Point(305,144), new Size(36,18)),  //size                   -> 9
                new Rectangle(new Point(119,191), new Size(22,11)),  //difficulty             -> 10
                new Rectangle(new Point(126,213), new Size(7,11)),   //complexity             -> 11
                new Rectangle(new Point(309,208), new Size(36,18))   //guide mode             -> 11
            };



            //crop each rectangle from the bitmap and add it to the list
            List<Bitmap> list = new List<Bitmap>();
            foreach (var rec in rectangleList) {
                list.Add(cropBitmap(winterface, rec));
            }

            //free me
            rectangleList.Clear();

            return list;
        }

        /// <summary>
        /// this method will contrast and invert each bitmap in a list
        /// </summary>
        /// <param name="winterfaceInformation"></param>
        public static void processList(ref List<Bitmap> winterfaceInformation) {
            for (var i = 0; i < winterfaceInformation.Count; ++i) {
                winterfaceInformation[i] = AdjustContrast(winterfaceInformation[i], (float)100.0);
                winterfaceInformation[i] = whiten_and_invert(winterfaceInformation[i]);
            }
        }

        //search for a small img in a big img and return its location (rectangle)
        public static Rectangle FindRec(Bitmap smallBmp, Bitmap bigBmp) {
            //tolerance
            const double tolerance = 0.1;

            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       System.Drawing.Imaging.ImageLockMode.ReadOnly,
                       System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            Rectangle location = Rectangle.Empty;
            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++) {
                    for (int x = 0; x < bigWidth; x++) {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++) {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++) {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0]) {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound) {
                            location.X = x;
                            location.Y = y;
                            location.Width = smallBmp.Width;
                            location.Height = smallBmp.Height;
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);
            return location;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public static string GetActiveWindowTitle() {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return null;
        }

        public static string GetWindowTitle(IntPtr handle) {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);

            if (GetWindowText(handle, Buff, nChars) > 0) {
                return Buff.ToString();
            }
            return null;
        }


    }
}
