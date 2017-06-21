using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        /// chops the winterface into OCR-Friendly sub-bitmaps
        /// </summary>
        /// <param name="winterface">an image of the winterface</param>
        /// <returns>list of sub-bitmaps which are the useful bits to use OCR on</returns>
        private static List<Bitmap> chopWinterface(ref Bitmap winterface) {
            //TODO
            return null;
        }

        /// <summary>
        /// returns a list of strings of winterface information
        /// </summary>
        /// <param name="winterface">an image of the winterface</param>
        /// <returns>list of strings of winterface information</returns>
        public static List<String> readWinterface(ref Bitmap winterface) {
            //chop into OCR friendly bits
            var ocr_friendly = chopWinterface(ref winterface);
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
