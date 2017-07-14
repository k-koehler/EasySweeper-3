using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EasyAPI;

namespace EasyWeb
{
    public partial class Main : System.Web.UI.Page
    {
        public Floor[] Floors { get; private set; } 

        protected void Page_Load(object sender, EventArgs e)
        {
            Floors = GetFloors();
        }

        private Floor[] GetFloors()
        {
            return new Floor[1]
            {
                Floor.TestFloor
            };
        }
    }
}