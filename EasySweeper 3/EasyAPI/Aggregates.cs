using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public class Aggregates
    {

        private string _theme;
        private int? _playerCount;

        public string Theme => _theme;
        public int? PlayerCount => _playerCount;

        public int? TotalFloors { get; set; }
        public Nullable<TimeSpan> FastestTime { get; set; }
        public Nullable<TimeSpan> SlowestTime { get; set; }
        public Nullable<TimeSpan> AverageTime { get; set; }
        public Nullable<TimeSpan> TotalTime { get; set; }
        public Nullable<TimeSpan> StandardDeviationTime { get; set; }
        public int? AverageMod { get; set; }


        public Aggregates(
            string theme, 
            int? playerCount, 
            int? totalFloors = null,
            Nullable<TimeSpan> fastestTime = null,
            Nullable<TimeSpan> slowestTime = null,
            Nullable<TimeSpan> averageTime = null,
            Nullable<TimeSpan> totalTime = null,
            Nullable<TimeSpan> standardDeviationTime = null,
            int? averageMod = null)
        {
            _theme = theme ?? "";
            _playerCount = playerCount;
            TotalFloors = totalFloors;
            SlowestTime = slowestTime;
            FastestTime = fastestTime;
            AverageTime = averageTime;
            TotalTime = totalTime;
            StandardDeviationTime = standardDeviationTime;
            AverageMod = averageMod;
        }
    }
}
