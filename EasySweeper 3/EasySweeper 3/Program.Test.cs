using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySweeper_3 {
    partial class Program {

        /*//test the map capture
        static async partial void testMap(CaptureDevice dev) {
            var mps = new EasyMap.MapScanner();
            var map = await mps.ScanForMap(100);
            Console.WriteLine("found map");
        }*/

        //test the winterface capture
        static async partial void testWinterface(CaptureDevice dev) {
            var wfs = new EasyWinterface.WinterfaceScanner();
            //var floor = await wfs.ScanForWinterface(1000);
            Console.WriteLine("found winterface");
        }

    }
}
