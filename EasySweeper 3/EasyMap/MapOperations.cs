using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyMap {
    public class MapOperations {

        public async Task<Bitmap> CaptureMap(CaptureDevice dev, int timeout) {
            Bitmap bmp = null;
            while(bmp == null) {
                Thread.Sleep(timeout);
                bmp = dev.findMap();
            }
            return bmp;
        }

    }
}
