using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySweeper_3 {
    class PixelMatchOCR {

        /// <summary>
        /// a list of all the glyphs
        /// </summary>
        private List<Tuple<Bitmap, string>> _timerGlyphs;
        private List<Tuple<Bitmap, string>> _floorGlyphs;
        private List<Tuple<Bitmap, string>> _playerGlyphs;
        private List<Tuple<Bitmap, string>> _levelModPcntCmpt;

        /// <summary>
        /// constructor, initializes the glyph lists
        /// </summary>
        public PixelMatchOCR() {
            _initializeLists();
        }

        /// <summary>
        /// make the glyph lists useable
        /// </summary>
        private void _initializeLists() {

            //timer
            _timerGlyphs = new List<Tuple<Bitmap, string>>();
            var timerPath = new FileInfo("../../Glyphs/timerGlyphs/");
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/0.bmp"), "0"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/1.bmp"), "1"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/2.bmp"), "2"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/3.bmp"), "3"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/4.bmp"), "4"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/5.bmp"), "5"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/6.bmp"), "6"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/7.bmp"), "7"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/8.bmp"), "8"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/9.bmp"), "9"));
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/semicolon.bmp"), ":"));

            //floor
            _floorGlyphs = new List<Tuple<Bitmap, string>>();
            var floorPath = new FileInfo("../../Glyphs/floorGlyphs/");
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/0.bmp"), "0"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/1.bmp"), "1"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/2.bmp"), "2"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/3.bmp"), "3"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/4.bmp"), "4"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/5.bmp"), "5"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/6.bmp"), "6"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/7.bmp"), "7"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/8.bmp"), "8"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/9.bmp"), "9"));
            _floorGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(floorPath.FullName + "/Floor.bmp"), "Floor "));
        }

        public enum GLYPH_TYPE { TIMER, FLOOR, PLAYER, LVLMOD_PCNTCMP };

        /// <summary>
        /// this is all you need to call if you want to read a winterface
        /// </summary>
        /// <returns>a list corresponding to the data on the winterface</returns>
        public List<string> readWinterface(Bitmap winterface) {
            var winterfaceList = MapOperations.chopWinterface(winterface);
            MapOperations.processList(ref winterfaceList);
            return _pixelMatchGeneric(winterfaceList);
        }

        /// <summary>
        /// for use in public method readWinterface
        /// pixel matches each bitmap in processed winterface list
        /// </summary>
        private List<string> _pixelMatchGeneric(List<Bitmap> winterfaceList) {
            List<string> winterfaceData = new List<string>();
            winterfaceData.Add(pixelMatch(winterfaceList[0], GLYPH_TYPE.TIMER));
            winterfaceData.Add(pixelMatch(winterfaceList[1], GLYPH_TYPE.FLOOR));
            //winterfaceData.Add(pixelMatch(winterfaceList[2], GLYPH_TYPE.LVLMOD_PCNTCMP));
            //winterfaceData.Add(pixelMatch(winterfaceList[3], GLYPH_TYPE.LVLMOD_PCNTCMP));
            //winterfaceData.Add(pixelMatch(winterfaceList[4], GLYPH_TYPE.PLAYER));
            //winterfaceData.Add(pixelMatch(winterfaceList[5], GLYPH_TYPE.PLAYER));
            //winterfaceData.Add(pixelMatch(winterfaceList[6], GLYPH_TYPE.PLAYER));
            //winterfaceData.Add(pixelMatch(winterfaceList[7], GLYPH_TYPE.PLAYER));
            //winterfaceData.Add(pixelMatch(winterfaceList[8], GLYPH_TYPE.PLAYER));
            return winterfaceData;
        }

        /// <summary>
        /// public method to read a bitmap if you know its winterfaceType (player, timer, etc.)
        /// </summary>
        public string pixelMatch(Bitmap bitmap, GLYPH_TYPE winterfaceType) {
            switch (winterfaceType) {
                case GLYPH_TYPE.FLOOR:
                return _scanPixels(_floorGlyphs, bitmap);
                case GLYPH_TYPE.LVLMOD_PCNTCMP:
                return _scanPixels(_levelModPcntCmpt, bitmap);
                case GLYPH_TYPE.PLAYER:
                return _scanPixels(_playerGlyphs, bitmap);
                case GLYPH_TYPE.TIMER:
                return _scanPixels(_timerGlyphs, bitmap);
            }
            throw new Exception("this should never happen");
        }

        /// <summary>
        /// where all the good shit happens
        /// scans a larger bitmap for glyphs, returns a string of what it finds
        /// <param name="floorGlyphs">the glyphs which correspond to the winterface type</param>
        /// <param name="bitmap">the larger bitmap</param>
        /// <returns>string representing the text in the pic</returns>
        private static string _scanPixels(List<Tuple<Bitmap, string>> floorGlyphs, Bitmap bitmap) {
            var retStr = "";
            for (var i = 0; i < bitmap.Width; ++i) {
                foreach (var glyph in floorGlyphs) {
                    if (MapOperations.FindRec(glyph.Item1,MapOperations.cropBitmap(bitmap, new Rectangle(i, 0, glyph.Item1.Width, glyph.Item1.Height))).Width == 0)
                        continue;
#if TEST_LIB        //TESTING
                    Console.Write(glyph.Item2);
#endif              //END TESTING
                    retStr += glyph.Item2;
                    i += glyph.Item1.Width;
                }
            }
            return retStr;
        }
    }
}
