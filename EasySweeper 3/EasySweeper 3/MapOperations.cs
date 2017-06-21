using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Library which consists of all the operations we will perform on our Map Captures
/// </summary>

namespace EasySweeper_3 {
    class MapOperations {

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
