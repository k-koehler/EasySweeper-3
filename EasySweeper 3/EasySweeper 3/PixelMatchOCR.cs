using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySweeper_3 {
    public class PixelMatchOCR {

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
            _timerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(timerPath.FullName + "/semicolon.bmp"), ":")); //should be "colon" but cbf

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

            //lvlmodpcnt
            _levelModPcntCmpt = new List<Tuple<Bitmap, string>>();
            _levelModPcntCmpt.AddRange(_timerGlyphs);
            var lvlModPath = new FileInfo("../../Glyphs/lvlModPcntCmptGlyphs/");
            _levelModPcntCmpt.Add(new Tuple<Bitmap, string>(new Bitmap(lvlModPath.FullName + "/plus.bmp"),    "+"));
            _levelModPcntCmpt.Add(new Tuple<Bitmap, string>(new Bitmap(lvlModPath.FullName + "/minus.bmp"),   "-"));
            _levelModPcntCmpt.Add(new Tuple<Bitmap, string>(new Bitmap(lvlModPath.FullName + "/percent.bmp"), "%"));

            //player
            _playerGlyphs = new List<Tuple<Bitmap, string>>();
            var playerPath = new FileInfo("../../Glyphs/playerGlyphs/");
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/0.bmp"), "0"));           
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/2.bmp"), "2"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/3.bmp"), "3"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/4.bmp"), "4"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/5.bmp"), "5"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/6.bmp"), "6"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/7.bmp"), "7"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/8.bmp"), "8"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/9.bmp"), "9"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/a.bmp"), "a"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/b.bmp"), "b"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/c.bmp"), "c"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/d.bmp"), "d"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/e.bmp"), "e"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/f.bmp"), "f"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/g.bmp"), "g"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/h.bmp"), "h"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/j.bmp"), "j"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/i.bmp"), "i"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/k.bmp"), "k"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/m.bmp"), "m"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/n.bmp"), "n"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/o.bmp"), "o"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/p.bmp"), "p"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/q.bmp"), "q"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/r.bmp"), "r"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/s.bmp"), "s"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/t.bmp"), "t"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/u.bmp"), "u"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/v.bmp"), "v"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/w.bmp"), "w"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/x.bmp"), "x"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/y.bmp"), "y"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/z.bmp"), "z"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/a_cap.bmp"), "A"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/b_cap.bmp"), "B"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/c_cap.bmp"), "C"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/d_cap.bmp"), "D"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/e_cap.bmp"), "E"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/f_cap.bmp"), "F"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/g_cap.bmp"), "G"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/h_cap.bmp"), "H"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/i_cap.bmp"), "I"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/j_cap.bmp"), "J"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/k_cap.bmp"), "K"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/l_cap.bmp"), "L"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/m_cap.bmp"), "M"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/n_cap.bmp"), "N"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/o_cap.bmp"), "O"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/p_cap.bmp"), "P"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/q_cap.bmp"), "Q"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/r_cap.bmp"), "R"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/s_cap.bmp"), "S"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/t_cap.bmp"), "T"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/u_cap.bmp"), "U"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/v_cap.bmp"), "V"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/w_cap.bmp"), "W"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/x_cap.bmp"), "X"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/y_cap.bmp"), "Y"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/z_cap.bmp"), "Z"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/underscore.bmp"), "_"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/dash.bmp"), "-"));

            //check these last
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/1.bmp"), "1"));
            _playerGlyphs.Add(new Tuple<Bitmap, string>(new Bitmap(playerPath.FullName + "/l.bmp"), "l"));

        }

        public enum GLYPH_TYPE { TIMER, FLOOR, PLAYER, LVLMOD_PCNTCMP, SIZE, GUIDE_MODE};

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
            winterfaceData.Add(pixelMatch(winterfaceList[9], GLYPH_TYPE.SIZE));
            winterfaceData.Add(pixelMatch(winterfaceList[10], GLYPH_TYPE.FLOOR));
            winterfaceData.Add(pixelMatch(winterfaceList[11], GLYPH_TYPE.FLOOR));
            winterfaceData.Add(pixelMatch(winterfaceList[12], GLYPH_TYPE.GUIDE_MODE));
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
                return _scanPixels(_playerGlyphs, bitmap, GLYPH_TYPE.PLAYER);
                case GLYPH_TYPE.TIMER:
                return _scanPixels(_timerGlyphs, bitmap);
                case GLYPH_TYPE.SIZE:
                return _scanPixels(_levelModPcntCmpt, bitmap, GLYPH_TYPE.SIZE);
                case GLYPH_TYPE.GUIDE_MODE:
                return _scanPixels(_levelModPcntCmpt, bitmap, GLYPH_TYPE.GUIDE_MODE);
            }
            throw new Exception("this should never happen");
        }



        /// <summary>
        /// where all the good shit happens
        /// scans a larger bitmap for glyphs, returns a string of what it finds
        /// <param name="floorGlyphs">the glyphs which correspond to the winterface type</param>
        /// <param name="bitmap">the larger bitmap</param>
        /// <returns>string representing the text in the pic</returns>
        private static string _scanPixels(List<Tuple<Bitmap, string>> floorGlyphs, Bitmap bitmap, GLYPH_TYPE gt = GLYPH_TYPE.FLOOR) {

            if(gt == GLYPH_TYPE.PLAYER) {
                var playerPath = new FileInfo("../../Glyphs/playerGlyphs/");
                if (MapOperations.FindRec(new Bitmap(playerPath.FullName + "/leecher.bmp"), bitmap).Width != 0) {
                    return null;
                }
            }

            var retStr = "";
            var widthBetweenFindings = 0;
            for (var i = 0; i < bitmap.Width; ++i) {
                foreach (var glyph in floorGlyphs) {
                    if (MapOperations.FindRec(glyph.Item1, MapOperations.cropBitmap(bitmap, new Rectangle(i, 0, glyph.Item1.Width, glyph.Item1.Height))).Width == 0) {
                        if (glyph == floorGlyphs[floorGlyphs.Count - 1])
                            widthBetweenFindings++;
                        continue;
                    }
#if TEST_LIB        //TESTING
                    Console.Write(glyph.Item2);
#endif              //END TESTING
                    if (widthBetweenFindings >= 5 && gt == GLYPH_TYPE.PLAYER)
                        retStr += " ";
                    retStr += glyph.Item2;
                    i += glyph.Item1.Width;
                    widthBetweenFindings = 0;
                }
            }

            if (gt == GLYPH_TYPE.SIZE)
                return _dg_size(retStr);
            else if (gt == GLYPH_TYPE.GUIDE_MODE)
                return _guideMode(retStr);
            else return retStr;
        }

        private static string _guideMode(string retStr) {
            if (retStr == "+0%")
                return "No";
            else return "Yes";
        }

        private static string _dg_size(string v) {
            switch (v) {
                case "+15%":
                    return "Large";
                case "+7%":
                    return "Medium";
                case "+0%":
                    return "Small";
                default:
                    return null;
            }
        }
    }
}
