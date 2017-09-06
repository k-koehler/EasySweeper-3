using System;
using System.Collections.Generic;
using System.Text;

namespace EasyAPI
{
    class SearchParameters
    {
        private string _toStringOverride = "";
        private readonly StringBuilder s = new StringBuilder();

        public IEnumerable<int> Ids { get; private set; }
        public IEnumerable<int> Floors { get; private set; }
        public IEnumerable<Tuple<Player, int>> Participants { get; private set; }
        public TimeSpan? Start { get; private set; }
        public TimeSpan? End { get; private set; }
        public IEnumerable<int> Bonuses { get; private set; }
        public IEnumerable<int> Mods { get; private set; }
        public IEnumerable<string> Sizes { get; private set; }
        public IEnumerable<int> Complexities { get; private set; }
        public IEnumerable<int> Difficulties { get; private set; }
        public string Image { get; private set; }
        public DateTime? DateFrom { get; private set; }
        public DateTime? DateTo { get; private set; }
        public bool IgnorePosition { get; private set; }
        public int? PlayerCount { get; private set; }


    public SearchParameters(
            IEnumerable<int> ids = null,
            IEnumerable<int> floors = null,
            IEnumerable<Tuple<Player, int>> participants = null,
            TimeSpan? start = null,
            TimeSpan? end = null,
            IEnumerable<int> bonuses = null,
            IEnumerable<int> mods = null,
            IEnumerable<string> sizes = null,
            IEnumerable<int> complexities = null,
            IEnumerable<int> difficulties = null,
            string image = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool ignorePosition = false,
            int? playerCount = null)
        {
            Ids = ids;
            Floors = floors;
            Participants = participants;
            Start = start;
            End = end;
            Bonuses = bonuses;
            Mods = mods;
            Sizes = sizes;
            Complexities = complexities;
            Difficulties = difficulties;
            Image = image;
            DateFrom = dateFrom;
            DateTo = DateTo;
            IgnorePosition = ignorePosition;
            PlayerCount = playerCount;

            AppendIEnumerable("Ids", ids);
            AppendIEnumerable("Floors", floors);

            if (participants != null)
            {
                s.Append("Participants");
                foreach (var postionPlayer in participants)
                {
                    s.Append(postionPlayer.Item1.User);
                    s.Append("_");
                    s.Append(postionPlayer.Item2);
                }
            }

            Append("Start", start);
            Append("End", end);

            AppendIEnumerable("Bonuses", bonuses);
            AppendIEnumerable("Mods", mods);
            AppendIEnumerable("Sizes", sizes);
            AppendIEnumerable("Complexities", complexities);
            AppendIEnumerable("Difficulties", difficulties);

            Append("Image", image);
            Append("DateFrom", dateFrom);
            Append("DateTo", dateTo);
            Append("IgnorePosition", ignorePosition);
            Append("PlayerCount", playerCount);

            _toStringOverride = s.ToString();
        }

        private void AppendIEnumerable<T>(string name, IEnumerable<T> enumerable)
        {
            if (enumerable != null)
            {
                s.Append(name);
                foreach (T t in enumerable)
                {
                    s.Append(t?.ToString());
                    s.Append(",");
                }
            }
        }

        private void Append(string name, object obj)
        {
            if (obj != null)
            {
                s.Append(name);
                s.Append(obj.ToString());
            }
        }

        public override string ToString()
        {
            return _toStringOverride;
        }
    }
}
