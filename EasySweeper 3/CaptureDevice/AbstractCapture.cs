using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureDevice {

    public enum CAPTURE_DEVICE_TYPE { STATIC, DYNAMIC };

    public abstract class AbstractCapture {

        private CAPTURE_DEVICE_TYPE _deviceType;
        protected abstract Bitmap _captureScreen();

        public AbstractCapture(CAPTURE_DEVICE_TYPE dt) {
            _deviceType = dt;
        }

        public async Task<Bitmap> CaptureImage(Bitmap referenceImage, Rectangle referenceDimenensions) {
            while (true) {
                var captureRec = _findRectangle();
                using (var captureScreen = _captureScreen()) {
                    try {
                        using (var bmp = _searchBitmap(referenceImage, captureScreen)) {
                            return BitmapOperations.CropBitmap(bmp, referenceDimenensions);
                        }
                    } catch (BadImageFormatException e) {
                        continue;
                    }
                }
            }
        }

        private Bitmap _searchBitmap(Bitmap referenceImage, Bitmap captureScreen) {
            throw new NotImplementedException();
        }

        private Rectangle _findRectangle() {
            throw new NotImplementedException();
        }

    }
}
