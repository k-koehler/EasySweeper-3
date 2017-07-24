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
        public static string ConnectionString => "Server=easywinterface.database.windows.net;InitialCatalog=EasyWinterface;uid=tresamigos;pwd=fuckFut123";

        private static async Task<SqlCommand> CreateSprocAsync(string sprocName, SqlConnection conn, SqlParameter[] parameters = null)
        {
            await conn.OpenAsync();
            SqlCommand command = new SqlCommand()
            {
                CommandText = sprocName,
                CommandType = CommandType.StoredProcedure,
                Connection = conn
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            return command;
        }
        // Item1 -> Results of the stored procedure
        // Item2 -> Delegate, which when invoked will dispose off the SQLConnection, SQLCommand and SQLReader used to execute the sproc.
        public static async Task<Tuple<SqlDataReader, SqlCommand, Action>> ExecuteSprocAsync(string sprocName, SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand command = await CreateSprocAsync(sprocName, conn, parameters);
            SqlDataReader reader = await command.ExecuteReaderAsync();

            Action cleanup = () =>
            {
                conn.Dispose();
                command.Dispose();
                reader.Dispose();
            };

            return new Tuple<SqlDataReader, SqlCommand, Action>(reader, command, cleanup);
        }
    }
}
