using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyAPI;
using System.Threading.Tasks;

namespace EasyWeb
{
    public partial class People : System.Web.UI.Page
    {
        public Player _player;
        public Floor[] _floors;
        public int? _count;


        protected async void Page_Load(object sender, EventArgs e)
        {
            string playerName = (string)RouteData.Values["Username"];
            string strCount = (string)RouteData.Values["Count"];

            _count = TypeExtension.TryParseNullable(strCount);

            Page.Title = playerName;
            _player = new Player(playerName);

            _floors = (await Global.API.SearchFloor(
                participants: _player.User, 
                ignorePosition: true, 
                playerCount: _count)
                ).ToArray();
        }

        public static string GenerateTable(IEnumerable<Floor> floors, string theme, int count)
        {
            string[] properties = new string[count + 3];
            properties[0] = "Time";
            for(int i = 1; i < count; i++)
            {
                properties[i] = "P" + i;
            }

            return HTMLGenerator.GenerateTable<Floor>(
                floors.Where(
                    floor => 
                        floor.Theme.ToLower() == theme.ToLower() 
                        && floor.Players.Count == count),
                theme + "Table",
                properties).ToString();
        }

        //public static IEnumerable<string> GenerateThemeTables(IEnumerable<Floor> floors, int count)
        //{
        //    string[] properties = new string[count + 3];
        //    properties[0] = "Time";
        //    for (int i = 1; i < count; i++)
        //    {
        //        properties[i] = "P" + i;
        //    }
        //
        //    foreach (string s in Floor.Themes)
        //    {
        //        yield return HTMLGenerator.GenerateTable<Floor>(
        //        floors.Where(
        //            floor =>
        //                floor.Players.Count == count),
        //        s + "Table",
        //        properties).ToString();
        //    }
        //}
    }
}