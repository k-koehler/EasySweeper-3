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

            var ocr = new PixelMatchOCR();
            foreach (var file in Directory.EnumerateFiles("C:\\Users\\Chelsea\\Documents\\EasySweeper", "*.bmp"))
            {
                var list = ocr.readWinterface(new Bitmap(file));
                File.AppendAllText("log.txt", list[0]+"\n");
                File.AppendAllText("log.txt", list[1] + "\n");
                File.AppendAllText("log.txt", "percent completed: " + list[2] + "\n");
                File.AppendAllText("log.txt", "level mod: " + list[3] + "\n");
            }

#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
#endif

        }
    }
}
