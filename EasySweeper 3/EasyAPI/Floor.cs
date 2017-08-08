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
        private DateTime _date;

        public static Player[] TestPlayers = new Player[2]
        {
            new Player("Puff Pure"),
            new Player("Big Chin")
        };

        public static readonly Floor TestFloor = new Floor(
            players: TestPlayers, 
            time: new TimeSpan(0, 5, 30), 
            bonusPercentage: 1, 
            mod: 1, 
            complexity: 1, 
            size: "Small", 
            floor: 1);

        public IList<Player> Players => _players;
        public TimeSpan Time => _time;
        public int FloorNum => _floor;
        public int BonusPercentage => _bonusPercentage;
        public int Mod => _mod;
        public string Size => _size;
        public int Difficulty => _difficulty;
        public int Complexity => _complexity;
        public DateTime Date => _date;
        public string Theme => CalcTheme();

        public static string[] Themes => new string[] { "Frozen", "Abandoned 1", "Furnished", "Abandoned 2", "Occult", "Warped" };

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
            IList<Player> players,
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

        public Floor(
            int id,
            IList<Player> players,
            TimeSpan time,
            int floor,
            int bonusPercentage,
            int mod,
            string size,
            DateTime date,
            int difficulty = 55,
            int complexity = 6,
            string url = null
            )
            : this(players, time, floor, bonusPercentage, mod, size, difficulty, complexity, url)
        {
            _id = id;
            _date = date;
        }

        private string CalcTheme()
        {
            if (_floor >= 48 && _floor < 60)
                return "Warped";
            if (_floor >= 36)
                return "Occult";
            if (_floor >= 30)
                return "Abandoned 2";
            if (_floor >= 18)
                return "Furnished";
            if (_floor >= 13)
                return "Abandoned 1";
            if (_floor >= 1)
                return "Frozen";

            return "Fak";
        }

        public static bool TryParseTheme(string theme, out string themeRange)
        {
            if (string.IsNullOrWhiteSpace(theme) || theme == "*")
            {
                themeRange = null;
                return false;
            }

            themeRange = "";
            switch (theme.ToLower())
            {
                case "frozen": themeRange = "1-12";
                    break;
                case "abandoned 1": themeRange = "13-17";
                    break;
                case "furnished": themeRange = "18-29";
                    break;
                case "abandoned 2": themeRange = "30-35";
                    break;
                case "occult": themeRange = "36-47";
                    break;
                case "warped": themeRange = "48-60";
                    break;
            }
            if (themeRange != "")
                return true;

            return false;
        }
    }
}
