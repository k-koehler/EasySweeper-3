using System;
using System.Drawing;
using System.Threading.Tasks;

namespace CaptureDevice {
    public class WSGfxCapture : AbstractCapture {

        public WSGfxCapture(CAPTURE_DEVICE_TYPE dt) : base(dt) {}

        protected override Bitmap _captureScreen() {
            return null;
        }

    }
}