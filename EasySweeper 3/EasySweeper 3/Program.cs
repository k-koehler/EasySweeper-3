using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

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

            /*var test_winterface_chopper = MapOperations.chopWinterface(ref test_winterface);
            MapOperations.processList(ref test_winterface_chopper);
            int i = 0;
            foreach(var bmp in test_winterface_chopper) {
                bmp.Save(i.ToString() + ".bmp");
                ++i;
            }*/

            /*Console.WriteLine("Testing Method: readWinterface...\n");
            var list = MapOperations.readWinterface(ref test_winterface);
            foreach(var str in list) {
                Console.WriteLine(str);
            }*/

            var i = 0;
            foreach (var file in Directory.EnumerateFiles("C:\\Users\\Chelsea\\Documents\\EasySweeper", "*.bmp"))
            {
                var bmp_load = new Bitmap(file);
                var test_winterface_chopper = MapOperations.chopWinterface(ref bmp_load);
                MapOperations.processList(ref test_winterface_chopper);
                test_winterface_chopper[0].Save(i + ".bmp");
                Console.WriteLine("filename="+ file + ": " + MapOperations.readWinterface(ref bmp_load)[0]);
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
