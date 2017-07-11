using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyMap {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        private static int _getTimeout() { return 100; }

        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new Form1();
            var ms = new MapScanner();
            var map = new Map();

            var MapFind = new Task(async () => {
                while (true) {
                    var bmp = await ms.CaptureMap(_getTimeout());
                    if (form.ChangeMainMapPicture(bmp))
                        MapOperations.ProcessChangedMap(bmp, form, ref map);
                }
            });

            MapFind.Start();
            Application.Run(form);
        }
    }
}
