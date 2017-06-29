using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySweeper_3 {
    class PixelMatchOCR {

        /// <summary>
        /// a list of all the glyphs
        /// </summary>
        private List<Bitmap> _timerGlyphs;
        private List<Bitmap> _floorGlyphs;
        private List<Bitmap> _playerGlyphs;
        private List<Bitmap> _levelModPcntCmpt;

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
            //TODO
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
            winterfaceData.Add(pixelMatch(winterfaceList[2], GLYPH_TYPE.LVLMOD_PCNTCMP));
            winterfaceData.Add(pixelMatch(winterfaceList[3], GLYPH_TYPE.LVLMOD_PCNTCMP));
            winterfaceData.Add(pixelMatch(winterfaceList[4], GLYPH_TYPE.PLAYER));
            winterfaceData.Add(pixelMatch(winterfaceList[5], GLYPH_TYPE.PLAYER));
            winterfaceData.Add(pixelMatch(winterfaceList[6], GLYPH_TYPE.PLAYER));
            winterfaceData.Add(pixelMatch(winterfaceList[7], GLYPH_TYPE.PLAYER));
            winterfaceData.Add(pixelMatch(winterfaceList[8], GLYPH_TYPE.PLAYER));
            return winterfaceData;
        }

        /// <summary>
        /// public method to read a bitmap if you know its winterfaceType (player, timer, etc.)
        /// </summary>
        public string pixelMatch(Bitmap bitmap, GLYPH_TYPE winterfaceType) {
            switch(winterfaceType) {
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

        private static string _scanPixels(List<Bitmap> _floorGlyphs, Bitmap bitmap) {
            throw new NotImplementedException();
        }
    }
}
