﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    static class Database
    {
        public static async Task<bool> Test()
        {
            int ret = 0;

            try
            {
                Console.WriteLine("Before SP");
                using (StoredProcedure s = new StoredProcedure("spTestConnection"))
                {
                    Console.WriteLine("AFter SP");
                    SqlDataReader reader = await s.ExecuteAsync();
                    Console.WriteLine("After EXEC");
                    while (await reader.ReadAsync())
                    {
                        ret = reader.GetInt32(0);
                        Console.WriteLine(ret);
                    }
                    return ret == 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

        }

        public static async Task<int?> AddFloor(Floor f)
        {
            SqlParameter[] parameters =
            {
                Param("@Floor", SqlDbType.Int, null, f.FloorNum),
                Param("@Duration", SqlDbType.BigInt, null, f.Time.Ticks),
                Param("@Size", SqlDbType.NVarChar, 20, f.Size),
                new SqlParameter()
                {
                    ParameterName = "@FloorParticipants",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "dbo.FloorParticipants",
                    Value = ParticipantsToDataTable(f.Players.ToArray<Player>())
                },
                Param("@Mod", SqlDbType.Int, null, f.Mod),
                Param("@Bonus", SqlDbType.Int, null, f.BonusPercentage),
                Param("@Complexity", SqlDbType.Int, null, f.Complexity),
                Param("@Image", SqlDbType.NVarChar, 100, f.Url),
                new SqlParameter()
                {
                    ParameterName = "@FloorID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                }
            };
            try
            {
                using (StoredProcedure s = new StoredProcedure("spFloorAdd", parameters))
                {
                    SqlDataReader reader = await s.ExecuteAsync();
                    while (await reader.ReadAsync()) { /*Nothing returned in data set...*/}
                    return (int)s.Parameters["@FloorID"].Value;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Duplicate Floor Detected"))
                    throw (new DuplicateFloorException(ex.Message));
                throw (ex);
            }
        }

        private static SqlParameter Param(string name, SqlDbType type, int? size, object value)
        {
            return new SqlParameter()
            {
                ParameterName = name,
                SqlDbType = type,
                Value = value,
                Size = size ?? 0

            };
        }

        private static DataTable ParticipantsToDataTable(Player[] participants)
        {
            DataTable d = new DataTable();
            d.Columns.Add("Name", typeof(string));
            d.Columns.Add("Position", typeof(int));
            d.Columns.Add("UnknownName", typeof(int));

            for (int i = 0; i < participants.Length; i++)
            {
                d.Rows.Add(new object[]{
                            participants[i].User,
                            i,
                            participants[i].OCRFailed
                        });
            }
            return d;
        }
    }
}