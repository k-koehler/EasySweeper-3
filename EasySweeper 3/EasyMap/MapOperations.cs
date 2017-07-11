using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace EasyMap {

    public static partial class MapOperations {

        public static Bitmap CaptureMap(ref CaptureDevice dev) {
            return dev.findMap();
        }

        internal static void ProcessChangedMap(Bitmap bmp, Form1 form, ref Map map) {
            //Console.WriteLine(map.TEST_FIND_COORDINATES(bmp));
            Thread.Sleep(1000);
        }
    }
}