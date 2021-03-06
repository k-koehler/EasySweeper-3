﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using EasyWinterface;

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
            testWinterface(new CaptureDevice());
            var ocr = new EasyWinterface.PixelMatchOCR();
            foreach (var file in Directory.EnumerateFiles("C:\\Users\\Kevin\\Documents\\EasySweeper", "*.bmp"))
            {
                var list = ocr.readWinterface(new Bitmap(file));
                File.AppendAllText("log.txt", list[0]+"\n");
                File.AppendAllText("log.txt", list[1] + "\n");
                File.AppendAllText("log.txt", "percent completed: " + list[2] + "\n");
                File.AppendAllText("log.txt", "size: "        + list[9] + "\n");
                File.AppendAllText("log.txt", "difficulty: "  + list[10]+ "\n");
                File.AppendAllText("log.txt", "complexity: "  + list[11]+ "\n");
                File.AppendAllText("log.txt", "guide mode: " +  list[12]+ "\n");
                File.AppendAllText("log.txt", "level mod: "   + list[3] + "\n");
                File.AppendAllText("log.txt", "player 1: "    + list[4] + "\n");
                File.AppendAllText("log.txt", "player 2: "    + list[5] + "\n");
                File.AppendAllText("log.txt", "player 3: "    + list[6] + "\n");
                File.AppendAllText("log.txt", "player 4: "    + list[7] + "\n");
                File.AppendAllText("log.txt", "player 5: "    + list[8] + "\n");
                File.AppendAllText("log.txt", "----------------------------------------------\n");
            }

#else
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run();
#endif

        }


    }
}
