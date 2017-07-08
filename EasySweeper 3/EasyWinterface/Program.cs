using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using Imgur.API.Authentication.Impl;
using Imgur.API.Models;
using System.Diagnostics;
using Imgur.API;
using Imgur.API.Endpoints.Impl;
using System.Configuration;
using System.Linq.Expressions;
using Squirrel;

namespace EasyWinterface {
    static class Program {

        const int SCAN_TIMEOUT = 150; //ms

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main() {
            var appContext = new EWAppContext();
            var asynchTask = Tasks.WinterfaceSearch(SCAN_TIMEOUT, appContext);
            var updateTask = Tasks.UpdateVersion();
            Application.Run(appContext);
        }

       

        

    }

}


