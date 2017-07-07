using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyMap {
    public class MapScanner {

        private CaptureDevice _dev;

        public MapScanner() {
            _dev = new CaptureDevice();
        }

        public async Task<Bitmap> CaptureBitmap(int timeout){
            Bitmap bmp = null;
            while (bmp == null) {
                Thread.Sleep(timeout);
                bmp = MapOperations.CaptureMap(_dev);
            }
            return bmp;
        }

    }
}
