using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EasyWinterface
{
    class DatabaseHelper
    {
        public static string ConnectionString => "Server=NEDSPC\\SQLEXPRESS;Initial Catalog=EasySweeper;uid=EasySweeper;pwd=fkfut";

        private static async Task<SqlCommand> CreateSprocAsync(string sprocName, SqlConnection conn)
        {
            await conn.OpenAsync();

            return new SqlCommand()
            {
                CommandText = sprocName,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };
        }

        public static async Task<Tuple<SqlDataReader, Action>> ExecuteSprocAsync(string sprocName)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = await CreateSprocAsync(sprocName, conn);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            Action cleanup = () =>
            {
                conn.Dispose();
                command.Dispose();
                reader.Dispose();
            };

            return new Tuple<SqlDataReader, Action>(reader, cleanup);
        }
    }
}
