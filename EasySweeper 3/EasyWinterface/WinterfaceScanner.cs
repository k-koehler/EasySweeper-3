using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyWinterface {

    public class WinterfaceScanner {

        CaptureDevice dev;
        PixelMatchOCR ocr;

        public WinterfaceScanner() {
            dev = new CaptureDevice();
            ocr = new PixelMatchOCR();
        }

        public async Task<Bitmap> ScanForWinterface(int timeout) {
            int i = 0;
            Bitmap bmp = null;
            while (bmp == null) {
                await Task.Delay(timeout);
                if (i%2 == 0) bmp = WinterfaceOperations.captureWinterface(dev);
                else          bmp = WinterfaceOperations.captureWinterface(dev, true);
                ++i;
            }
            return bmp;
        }
    }
}
