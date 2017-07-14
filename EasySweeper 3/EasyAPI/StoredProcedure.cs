using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    class StoredProcedure : IDisposable
    {
        private string _spName;
        private Action _dispose;
        private SqlParameter[] _parameters;
        private SqlCommand _sqlCommand;
        public SqlParameterCollection Parameters => _sqlCommand.Parameters;

        public StoredProcedure(string spName, SqlParameter[] parameters = null)
        {
            _spName = spName;
            _parameters = parameters;
        }

        public async Task<SqlDataReader> ExecuteAsync()
        {
            Tuple<SqlDataReader, SqlCommand, Action> resultTuple = await DatabaseHelper.ExecuteSprocAsync(_spName, _parameters);
            SqlDataReader results = resultTuple.Item1;
            _sqlCommand = resultTuple.Item2;
            _dispose = resultTuple.Item3;
            return results;
        }


        #region IDisposable Members
        public void Dispose()
        {
            _dispose?.Invoke();
        }
    }
    #endregion

}

