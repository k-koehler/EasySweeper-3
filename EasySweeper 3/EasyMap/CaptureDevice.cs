using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EasyMap {
    public class CaptureDevice {

        //rectangle accepted by GetWindowRect
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //user32 function which gets a rectangle of a windowHandle
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        //name of rs process
        private const string PROC_NAME = "rs2client";
        //dimensions of runescape
        RECT rsDimensions;

        //checks if dimensions of rs2client.exe have changed
        private int checkDimensionChange() {
            //get the process
            foreach (var p in Process.GetProcessesByName(PROC_NAME)) {
                IntPtr rsWindowHandle = p.MainWindowHandle;
                RECT rec = new RECT();
                //check if its dimensions are the same
                if (GetWindowRect(rsWindowHandle, ref rec)) {
                    if (rsDimensions.left == rec.left &&
                        rsDimensions.right == rec.right &&
                        rsDimensions.top == rec.top &&
                        rsDimensions.bottom == rec.bottom) {
                        //debug
                        //Console.Write("didn't change");
                        //1 indicatees the dimensions have not changed
                        return 1;
                    } else {
                        rsDimensions = rec;
                        //0 indicates the dimensions have changed
                        return 0;
                    }
                }
            }

            //2 indicates runescape is not open
            //Console.WriteLine("using max screen");
            return 2;
        }

        //tentative area to detect the map
        private Rectangle tentativeScreenAreaCapture;

        //the smaller bitmap we're looking for which will indicate where the dungeoneering map is on the screen
        private Bitmap mapIndicator;

        //initially no tentative area
        public CaptureDevice() {
            tentativeScreenAreaCapture = new Rectangle(0, 0, 0, 0);
            mapIndicator = Properties.Resources.dgmap;
        }

        //captures an area of the screen into a bitmap
        //area specified by param: area
        public Bitmap bitmapFromScreenArea(Rectangle area) {

            if (area.Width <= 0 || area.Height <= 0)
                return null;

            Bitmap screenshot = new Bitmap(area.Width, area.Height, PixelFormat.Format32bppArgb);
            Graphics screenGraph = Graphics.FromImage(screenshot);
            screenGraph.CopyFromScreen(area.X,
                                       area.Y,
                                       0,
                                       0,
                                       area.Size,
                                       CopyPixelOperation.SourceCopy);
            return screenshot;
        }

        //basic method which returns rectangle of current screen
        private Rectangle getScreenDimensions() {
            return new Rectangle(new Point(SystemInformation.VirtualScreen.X, SystemInformation.VirtualScreen.Y), SystemInformation.VirtualScreen.Size);
        }

        //another basic method which converts a rec to a rectangle
        private Rectangle RECTtoRectangle(RECT rect) {
            Rectangle rec = new Rectangle();
            rec.X = rect.left;
            rec.Y = rect.top;
            rec.Size = new Size(rect.right, rect.bottom);
            return rec;
        }

        //attemps to find a rectangle for the screen
        private void findTentativeRectangle() {

            //case: tentativeScreen has not been set, initialize with either RS client or screen coords
            if (tentativeScreenAreaCapture.Width == 0) {
                if (rsDimensions.right != 0) {
                    tentativeScreenAreaCapture = RECTtoRectangle(rsDimensions);
                } else {
                    tentativeScreenAreaCapture = getScreenDimensions();
                }

            }

            switch (checkDimensionChange()) {
                //case: screen has changed
                case 0:
                    tentativeScreenAreaCapture = RECTtoRectangle(rsDimensions);
                    break;
                //case: screen the same
                case 1:
                    //screen is correct, make last found rec the rec
                    if (foundRectangle.Width == 0)
                        tentativeScreenAreaCapture = RECTtoRectangle(rsDimensions);
                    else tentativeScreenAreaCapture = foundRectangle;
                    break;
                //case rs not open anymore
                default:
                    tentativeScreenAreaCapture = getScreenDimensions();
                    break;
            }

        }

        //the area where the specified rectangle is located
        private Rectangle foundRectangle;
        public Bitmap foundBitmap;

        //search for a small img in a big img and return its location (rectangle)
        public Rectangle findRec(Bitmap optionalBitmap = null) {
            //tolerance
            const double tolerance = 0.1;

            //the small bitmap we're searching for
            Bitmap smallBmp;

            if (optionalBitmap != null) {
                //important to clone because optionalBitmap accessed by other processes...
                //might be null at any point
                smallBmp = (Bitmap)optionalBitmap.Clone();
            } else smallBmp = mapIndicator;

            //set tentativeScreenAreaCapture 
            findTentativeRectangle();
            //capture our bigBmp from which smallBmp is located (potentially)
            Bitmap bigBmp = bitmapFromScreenArea(tentativeScreenAreaCapture);

            //if our bitMapFromScreenArea didn't work properly
            if (bigBmp == null)
                return new Rectangle();

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
            foundRectangle = location;
            foundBitmap = bigBmp;
            return location;
        }

        //function that crops a bitmap to a rectangle
        public static Bitmap cropBitmap(Bitmap img, Rectangle rec) {

            if (img == null)
                return null;

            Bitmap nb = new Bitmap(rec.Width, rec.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(img, -rec.X, -rec.Y);
            return nb;
        }

        //dimensions of our map
        private const short MAP_WIDTH = 328, MAP_HEIGHT = 319, BARDOCK_W = 405, BARDOCK_H = 400;

        public Bitmap findMap(Bitmap optionalMap = null) {

            Rectangle rec;
            if (optionalMap != null) {
                rec = findRec(optionalMap);

                //if no match, return bitmap with 0 width/height
                if (rec.Width == 0)
                    return null;

                //set our rec to map dimensions
                //this is the size of detected rec that we want
                rec.Width = BARDOCK_W;
                rec.Height = BARDOCK_H;
            } else {
                rec = findRec();

                //if no match, return bitmap with 0 width/height
                if (rec.Width == 0)
                    return null;

                //set our rec to map dimensions
                //this is the size of detected rec that we want
                rec.X += 17;
                rec.Y += 23;
                rec.Width = MAP_WIDTH - 39;
                rec.Height = MAP_HEIGHT - 39;
            }


            //debug
            //Console.WriteLine("found match");

            //return a bitmap of the screen area
            return cropBitmap(foundBitmap, rec);
        }
    }
}
