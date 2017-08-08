using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyAPI;
using System.Xml.Linq;

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

        [HTMLColumn("Time")]
        public static string GetTime(this Floor floor)
        {
            return floor?.Time.ToString(@"mm\:ss") ?? "";
        }

        [HTMLColumn("P1")]
        public static string GetP1(this Floor floor)
        {
            return PlayerNumToURL(floor, 0);
        }

        [HTMLColumn("P2")]
        public static string GetP2(this Floor floor)
        {
            return PlayerNumToURL(floor, 1);
        }

        [HTMLColumn("P3")]
        public static string GetP3(this Floor floor)
        {
            return PlayerNumToURL(floor, 2);
        }

        [HTMLColumn("P4")]
        public static string GetP4(this Floor floor)
        {
            return PlayerNumToURL(floor, 3);
        }

        [HTMLColumn("P5")]
        public static string GetP5(this Floor floor)
        {
            return PlayerNumToURL(floor, 4);
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

        private static string PlayerNumToURL(Floor floor, int position)
        {
            if (floor?.Players.Count < position + 1)
                return "";

            string username = floor?.Players[position].User ?? "";
            return "<a href=\"/People/" + username + "/5\">" + username + "</a>";
        }

    }
}