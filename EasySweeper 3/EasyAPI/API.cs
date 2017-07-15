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

        private Guid _key; 

        private API(Guid key)
        {
            if(CheckAPIKey(key))
            {
                _key = key;
            }
            else
            {
                throw new Exception(); //todo make an exception
            }
            
        }

        public static API GetInstance(Guid key = default(Guid))
        {
            if (key == default(Guid) && instance != null)
            {
                return instance;
            }
            else
            {
                if (key == instance._key)
                {
                    return instance;
                }
                else
                {
                    instance = new API(key);
                    return instance;
                }
                    
            }
        }

        public async Task<int?> AddFloor(Floor floor, int? retry = null)
        {
            return await Database.AddFloor(floor);
        }

        public async Task<bool> TestDatabase()
        {
            bool success = await Database.Test();
            Console.WriteLine(success ? "\n Yes" : "\n No");
            return success;
        }

        private bool CheckAPIKey(Guid key)
        {
            return false;
        }
    }
}
