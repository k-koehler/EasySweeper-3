using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    class Storage
    {
        public async static Task<bool> AddFloor(Floor floor)
        {
            return false;
        }

        public async static Task<bool> TestDatabase()
        {
            bool success = await Database.Test();
            Console.WriteLine(success ? "\n Yes" : "\n No");
            return  success;
        }
    }
}
