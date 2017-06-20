using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Library which consists of all the operations we will perform on our Map Captures
/// </summary>

namespace EasySweeper_3 {
    class MapOperations {

        /// <summary>
        /// returns true if the reference contains a valid map capture
        /// </summary>
        public static bool is_valid_map(ref Bitmap map) {
            //all we're interested in right now is if the picture has the right dimensions
            //TODO using Size.Empty for now
            return map.Size == Size.Empty;
        }

        /// <summary>
        /// counts the rooms in a valid map
        /// </summary>
        /// <param name="map">
        /// the reference to the map for which the room count is required
        /// </param>
        /// <returns>
        /// unsigned int from 1-64 representing the # of rooms in the map
        /// returns 0 for invalid map
        /// </returns>
        public static uint room_count(ref Bitmap map){
            
            //return 0 on invalid map
            if (is_valid_map(ref map) == false)
                return 0;

            //return the room count
            //TODO
            return 1;
        }

    }
}
