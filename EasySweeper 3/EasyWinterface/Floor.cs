﻿using System;
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
        private int _mod;

        public IEnumerable<Player> Players => _players;
        public TimeSpan Time => _time;
        public int FloorNum => _floor;
        public int BonusPercentage => _bonusPercentage;
        public int Mod => _mod;

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
            
            return new Floor(players, time, floor, bonus, mod);
        }

        public override string ToString()
        {
            return String.Format("Floor: {0} Time: {1} Bonus: {2} Mod: {3}",
                _floor,
                _time.ToString(),
                _bonusPercentage,
                _mod);
        }


        public Floor(IEnumerable<Player> players, TimeSpan time, int floor, int bonusPercentage, int mod)
        {
            _players = players?.ToList<Player>() ?? new List<Player>();
            _time = time;
            _floor = floor;
            _bonusPercentage = bonusPercentage;
            _mod = mod;
        }




    }
}