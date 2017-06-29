using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasySweeper_3 {

    static partial class Program {

        //test the map capture
        static partial void testMap(CaptureDevice dev);

        //test the winterface capture
        static partial void testWinterface(CaptureDevice dev);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

#if TEST_LIB



#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

        }
    }
}
