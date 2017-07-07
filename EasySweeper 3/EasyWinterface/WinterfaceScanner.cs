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
            Bitmap bmp = null;
            while (bmp == null) {
                Thread.Sleep(timeout);
                bmp = WinterfaceOperations.captureWinterface(dev);
            }
            return bmp;
        }
    }
}
