using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public class Storage
    {
        public async static Task<int?> AddFloor(Floor floor, int? retry = null)
        {
                return await Database.AddFloor(floor);
        }

        public async static Task<bool> TestDatabase()
        {
            bool success = await Database.Test();
            Console.WriteLine(success ? "\n Yes" : "\n No");
            return success;
        }
    }
}
