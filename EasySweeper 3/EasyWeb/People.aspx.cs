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
        public IList<Floor> _floors;
        public IList<Aggregates> _aggregates;
        public int? _count;

        private static string[] AggregateProperties =
        {
                "Tot Floors",
                "Avg Time",
                "Total Time",
                "Std Dev"
        };

        protected async void Page_Load(object sender, EventArgs e)
        {
            string playerName = (string)RouteData.Values["Username"] ?? "Puff Pure";
            string strCount = (string)RouteData.Values["Count"] ?? "Summary";

            Page.Title = playerName;
            _player = new Player(playerName);

            if (strCount != "Summary")
            {
                _count = TypeExtension.TryParseNullable(strCount);

                _floors = await Global.API.SearchFloor(
                    participants: _player.User,
                    sizes: "Large",
                    complexities: "6",
                    bonuses: "13",
                    difficulties: "55",
                    ignorePosition: true,
                    playerCount: _count);
            }
            else
            {
                _count = null;
                _aggregates = (await Global.API.SearchAggregates(
                    participants: _player.User,
                    sizes: "Large",
                    complexities: "6",
                    bonuses: "13",
                    difficulties: "55",
                    ignorePosition: true,
                    playerCount: _count));
            }
        }

        public static string GenerateTable(IEnumerable<Floor> floors, string theme, int count)
        {
            string[] properties = new string[count + 4];
            properties[0] = "Time";
            int i;
            for (i = 1; i <= count; i++)
            {
                properties[i] = "P" + i;
            }
            properties[i + 1] = "Date";
            properties[i + 2] = "Img";
            return HTMLGenerator.GenerateTable<Floor>(
                floors.Where(
                    floor => 
                        floor.Theme.ToLower() == theme.ToLower() 
                        && floor.Players.Count == count),
                theme + "Table",
                properties).ToString();
        }

        public static string GenerateAggregates(Aggregates aggs)
        {
            Aggregates[] a = { aggs };
            return HTMLGenerator.GenerateTable<Aggregates>(a, aggs.Theme + aggs.PlayerCount + "Table", AggregateProperties)
                .ToString();

        }
    }
}