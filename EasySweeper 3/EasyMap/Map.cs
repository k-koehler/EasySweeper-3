using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMap {

    public class Map {

        private const byte _MAP_WIDTH = 8, _MAP_HEIGHT = 8;
        private const byte _HEIGHT_FACTOR = 10; //TODO NOT ACCURATE!
        //the array representing the possible rooms
        private byte[,] _mapArray;

        /// <summary>
        /// enum representing possible room types, corresponds to Properties.Resources.(rooms)
        /// </summary>
        private enum ROOM_TYPES {
            EAST,
            EAST_SOUTH,
            EAST_SOUTH_NORTH,
            NORTH,
            NORTH_EAST,
            NORTH_SOUTH,
            NORTH_SOUTH_WEST,
            NORTH_WEST,
            SOUTH,
            WEST,
            WEST_EAST,
            WEST_EAST_NORTH,
            WEST_EAST_SOUTH,
            WEST_EAST_SOUTH_NOTH,
            WEST_SOUTH,
            WEST_SOUTH_NORTH
        };

        /// <summary>
        /// a pairing of the possible rooms and their picture
        /// </summary>
        private List<Tuple<ROOM_TYPES, Bitmap>> _roomTypeList;

        private static void _initializeRoomTypeList(List<Tuple<ROOM_TYPES, Bitmap>> list) {
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.EAST, Properties.Resources.e));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.EAST_SOUTH, Properties.Resources.es));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.EAST_SOUTH_NORTH, Properties.Resources.esn));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.NORTH, Properties.Resources.n));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.NORTH_EAST, Properties.Resources.ne));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.NORTH_SOUTH, Properties.Resources.ns));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.NORTH_SOUTH_WEST, Properties.Resources.nsw));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.NORTH_WEST, Properties.Resources.nw));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.SOUTH, Properties.Resources.s));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST, Properties.Resources.w));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_EAST, Properties.Resources.we));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_EAST_NORTH, Properties.Resources.wen));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_EAST_SOUTH, Properties.Resources.wes));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_EAST_SOUTH_NOTH, Properties.Resources.wesn));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_SOUTH, Properties.Resources.ws));
            list.Add(new Tuple<ROOM_TYPES, Bitmap>(ROOM_TYPES.WEST_SOUTH_NORTH, Properties.Resources.wsn));
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Map() {
            _mapArray = new byte[_MAP_WIDTH, _MAP_HEIGHT];
            _initializeRoomTypeList(_roomTypeList);
        }

        /// <summary>
        /// Analyze the given map and update the Map class to contain all the relevant data
        /// </summary>
        /// <param name="bmp"></param>
        public void Analyze(Bitmap bmp) {
            for(int i=1; i <= _MAP_HEIGHT; ++i) {
                _scanRow(new Point(0, i * _HEIGHT_FACTOR));
            }
        }

        private void _scanRow(Point point) {
            throw new NotImplementedException();
        }

        public string TEST_FIND_COORDINATES(Bitmap bmp) {
            var retStr = "";
            for(int i=0; i<bmp.Width; ++i) {
                for(int j=0; i<bmp.Height; ++j) {
                    foreach(var option in _roomTypeList) {
                        if(MapOperations.FindRec(option.Item2, bmp).Width != 0) {
                            retStr += i + "," + j + " ";
                        }
                    }
                }
                retStr += "\n";
            }
            return retStr;
        }
    }
}
