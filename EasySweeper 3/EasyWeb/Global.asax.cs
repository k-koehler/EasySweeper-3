using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using EasyAPI;
using System.Web.Configuration;
using System.Configuration;

namespace EasyWeb
{
    public class Global : System.Web.HttpApplication
    {
        private static API _api;

        public static API API => _api;

        protected void Application_Start(object sender, EventArgs e)
        {
            
            ConfigureAPI();
            RegisterRoutes(RouteTable.Routes);

        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("", "", "~/Main.aspx");
            routes.MapPageRoute("", "People/{Username}/{Count}", "~/People.aspx");

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void ConfigureAPI()
        {
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration(null);

            Guid key = new Guid();

            API.ConfigureInstance(key, (bool valid) =>
            {
                _api = valid ? API.Instance : null;
            });
        }
    }
}