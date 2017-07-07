using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    static class Database
    {


        public static async Task<bool> Test()
        {
            int ret = 0;            
            using (SqlDataReader reader = await new StoredProcedure("spTestConnection").ExecuteAsync())
            {
                while (await reader.ReadAsync())
                {
                    ret = reader.GetInt32(0);
                }
            }
            return ret == 1;
        }

        //public static async Task<bool> AddFloor(Floor f)
        //{

        //}      
    }
}
