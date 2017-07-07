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
            SqlParameter[] parameters =
            {
                Param("@a", SqlDbType.Int, 3)
            };
            using (StoredProcedure s = new StoredProcedure("spTestConnection", parameters))
            {
                SqlDataReader reader = await s.ExecuteAsync();

                while (await reader.ReadAsync())
                {
                    ret = reader.GetInt32(0);
                }
            }            
            return ret == (int)parameters[0].Value;
        }

        private static SqlParameter Param(string name, SqlDbType type, object value)
        {
            return new SqlParameter()
            {
                ParameterName = name,
                SqlDbType = type,
                Value = value
            };
        }
       
       public static async Task<int?> AddFloor(Floor f)
       {
            SqlParameter[] parameters =
            {

            }
            return null;
       }      
    }
}
