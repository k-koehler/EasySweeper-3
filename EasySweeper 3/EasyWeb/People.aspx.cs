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

        protected async void Page_Load(object sender, EventArgs e)
        {
            _player = new Player((string)RouteData.Values["Username"]);

            Tuple<Player, int>[] p = { new Tuple<Player, int>(_player, 1) };

            _floors = (await Global.API.SearchFloor(participants: p, ignorePosition:true)).ToArray();

            p = null;

        }

    }
}