using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyAPI
{
    static class Conversions
    {
        public static int ToFloorNumber(string floorString)
        {
            return JustDigits(floorString);
        }

        public static TimeSpan ToTimeSpan(string timeString, char[] delimiters = null)
        {
            char[] split = delimiters ?? new char[] { ':' };
            string[] splitTimes = timeString.Split(split);

            return new TimeSpan(
                Convert.ToInt32(splitTimes[0]),
                Convert.ToInt32(splitTimes[1]),
                Convert.ToInt32(splitTimes[2]));
        }

        public static int ToBonusPercentage(string bonusString)
        {
            return JustDigits(bonusString);
        }

        public static int ToLevelMod(string modString)
        {
            return JustDigits(modString);
        }

        private static int JustDigits(string str)
        {
            return Convert.ToInt32(Regex.Replace(str, "\\D+", ""));
        }
    }
}
