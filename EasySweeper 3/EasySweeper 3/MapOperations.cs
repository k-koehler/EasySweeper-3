using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tesseract;

/// <summary>
/// Library which consists of all the operations we will perform on our Map Captures
/// </summary>

namespace EasySweeper_3 {
    partial class MapOperations {

        /// <summary>
        /// capture the winterface
        /// create a new task when using this function
        /// </summary>
        /// <returns>a bitmap of the winterface once it successfully captures</returns>
        public static Bitmap captureWinterface(ref CaptureDevice dev) {

            //if rs is not running, this is not valid
            //eventually fix so runescape must be foreground window
            const string PROC_NAME = "rs2client";
            var list = Process.GetProcessesByName(PROC_NAME);
            if (list.GetLength(0) == 0)
                return null;

            //rec.Width != 0 -> dev.findRec found the winterface
            Rectangle rec = new Rectangle(0, 0, 0, 0);
            rec = dev.findRec(Properties.Resources.winterfaceBmp);
            if (rec.Width == 0)
                return null;

            //garbage collect
            //TODO fix the memory leak
            GC.Collect();

            //some consts which will be needed
            const ushort WINT_WIDTH = 499, WINT_HEIGHT = 334, THREAD_SLEEP = 250;
            //expand the found rec and return it
            rec.Width = WINT_WIDTH;
            rec.Height = WINT_HEIGHT;
            return CaptureDevice.cropBitmap(dev.foundBitmap, rec);
        }

        /// <summary>
        /// captures the map 
        /// create a new task when using this function
        /// </summary>
        /// <returns>a bitmap of the map once it successfully captures</returns>
        public static Bitmap captureMap(ref CaptureDevice dev) {
            return dev.findMap();
        }

        /// <summary>
        /// crops a bitmap according to a rectangle
        /// </summary>
        /// <param name="img">crop this image</param>
        /// <param name="rec">crop the image at rec.location and rec.size</param>
        /// <returns>new bitmap (not reference) of the cropped bitmap</returns>
        public static Bitmap cropBitmap(Bitmap img, Rectangle rec) {

            if (img == null)
                return null;

            Bitmap nb = new Bitmap(rec.Width, rec.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(img, -rec.X, -rec.Y);
            return nb;
        }

        /// <summary>
        /// chops the winterface into OCR-Friendly sub-bitmaps
        /// </summary>
        /// <param name="winterface">an image of the winterface</param>
        /// <returns>list of sub-bitmaps which are the useful bits to use OCR on</returns>
        public static List<Bitmap> chopWinterface(ref Bitmap winterface) {

            //make sure the winterface has the right dimensions
            if (winterface.Width != 499 || winterface.Height != 334)
                throw new FormatException("Invalid winterface dimensions");

            //some random consts for your viewing pleasure
            var rectangleList = new List<Rectangle> {
                new Rectangle(new Point(30,30),   new Size(54,19)),  //timer                  -> 0
                new Rectangle(new Point(49,79),   new Size(55,11)),  //floor number           -> 1
                new Rectangle(new Point(307,171), new Size(37,9)),   //percentage completed   -> 2
                new Rectangle(new Point(310,186), new Size(33,14)),  //level mod              -> 3
                new Rectangle(new Point(356,59),  new Size(76,16)),  //player 1               -> 4
                new Rectangle(new Point(355,110), new Size(105,49)), //player 2               -> 5
                new Rectangle(new Point(355,160), new Size(105,49)), //player 3               -> 6
                new Rectangle(new Point(355,210), new Size(105,49)), //player 4               -> 7
                new Rectangle(new Point(355,260), new Size(105,49))  //player 5               -> 8
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
        /// adjusts the contrast of an image
        /// </summary>
        /// <param name="Image">image for which you want to adjust brightness</param>
        /// <param name="Value">degree of contrast you want</param>
        /// <returns></returns>
        public static Bitmap AdjustContrast(Bitmap Image, float Value) {
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;
            Bitmap NewBitmap = (Bitmap)Image.Clone();
            BitmapData data = NewBitmap.LockBits(
                new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                ImageLockMode.ReadWrite,
                NewBitmap.PixelFormat);
            int Height = NewBitmap.Height;
            int Width = NewBitmap.Width;

            unsafe
            {
                for (int y = 0; y < Height; ++y) {
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);
                    int columnOffset = 0;
                    for (int x = 0; x < Width; ++x) {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        float Red = R / 255.0f;
                        float Green = G / 255.0f;
                        float Blue = B / 255.0f;
                        Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                        Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                        Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

                        int iR = (int)Red;
                        iR = iR > 255 ? 255 : iR;
                        iR = iR < 0 ? 0 : iR;
                        int iG = (int)Green;
                        iG = iG > 255 ? 255 : iG;
                        iG = iG < 0 ? 0 : iG;
                        int iB = (int)Blue;
                        iB = iB > 255 ? 255 : iB;
                        iB = iB < 0 ? 0 : iB;

                        row[columnOffset] = (byte)iB;
                        row[columnOffset + 1] = (byte)iG;
                        row[columnOffset + 2] = (byte)iR;

                        columnOffset += 4;
                    }
                }
            }

            NewBitmap.UnlockBits(data);

            return NewBitmap;
        }

        /// <summary>
        /// honestly, I don't really know how this works, but it works! It does the opposite of what I expected...
        /// inverts a hi-contrast image and turns it into black and white (black text, white background)
        /// </summary>
        /// <param name="bmp">the image to turn into black & white and invert</param>
        public static Bitmap whiten_and_invert(Bitmap bmp) {
            var outBitmap = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++) {
                for (int j = 0; j < bmp.Height; j++) {
                    var colour = bmp.GetPixel(i, j);
                    if (colour.ToArgb() == Color.Black.ToArgb())
                        outBitmap.SetPixel(i, j, Color.White);
                    else outBitmap.SetPixel(i, j, Color.Black);
                }
            }
            return outBitmap;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height) {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes()) {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap Sharpen(Bitmap image) {
            Bitmap sharpenImage = (Bitmap)image.Clone();

            int filterWidth = 3;
            int filterHeight = 3;
            int width = image.Width;
            int height = image.Height;

            // Create sharpening filter.
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++) {
                        for (int filterY = 0; filterY < filterHeight; filterY++) {
                            int imageX = (x - filterWidth / 2 + filterX + width) % width;
                            int imageY = (y - filterHeight / 2 + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x) {
                for (int y = 0; y < height; ++y) {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            sharpenImage.UnlockBits(pbits);

            return sharpenImage;
        }

        /// <summary>
        /// this method will contrast and invert each bitmap in a list
        /// </summary>
        /// <param name="winterfaceInformation"></param>
        public static void processList(ref List<Bitmap> winterfaceInformation) {
            for (var i = 0; i < winterfaceInformation.Count; ++i) {
                winterfaceInformation[i] = ResizeImage(
                    winterfaceInformation[i], 
                    winterfaceInformation[i].Width *  10, 
                    winterfaceInformation[i].Height * 10);
                winterfaceInformation[i] = AdjustContrast(winterfaceInformation[i], (float)100.0);
                winterfaceInformation[i] = whiten_and_invert(winterfaceInformation[i]);
                //winterfaceInformation[i] = Sharpen(winterfaceInformation[i]);
            }
        }

        public enum WinterfaceAreaType { Timer = 0, Floor_Number, Percentage_Completed, Keyer, Player, Level_Mod };

        /// <summary>
        /// reads a preprocessed image, returns a string of its contents
        /// </summary>
        /// <param name="src">the source image</param>
        /// <returns>string representation of the image</returns>
        public static string tesseract_read_winterface_area(Bitmap winterfaceBitmap, WinterfaceAreaType winArea, EngineMode engMode, PageSegMode segMode, string datapath, string configpath = null) {
            
            //setup our tessengine
            TesseractEngine tessEng;
            if(configpath != null)
                tessEng = new TesseractEngine(datapath, "eng", engMode, configpath);
            else tessEng = new TesseractEngine(datapath, "eng", engMode, datapath);
            tessEng.DefaultPageSegMode = segMode;

            //goto correct place :3
            string ret_txt;
            switch (winArea) {
                case WinterfaceAreaType.Timer:
                    ret_txt = tesseract_read_timer(ref tessEng, ref winterfaceBitmap);
                    break;
                case WinterfaceAreaType.Player:
                    ret_txt = tesseract_read_player(ref tessEng, ref winterfaceBitmap);
                    break;
                case WinterfaceAreaType.Percentage_Completed:
                    ret_txt = tesseract_read_percent_completed(ref tessEng, ref winterfaceBitmap);
                    break;
                case WinterfaceAreaType.Keyer:
                    ret_txt = tesseract_read_keyer(ref tessEng, ref winterfaceBitmap);
                    break;
                case WinterfaceAreaType.Floor_Number:
                    ret_txt = tesseract_read_floor_number(ref tessEng, ref winterfaceBitmap);
                    break;
                case WinterfaceAreaType.Level_Mod:
                    ret_txt = tesseract_read_level_mod(ref tessEng, ref winterfaceBitmap);
                    break;
                default:
                    throw new Exception("this should never happen");

            }
            return ret_txt;
        }

        /// <summary>
        /// returns a list of strings of winterface information
        /// </summary>
        /// <param name="winterface">an image of the winterface</param>
        /// <returns>list of strings of winterface information</returns>
        public static List<String> readWinterface(ref Bitmap winterface) {
            //chop into OCR friendly bits
            var ocr_friendly = chopWinterface(ref winterface);
            processList(ref ocr_friendly);
            var stringList = new List<String>();

            //some annoying shit here
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[0], WinterfaceAreaType.Timer,                EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[1], WinterfaceAreaType.Floor_Number,         EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[2], WinterfaceAreaType.Percentage_Completed, EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[3], WinterfaceAreaType.Level_Mod,            EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[4], WinterfaceAreaType.Keyer,                EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[5], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[6], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[7], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[8], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));

            return stringList;
        }

        /// <summary>
        /// interprets a picture of the map as a graph with Vertices as connections and Nodes as rooms
        /// </summary>
        /// <param name="dg_map">an image of the map</param>
        /// <returns>a graph representing the image</returns>
        public static Graph interpretMap(ref Bitmap dg_map) {
            //TODO
            return null;
        }

        /// <summary>
        /// counts the rooms in a valid map
        /// </summary>
        public static uint room_count(ref Graph dg_graph){
            //return the room count
            return dg_graph.roomcount;
        }

    }
}
