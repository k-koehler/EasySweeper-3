﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Library which consists of all the operations we will perform on our Map Captures
/// </summary>

namespace EasySweeper_3 {
    class MapOperations {

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
                new Rectangle(new Point(30,35),   new Size(49,9)),   //timer                  -> 0
                new Rectangle(new Point(49,79),   new Size(55,11)),  //floor number           -> 1
                new Rectangle(new Point(307,171), new Size(37,9)),   //percentage completed   -> 2
                new Rectangle(new Point(310,191), new Size(33,9)),   //level mod              -> 3
                new Rectangle(new Point(358,61),  new Size(102,41)), //player 1               -> 4
                new Rectangle(new Point(358,111), new Size(102,41)), //player 2               -> 5
                new Rectangle(new Point(358,161), new Size(102,41)), //player 3               -> 6
                new Rectangle(new Point(358,211), new Size(102,41)), //player 4               -> 7
                new Rectangle(new Point(358,261), new Size(102,41))  //player 5               -> 8
            };

            //crop each rectangle from the bitmap and add it to the list
            List<Bitmap> list = new List<Bitmap>();
            foreach(var rec in rectangleList) {
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
        /// this method will contrast and invert each bitmap in a list
        /// </summary>
        /// <param name="winterfaceInformation"></param>
        public static void processList(ref List<Bitmap> winterfaceInformation) {
            for(var i=0; i<winterfaceInformation.Count; ++i) {
                winterfaceInformation[i] = AdjustContrast(winterfaceInformation[i], (float)100.0);
                winterfaceInformation[i] = whiten_and_invert(winterfaceInformation[i]);
                //TODO invert
            }
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
            //TODO
            return null;
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
