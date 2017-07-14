using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public class Floor
    {
        private int? _id;
        private List<Player> _players;
        private TimeSpan _time;
        private int _floor;
        private int _bonusPercentage;
        private int _mod;
        private string _url;
        private string _size;
        private int _difficulty;
        private int _complexity;

        public static Player[] TestPlayers = new Player[2]
        {
            new Player("Puff Pure"), 
            new Player("Big Chin")
        };

        public static readonly Floor TestFloor = new Floor(TestPlayers, new TimeSpan(1000), 1, 1, 1, "Large", 1);

        public IEnumerable<Player> Players => _players;
        public TimeSpan Time => _time;
        public int FloorNum => _floor;
        public int BonusPercentage => _bonusPercentage;
        public int Mod => _mod;
        public string Size => _size;
        public int Difficulty => _difficulty;
        public int Complexity => _complexity;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public int? ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public static explicit operator Floor(List<string> strings)
        {
            List<Player> players = new List<Player>();
            TimeSpan time = Conversions.ToTimeSpan(strings[0]);
            int floor = Conversions.ToFloorNumber(strings[1]);
            int bonus = Conversions.ToBonusPercentage(strings[2]);
            int mod = Conversions.ToLevelMod(strings[3]);
            Console.WriteLine("mod");

            //strings 4 -> 8 are the player names.
            //an empty string ("") means the OCR failed
            //a null string means this player was a leecher, so we'll discard this player.
            for (int i = 4; i <= 8; i++)
            {
                if (strings[i] != null)
                {
                    players.Add(new Player(strings[i]));
                }
            }

            string size = strings[9];

            int difficulty = Convert.ToInt32(strings[10]);
            int complexity = Convert.ToInt32(strings[11]);

            return new Floor(players, time, floor, bonus, mod, size, difficulty, complexity);
        }

        public override string ToString()
        {
            return String.Format("Floor: {0} Time: {1} Bonus: {2} Mod: {3}",
                _floor,
                _time.ToString(),
                _bonusPercentage,
                _mod);
        }


        public Floor(
            IEnumerable<Player> players,
            TimeSpan time,
            int floor,
            int bonusPercentage,
            int mod,
            string size,
            int difficulty = 55,
            int complexity = 6,
            string url = null)
        {
            _players = players?.ToList<Player>() ?? new List<Player>();
            _time = time;
            _floor = floor;
            _bonusPercentage = bonusPercentage;
            _mod = mod;
            _size = size;
            _difficulty = difficulty;
            _complexity = complexity;
            _url = url;
        }
    }
}
