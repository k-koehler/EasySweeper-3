using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWinterface
{
    class StoredProcedure : IDisposable
    {
        private string _spName;
        private Action _dispose;
        public StoredProcedure(string spName)
        {
            _spName = spName;
        }

        public async Task<SqlDataReader> ExecuteAsync()
        {
            Tuple<SqlDataReader, Action> resultTuple = await DatabaseHelper.ExecuteSprocAsync(_spName);
            SqlDataReader results = resultTuple.Item1;
            _dispose = resultTuple.Item2;
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

