using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public sealed class API
    {
        private static API instance;
        public static API Instance => instance;
        public bool Configured => _configured;

        private bool _configured;
        private Guid _key; 

        private API(Guid key)
        {
            _key = key;
        }

        public static API GetInstance()
        {
            if (instance.Configured)
                return instance;

            throw new UnconfiguredAPIException();
        }

        public static async void ConfigureInstance(Guid key, Action<bool> callback = null)
        {
            bool valid = await CheckAPIKey(key);

            if (valid)
            {
                instance = new API(key)
                {
                    _configured = true
                };
            }
            else
            {
                instance = null;
                throw new InvalidAPIKeyException(key);
            }

            callback(valid);
        }

        public async Task<int?> AddFloor(Floor floor, int? retry = null)
        {
            if (Configured)
                return await Database.AddFloor(floor);
            else
                throw new UnconfiguredAPIException();
        }

        public async Task<bool> TestDatabase()
        {
            if (Configured)
            {
                bool success = await Database.Test();
                Console.WriteLine(success ? "\n Yes" : "\n No");
                return success;
            }
            else
            {
                throw new UnconfiguredAPIException();
            }              
        }

        private static async Task<bool> CheckAPIKey(Guid key)
        {
            return await Database.CheckAPIKey(key);
        }
    }
}
