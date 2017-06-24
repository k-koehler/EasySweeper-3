using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySweeper_3 {
    partial class Program {

        //test the map capture
        static partial void testMap(CaptureDevice dev) {
            var testTask = new Task(() => {

                while (true) {
                    var bmp = MapOperations.captureMap(ref dev);
                    if (bmp != null)
                        Console.WriteLine("found map");
                    Thread.Sleep(1000);
                    bmp.Dispose();
                }
            });
            testTask.Start();
        }

        //test the winterface capture
        static partial void testWinterface(CaptureDevice dev) {
            Task testTask2 = new Task(() => {

                while (true) {
                    var winterface = MapOperations.captureWinterface(ref dev);

                    if (winterface == null)
                        continue;

                    Console.WriteLine("found winterface");

                    //delete it
                    winterface.Dispose();
                    Thread.Sleep(1000);
                }

            });
            testTask2.Start();
        }

    }
}
