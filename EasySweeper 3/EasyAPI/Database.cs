using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAPI
{
    public static class Database
    {
        public static async Task<bool> Test()
        {
            int ret = 0;

            try
            {
                using (StoredProcedure s = new StoredProcedure("spTestConnection"))
                {
                    SqlDataReader reader = await s.ExecuteAsync();

                    while (await reader.ReadAsync())
                    {
                        ret = reader.GetInt32(0);
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
                Param("@Duration", SqlDbType.BigInt, null, Round(f.Time)),
                Param("@Size", SqlDbType.NVarChar, 20, f.Size),
                ParticipantsTVP(f.Players, "@FloorParticipants"),           
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
                    return 0;
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Duplicate Floor Detected"))
                    throw (new DuplicateFloorException(ex.Message));
                throw (ex);
            }
        }

        public static async Task<IList<Floor>> SearchFloor
            (IEnumerable<int> ids,
            IEnumerable<int> floorNumbers,
            IEnumerable<Tuple<Player, int>> participants,
            TimeSpan? start, 
            TimeSpan? end, 
            IEnumerable<int> bonuses,
            IEnumerable<int> mods,
            IEnumerable<string> sizes,
            IEnumerable<int> complexities,
            string image,
            DateTime? dateFrom,
            DateTime? dateTo,
            bool ignorePosition = false,
            int? playerCount = null)
        {

            SqlParameter[] parameters =
            {
                TVP<int>("@FloorIDs", ids, "dbo.IntSet"),
                TVP<int>("@Floors", floorNumbers, "dbo.IntSet"),
                ParticipantsTVP(participants, "@FloorParticipants"),
                Param("@DurationFrom", SqlDbType.BigInt, null, Round(start)),
                Param("@DurationTo", SqlDbType.BigInt, null, Round(end)),
                TVP<int>("@Bonuses", bonuses, "dbo.IntSet"),
                TVP<int>("@Mods", mods, "dbo.IntSet"),
                TVP<string>("@Sizes", sizes, "dbo.StringSet"),
                TVP<int>("@Complexities", complexities, "dbo.IntSet"),
                Param("@Image", SqlDbType.NVarChar, 100, image),
                Param("@DateFrom", SqlDbType.DateTime2, 0, dateFrom),
                Param("@DateTo", SqlDbType.DateTime2, 0, dateTo),
                Param("@IgnorePlayerPosition", SqlDbType.Bit, null, ignorePosition),
                Param("@PlayerCount", SqlDbType.Int, null, playerCount)
            };

            List<Floor> floors = new List<Floor>();
            List<Player> players = new List<Player>();
            try
            {
                using (StoredProcedure s = new StoredProcedure("spFloorSearch", parameters))
                {
                    SqlDataReader reader = await s.ExecuteAsync();
                    while (await reader.ReadAsync())
                    {
                        for (int i = 1; i <= 5; i++)
                        {                            
                            if (reader["P" + i] != DBNull.Value)
                                players.Add(new Player((string)reader["P" + i]));
                        }

                        floors.Add(new Floor(
                                (int)reader.GetSqlInt32(reader.GetOrdinal("ID")),
                                players,
                                new TimeSpan(0, 0, 0, Convert.ToInt32(reader["Duration"]), 0),
                                (int)reader["Floor"],
                                (int)reader["Bonus"],
                                (int)reader["Mod"],
                                (string)reader["Size"],
                                Convert.ToDateTime(reader["Date"]),
                                (int)reader["Complexity"]
                            ));

                        players.Clear();
                    }
                }

                foreach (Floor f in floors)
                    Console.WriteLine(f.ToString());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + ' ' + e.StackTrace);
            }

            return floors;
        }

        public static async Task<bool> CheckAPIKey(Guid key)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    Param("@Key", SqlDbType.UniqueIdentifier, null, key),
                    new SqlParameter()
                    {
                        ParameterName = "@Valid",
                        SqlDbType = SqlDbType.Bit,
                        Direction = ParameterDirection.Output
                    }
                };

                using (StoredProcedure s = new StoredProcedure("spAPIKeyCheck", parameters))
                {
                    SqlDataReader reader = await s.ExecuteAsync();
                    while (await reader.ReadAsync())
                    { }
                    return (bool)s.Parameters["@Valid"].Value == true;
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        private static SqlParameter Param(string name, SqlDbType type, int? size, object value)
        {
            return new SqlParameter()
            {
                ParameterName = name,
                SqlDbType = type,
                Value = value ?? DBNull.Value,
                Size = size ?? 0

            };
        }

        private static SqlParameter TVP<T>(string parameterName, IEnumerable<T> values, string tableTypeName, string colName = "Value")
           where T : IConvertible
        {
            if (parameterName == null || tableTypeName == null)
                return null;

            DataTable d = new DataTable();
            d.Columns.Add(colName, typeof(T));

            values = values ?? new T[0];

            foreach (T value in values)
            {
                d.Rows.Add(value);
            }

            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = tableTypeName,
                Value = d
            };
        }


        private static SqlParameter ParticipantsTVP(IList<Player> players, string parameterName = "@FloorParticipants")
        {
            int length = players.Count;

            Tuple<Player, int>[] participants = new Tuple<Player, int>[length];

            for (int i = 0; i < length; i++)
                participants[i] = new Tuple<Player, int>(players[i], i);

            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.FloorParticipants",
                Value =  ParticipantsToDataTable(participants)
            };
        }

        private static SqlParameter ParticipantsTVP(IEnumerable<Tuple<Player, int>> players, string parameterName = "@FloorParticipants")
        {

            return new SqlParameter()
            {
                ParameterName = parameterName,
                SqlDbType = SqlDbType.Structured,
                TypeName = "dbo.FloorParticipants",
                Value = ParticipantsToDataTable(players)
            };
        }

        private static DataTable ParticipantsToDataTable(IEnumerable<Tuple<Player, int>> participants)
        {

            if (participants == null)
                return null;

            DataTable d = new DataTable();
            d.Columns.Add("Name", typeof(string));
            d.Columns.Add("Position", typeof(int));
            d.Columns.Add("UnknownName", typeof(int));

            foreach (var p in participants)
            {
                if (p.Item1 != null)
                {
                    if (p.Item1.OCRFailed)
                    {
                        d.Rows.Add(new object[] {
                            p.Item1.User,
                            p.Item2,
                            p.Item1.OCRFailed
                        });
                    }
                    else if (string.IsNullOrWhiteSpace(p.Item1.User))
                    {
                        d.Rows.Add(new object[] {
                            p.Item1.User,
                            p.Item2,
                            p.Item1.OCRFailed
                        });
                    }
                }
            }
            return d;
        }

        private static long? Round(TimeSpan? time)
        {
            if (!time.HasValue)
                return null;

            return (long)Math.Round(time.Value.TotalSeconds);
        }
    }
}
