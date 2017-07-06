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
        public static string ConnectionString => "Server=NEDSPC\\SQLEXPRESS;Initial Catalog=EasySweeper;uid=EasySweeper;pwd=fkfut";

        public static async Task<bool> Test()
        {
            int ret=0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = await Sproc("spTestConnection", conn))
                {
                   // Console.WriteLine("in reader");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           // Console.WriteLine("in reader");
                            ret = reader.GetInt32(0);
                        }
                    }
                }
            }

            return ret == 1;
        }

        private static async Task<SqlCommand> Sproc(string sprocName, SqlConnection conn)
        {
            await conn.OpenAsync();

            return new SqlCommand()
            {
                CommandText = sprocName,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };
        }
    }
}
