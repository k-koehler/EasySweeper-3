using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyAPI
{
    public sealed class API
    {
        private static API instance;
        public static API Instance => GetInstance();
        public bool Configured => _configured;

        private bool _configured;
        private Guid _key;

        private API(Guid key)
        {
            _key = key;
        }

        public static API GetInstance()
        {
            if (instance.Configured)
                return instance;

            throw new UnconfiguredAPIException();
        }

        //<summary></summary>
        public static async void ConfigureInstance(Guid key, Action<bool> callback = null)
        {
            bool valid = await CheckAPIKey(key);

            if (valid)
            {
                instance = new API(key)
                {
                    _configured = true
                };
            }
            else
            {
                instance = null;
                throw new InvalidAPIKeyException(key);
            }

            callback(valid);
        }

        public async Task<int?> AddFloor(Floor floor, int? retry = null)
        {
            CheckConfigured();

            return await Database.AddFloor(floor);
        }

        public async Task<bool> TestDatabase()
        {
            CheckConfigured();

            bool success = await Database.Test();
            Console.WriteLine(success ? "\n Yes" : "\n No");
            return success;
        }
        
        /// <summary>
        /// Find floors given a range of search conditions. All the conditions passed will be anded together. Any nulls will be ignored.
        /// 
        /// </summary>
        /// <param name="ids">Collection of Floor IDs to search for</param>
        /// <param name="participants">List of players with their position, to search for</param>
        /// <param name="start">Lower bound on times to search for, for example passing as <code>new TimeSpan(hours: 0, minutes: 5, seconds: 30)</code> will return any floors that took longer than 5 minutes 30 seconds to complete.</param>
        /// <param name="end">Upper bound on times to search for, for example passing as <code>new TimeSpan(hours: 0, minutes: 5, seconds: 30)</code> will return any floors that took less than 5 minutes 30 seconds to complete.</param>
        /// <param name="bonuses">Collection of Bonus percentages to search for</param>
        /// <param name="mods">Collection of Level Mods to search for</param>
        /// <param name="sizes">Case insensitive collection of sizes to search for</param>
        /// <param name="complexities">Collection of complexities to search for</param>
        /// <param name="image">Case insensitive string to search for. Floors are uploaded with an Imgur picture of their winterface if submitted by EasyWinterface, this parameter searches the URL of each floor's image</param>
        /// <param name="dateFrom">Lower bound of dates to search for. For example passing <code>new DateTime(year: 2017, month: 1, day: 1)</code> would return any floors completed on or after this date.</param>
        /// <param name="dateTo">Upper bound of dates to search for. For example passing <code>new DateTime(year: 2017, month: 1, day: 1)</code> would return any floors completed on or before this date.</param>
        /// <param name="ignorePosition">When true instructs the search to ignore the position of players in the participants parameter</param>
        /// <returns>Collection containing any floors that meet the specified confitions.</returns>
        /// <example>
        /// Sample code to find any Large Floors faster than 5 minutes that had a bonus percentage completion of 11%-15%, completed in 2017.
        /// <code>
        /// List<Floor> floors;
        /// Task.Run(async () =&gt
        /// {
        ///     floors = (List&ltFloor&gt) (await API.Instance.SearchFloor(
        ///             sizes: new string[] { "Large" },
        ///             start: new TimeSpan(hours: 0, minutes: 5, seconds: 0),
        ///             bonuses: new int[] { 10, 11, 12, 13, 14, 15 },
        ///             dateFrom: new DateTime(year:2017, month:1, day:1),
        ///             dateTo: new DateTime(year:2018, month: 1, day:1)
        ///     ));
        /// });
        /// </code>
        /// </example>
        public async Task<IList<Floor>> SearchFloor(
            IEnumerable<int> ids = null,
            IEnumerable<Tuple<Player, int>> participants = null,
            TimeSpan? start = null,
            TimeSpan? end = null,
            IEnumerable<int> bonuses = null,
            IEnumerable<int> mods = null,
            IEnumerable<string> sizes = null,
            IEnumerable<int> complexities = null,
            string image = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool ignorePosition = false)
        {

            CheckConfigured();

            return await Database.SearchFloor(ids, participants, start, end, bonuses, mods, sizes, complexities, image, dateFrom, dateTo, ignorePosition);
        }

        public async Task<IList<Floor>> SearchFloor(
            string ids = null,
            string participants = null,
            TimeSpan? start = null,
            TimeSpan? end = null,
            string bonuses = null,
            string mods = null,
            string sizes = null,
            string complexities = null,
            string image = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            bool ignorePosition = false)
        {
            CheckConfigured();

            return await SearchFloor(
                    ids: ParseNumberRanges(ids),
                    participants: ParsePlayerList(participants),
                    start: start,
                    end: end,
                    bonuses: ParseNumberRanges(bonuses),
                    mods: ParseNumberRanges(mods),
                    sizes: sizes?.Split(','),
                    complexities: ParseNumberRanges(sizes),
                    image: image,
                    dateFrom: dateFrom,
                    dateTo: dateTo,
                    ignorePosition: ignorePosition);

        }

        private static IEnumerable<Tuple<Player, int>> ParsePlayerList(string participants)
        {
            if (string.IsNullOrEmpty(participants))
                return null;
            List<Tuple<Player, int>> playersWithPositions = new List<Tuple<Player, int>>();
            string[] ss;

            var iter = participants.Split(',').GetEnumerator();
            for(int i = 0; iter.MoveNext(); i++)
            {
                string s = (string)iter.Current;
                if (s.Contains("!"))
                {
                    ss = s.Split('!');
                    if (ss.Length <= 2)
                    {
                        if (ss[0].Length < 12 && Int32.TryParse(ss[1], out int position))
                        {
                            playersWithPositions.Add(
                                new Tuple<Player, int>(new Player(ss[0]), position));
                        }

                    }
                }
                else
                {
                    playersWithPositions.Add(
                        new Tuple<Player, int>(new Player(s), i));
                }
            }

            return playersWithPositions; 
        }

        private static async Task<bool> CheckAPIKey(Guid key)
        {
            return await Database.CheckAPIKey(key);
        }

        private void CheckConfigured()
        {
            if (!Configured)
                throw new UnconfiguredAPIException();
        }

        /// <summary>
        /// Parses a list of comma separated ranges or single integers.
        /// Items in the list that cannot be parsed will be ignored.
        /// Can handle inputs where the start of the passed range is lower than the end, e.g. 9-5.
        /// Will ignore ranges that contain negative numbers because they suck
        /// </summary>
        /// <param name="ranges">Comma separated list of ranges or single integers to be parsed</param>
        /// <returns>Collection of integers containing all of the single integers passed, with all ranges passed expanded into individual elements</returns>
        /// <example>
        /// ParseNumberRanges("1,2,4-6") would return an IEnumerable containing [1,2,4,5,6]
        /// ParseNumberRanges("1,2,4-6") would return the same as ParseNumberRanges("1,2,6-4")
        /// </example>
        private static IEnumerable<int> ParseNumberRanges(string ranges)
        {
            if (string.IsNullOrEmpty(ranges))
                return null;

            List<int> list = new List<int>();
 
            foreach (string match in ranges.Split(','))
            {
                list.AddRange(ParseMatch(match));
            }

            return list;
        }

        /// <summary>
        /// Parses a single match into either an Enumerable containing 1 int in the case just 1 number is passed
        /// Or multiple consecutive numbers over a range if a range is passed.
        /// Can handle inputs where the start of the passed range is lower than the end, e.g. 9-5.
        /// A parameter that could not be parsed will return an empty IEnumberable&ltint&gt
        /// </summary>
        /// <param name="match">String to parse</param>
        /// <example>
        /// ParseMatch("1") would return an IEnumerable with a single 1 
        /// ParseMatch("1-5") would return an IEnumerable containing [1,2,3,4,5]
        /// ParseMatch("5-1") would return the same as passing "1-5"
        /// </example>
        private static IEnumerable<int> ParseMatch(string match)
        {
            if (string.IsNullOrEmpty(match))
                return Enumerable.Empty<int>();

            if (Int32.TryParse(match, out int single))
                return new int[] { single };
            
            if (Regex.IsMatch(match, @"^\d*-\d*$"))
            {
                return RangeToEnumerable(match);
            }

            return Enumerable.Empty<int>();
        }

        private static IEnumerable<int> RangeToEnumerable(string range)
        {
            string[] split = range.ToString().Split('-');

            if (split.Count() != 2)
                Enumerable.Empty<int>();

            try
            {
                Int32.TryParse(split[0], out int start);
                Int32.TryParse(split[1], out int end);
                int temp;

                if (start > end)
                {
                    temp = end;
                    end = start;
                    start = temp;
                }

                return Enumerable.Range(start, end - start);
            }
            catch (InvalidCastException)
            {
                return Enumerable.Empty<int>();
            }
        }
    }
}
