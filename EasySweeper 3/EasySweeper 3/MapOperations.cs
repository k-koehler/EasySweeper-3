using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            const ushort WINT_WIDTH = 499, WINT_HEIGHT = 334;
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
        /// this method will contrast and invert each bitmap in a list
        /// </summary>
        /// <param name="winterfaceInformation"></param>
        public static void processList(ref List<Bitmap> winterfaceInformation) {
            for (var i = 0; i < 1/*winterfaceInformation.Count*/; ++i) {
                winterfaceInformation[i] = ResizeImage(
                    winterfaceInformation[i],
                    winterfaceInformation[i].Width  * 5,
                    winterfaceInformation[i].Height * 5);
                winterfaceInformation[i] = AdjustContrast(winterfaceInformation[i], (float)100.0);
                winterfaceInformation[i] = whiten_and_invert(winterfaceInformation[i]);
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
            stringList.Add(tesseract_read_winterface_area(ocr_friendly[0], WinterfaceAreaType.Timer,                EngineMode.Default, PageSegMode.SingleWord,   @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[1], WinterfaceAreaType.Floor_Number,         EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[2], WinterfaceAreaType.Percentage_Completed, EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[3], WinterfaceAreaType.Level_Mod,            EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[4], WinterfaceAreaType.Keyer,                EngineMode.TesseractAndCube, PageSegMode.SingleLine,   @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[5], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[6], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[7], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));
            //stringList.Add(tesseract_read_winterface_area(ocr_friendly[8], WinterfaceAreaType.Player,               EngineMode.TesseractAndCube, PageSegMode.SingleColumn, @"./tessdata"));

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
