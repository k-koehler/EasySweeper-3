using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EasyWinterface {
    class CheatProtector {

        private Queue<IntPtr> _windowBuffer;
        private TimeSpan _bufferLength;
        Task checkForCheating;

        public CheatProtector() {
            _windowBuffer = new Queue<IntPtr>();
            _bufferLength = new TimeSpan(0, 0, 2); //2 seconds
            checkForCheating = Tasks.TrackForegroundWindow(_windowBuffer, _bufferLength);
        }

        public bool isWinterfaceValid() {
            checkForCheating.Dispose();
            return (_windowBuffer.Count == 1) ? true : false;
        }

    }
}
