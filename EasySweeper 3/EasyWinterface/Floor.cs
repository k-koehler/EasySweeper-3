using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    class Floor
    {
        private List<Player> _players;
        private TimeSpan _time;
        private int _floor;
        private int _bonusPercentage;

        public IEnumerable<Player> Players => _players;
        public TimeSpan Time => _time;
        public int FloorNum => _floor;
        public int BonusPercentage => _bonusPercentage;

        public static explicit operator Floor(List<string> strings)
        {
            List<Player> players = new List<Player>();

            int floor = Conversions.ToFloorNumber(strings[0]);
            TimeSpan time = Conversions.ToTimeSpan(strings[1]);
            int bonus = Conversions.ToBonusPercentage(strings[2]);
            int mod = Conversions.ToLevelMod(strings[3]);
            


            return null;
        }

        public Floor(IEnumerable<Player> players, TimeSpan time, int floor, int bonusPercentage)
        {            
            _players = players?.ToList<Player>() ?? new List<Player>();
            _time = time;            
        }




    }
}
