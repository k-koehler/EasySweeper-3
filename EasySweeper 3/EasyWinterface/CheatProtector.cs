using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EasyWinterface {
    class CheatProtector {

        private List<IntPtr> _windowBuffer;
        private TimeSpan _bufferLength;
        Task checkForCheating;
        CancellationTokenSource cts;

        public CheatProtector() {
            cts = new CancellationTokenSource();
            _windowBuffer = new List<IntPtr>();
            _bufferLength = new TimeSpan(0, 0, 1); //2 seconds
            checkForCheating = Tasks.TrackForegroundWindow(_windowBuffer, _bufferLength, cts);
        }

        public bool isWinterfaceValid() {
            cts.Cancel();
            return ( (_windowBuffer.Count == 1) && (WinterfaceOperations.GetWindowTitle(_windowBuffer[0]) == "RuneScape")) ? true : false;
        }

    }
}
