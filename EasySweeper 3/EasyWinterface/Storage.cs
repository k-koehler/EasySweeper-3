using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EasyWinterface
{
    class Storage
    {
        public async static Task<bool> AddFloor(Floor floor, int? retry = null)
        {
            try
            {
                floor.ID = await Database.AddFloor(floor);
#if TEST
                MessageBox.Show("Uploaded Successfully!!!!!!!!");
#endif
            }
            catch (DuplicateFloorException ex)
            {
                MessageBox.Show(ex.Message, "Duplicate Floor(s) Detected!");
            }
            catch (SqlException ex)
            {                    
                if(MessageBox.Show(ex.Message, "Database Upload Failed", MessageBoxButtons.RetryCancel) ==
                    DialogResult.Retry)
                {
                    retry = ++retry ?? 1;
                    if (retry < 3)
                        await AddFloor(floor, retry);
                    else
                        MessageBox.Show("Too many retries!");
                }
                // Store on local machine here if we ever implement that...
            }
            
            return floor.ID != null;
        }

        public async static Task<bool> TestDatabase()
        {
            bool success = await Database.Test();
            Console.WriteLine(success ? "\n Yes" : "\n No");
            return  success;
        }
    }
}
