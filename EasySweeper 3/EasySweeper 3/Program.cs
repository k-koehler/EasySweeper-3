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
            //var dev = new CaptureDevice();
            //testing for map capture
            //testMap(dev);

            //testing for winterface capture
            // testWinterface(dev);

            foreach (var file in Directory.EnumerateFiles("C:\\Users\\Chelsea\\Documents\\EasySweeper", "*.bmp"))
            {
                GC.Collect();
                var bmp_load = new Bitmap(file);
                File.AppendAllText("log.txt", "filename=" + file + ": \n");
                File.AppendAllText("log.txt", "entering read_winterface... data...\n");
                var str_list = MapOperations.readWinterface(ref bmp_load);
                File.AppendAllLines("log.txt", str_list);
            }

            while (true) ;

#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

        }
    }
}
