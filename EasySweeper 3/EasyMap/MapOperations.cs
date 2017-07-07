using System;
using System.Drawing;

namespace EasyMap {
    public static class MapOperations {

        public static Bitmap CaptureMap(CaptureDevice dev) {
            return dev.findMap();
        }

        internal static void ProcessChangedMap(Bitmap bmp, Form1 form) {
            return;
        }
    }
}