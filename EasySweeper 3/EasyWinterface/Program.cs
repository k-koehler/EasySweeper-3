using System;
using System.Windows.Forms;

using EasySweeper_3;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace EasyWinterface {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {

            var ocr = new PixelMatchOCR();

            //look for winterface
            var asynchTask = new Task(() => {
                while (true) {
                    var bmp = MapOperations.captureWinterface(new CaptureDevice());
                    if (bmp == null) {
                        Thread.Sleep(200);
                        continue;
                    } else {
                        var list = ocr.readWinterface(bmp);
                        updateDB(list);
                        Thread.Sleep(240000); //4 minutes
                    }
                }
            });

            Application.Run();
        }

        public static void updateDB(List<string> list) {
            //TODO
        }
    }
}
