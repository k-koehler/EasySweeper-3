using System;
using System.Collections.Generic;
using System.Drawing;
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
            var dev = new CaptureDevice();
            //testing for map capture
            //testMap(dev);

            //testing for winterface capture
            //testWinterface(dev);

            //let this thread live on
            //while (true) ;

            var test_winterface = new Bitmap("testWint.bmp");
            var test_winterface_chopper = MapOperations.chopWinterface(ref test_winterface);
            MapOperations.processList(ref test_winterface_chopper);
            int i = 0;
            foreach(var bmp in test_winterface_chopper) {
                bmp.Save(i.ToString() + ".bmp");
                ++i;
            }

#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

        }
    }
}
