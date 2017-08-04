using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyAPI;

namespace EasyWeb
{
    public static class APIExtensions
    {
        [HTMLColumn("Floor")]
        public static string GetFloorNumber(this Floor floor)
        {
            return floor?.FloorNum.ToString() ?? "";
        }

        [HTMLColumn("Theme")]
        public static string GetTheme(this Floor floor)
        {
            return floor?.Theme ?? "";
        }

        [HTMLColumn("P1")]
        public static string GetP1(this Floor floor)
        {
            return floor?.Players[0].User ?? "";
        }

        [HTMLColumn("P2")]
        public static string GetP2(this Floor floor)
        {
            return floor?.Players[1].User ?? "";
        }

        [HTMLColumn("P3")]
        public static string GetP3(this Floor floor)
        {
            return floor?.Players[2].User ?? "";
        }

        [HTMLColumn("P4")]
        public static string GetP4(this Floor floor)
        {
            return floor?.Players[3].User ?? "";
        }

        [HTMLColumn("P5")]
        public static string GetP5(this Floor floor)
        {
            return floor?.Players[4].User ?? "";
        }

        [HTMLColumn("Date")]
        public static string GetDate(this Floor floor)
        {
            return floor?.Date.ToString("dd MMM yy") ?? "";
        }

        [HTMLColumn("Img")]
        public static string Img(this Floor floor)
        {

            return floor?.Url ?? "";
        }

    }
}