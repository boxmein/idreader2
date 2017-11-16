using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


/*
    ==== MS Access Table Structure ====

    This class reads data from Access using a SELECT query.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: Yksus
        Columns:
            TODO TODO TODO TODO TODO TODO TODO TODO      TODO TODO TODO
                 TODO      TODO      TODO TODO      TODO TODO      TODO
                 TODO      TODO TODO TODO TODO TODO      TODO TODO TODO
*/

namespace personali_raport
{
    class AccessLogReader : ILogReader
    {
        const string FIRST_NAME_FIELD = "Eesnimi";
        const string LAST_NAME_FIELD = "Perekonnanimi";
        const string ID_CODE_FIELD = "Isikukood";
        const string DATETIME_FIELD = "Kellaaeg";
        const string KUTSE_FIELD = "Kutse";

        const string TABLE_NAME = "Logi";
        const string TABLE_QUERY = "SELECT * FROM " + TABLE_NAME + ";";
        const string TABLE_QUERY_WITH_DATES = "SELECT Kellaaeg, * FROM " + TABLE_NAME + " WHERE (Logi." + DATETIME_FIELD + " >= @start) AND (Logi." + DATETIME_FIELD + ") <= @end;";

        /// <summary>
        /// Select a list of people with their platoon.
        /// </summary>
        const string ATTENDANCE_QUERY = @"SELECT 
                                        Yksus.Eesnimi & ' ' & Yksus.Perekonnanimi AS Nimi, Yksus.Ryhm
                                    FROM Yksus
                                    INNER JOIN Logi
                                        ON Logi.Isikukood = Yksus.Isikukood
                                    WHERE Logi.Kellaaeg > @start 
                                      AND Logi.Kellaaeg < @end
                                    ORDER BY Logi.Kellaaeg ASC;";
        /// <summary>
        /// Select a list of people with their platoon, limited to only one platoon.
        /// </summary>
        const string ATTENDANCE_QUERY_PLATOON = @"SELECT 
                                        Yksus.Eesnimi & ' ' & Yksus.Perekonnanimi AS Nimi, Yksus.Ryhm
                                    FROM Yksus
                                    INNER JOIN Logi
                                        ON Logi.Isikukood = Yksus.Isikukood
                                    WHERE Logi.Kellaaeg > @start 
                                      AND Logi.Kellaaeg < @end
                                      AND Yksus.Ryhm = @platoon
                                    ORDER BY Logi.Kellaaeg ASC;";
        /// <summary>
        /// Select a count of each Company's members between the timestamps.
        /// </summary>
        const string PERSREP_QUERY = @"SELECT
                                        Yksus.Kompanii,
                                        SUM(IIF(Yksus.KKV='O',1,0)) AS Ohvitsere,
                                        SUM(IIF(Yksus.KKV='AO',1,0)) AS Allohvitsere,
                                        SUM(IIF(Yksus.KKV='S',1,0)) AS Sodureid,
                                        SUM(IIF(Yksus.KKV='TSIV',1,0)) AS Tsiviliste
                                    FROM Yksus
                                    INNER JOIN Logi
                                        ON Logi.Isikukood = Yksus.Isikukood
                                    WHERE Logi.Kellaaeg > @start 
                                      AND Logi.Kellaaeg < @end
                                    GROUP BY Yksus.Kompanii;";

        /// <summary>
        /// Select all serving people and whether or not they have been signed in.
        /// </summary>
        const string TREE_QUERY = @"SELECT 
                                        Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi, 
                                        Yksus.Kompanii, Yksus.Ryhm, COUNT(Logi.Kellaaeg) AS Kohal
                                    FROM Yksus 
                                    LEFT OUTER JOIN Logi 
                                        ON Logi.Isikukood = Yksus.Isikukood 
                                    WHERE Logi.Kellaaeg >= @start
                                        AND Logi.Kellaaeg <= @end
                                    GROUP BY Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi, 
                                             Yksus.Kompanii, Yksus.Ryhm
                                    ORDER BY Yksus.Kompanii, Yksus.Ryhm ASC;";

        private OleDbConnection databaseConnection;

        public AccessLogReader(OleDbConnection oleDb)
        {
            Debug.Assert(oleDb != null, "OleDbConnection was null in PersonMessageReader constructor");
            databaseConnection = oleDb;
        }

        public IEnumerable<CardLogEntry> ReadAllCardsInTimespan(DateTime start, DateTime end)
        {
            Debug.Assert(start != null, "ReadAllCardsInTimespan: start time was null");
            Debug.Assert(end != null, "ReadAllCardsInTimespan: end time was null");
            var cursor = databaseConnection.CreateCommand();

            if (start == null || start.Equals(DateTime.MinValue) ||
                end == null || end.Equals(DateTime.MaxValue))
            {
                cursor.CommandText = TABLE_QUERY;
            } else
            {
                cursor.CommandText = TABLE_QUERY_WITH_DATES;

                cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
                cursor.Parameters[0].Value = start;

                cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
                cursor.Parameters[1].Value = end;
            }
            
            CardLogEntry entry = null;
            OleDbDataReader reader = null;
            try
            {
                cursor.Prepare();
                reader = cursor.ExecuteReader();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Logide tabelit '" + TABLE_NAME + "' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
            }

            if (reader == null)
            {
                Debug.Print("Something went wrong while reading.");
                yield break;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string firstName = reader.GetString(reader.GetOrdinal(FIRST_NAME_FIELD));
                    string lastName = reader.GetString(reader.GetOrdinal(LAST_NAME_FIELD));
                    string idCode = reader.GetString(reader.GetOrdinal(ID_CODE_FIELD));
                    DateTime dateTime = reader.GetDateTime(reader.GetOrdinal(DATETIME_FIELD));

                    long idCodeL = 0;

                    if (!long.TryParse(idCode, out idCodeL))
                    {
                        Debug.Print("Failed to parse idCode to long: {0}", idCode);
                    }

                    entry = new CardLogEntry()
                    {
                        firstName = firstName,
                        lastName = lastName,
                        idCode = idCodeL,
                        datetime = dateTime
                    };
                    yield return entry;
                }
            } else
            {
                yield break;
            }
        }

        public List<PersrepItem> ReadPersrepData(DateTime start, DateTime end) {
            Debug.Assert(start != null, "ReadPersrepData: start time was null");
            Debug.Assert(end != null, "ReadPersrepData: end time was null");
            var cursor = databaseConnection.CreateCommand();

            Debug.Print("ReadPersrepData: start");

            var data = new List<PersrepItem>();

            cursor.CommandText = PERSREP_QUERY;
            
            cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
            cursor.Parameters[0].Value = start;

            cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
            cursor.Parameters[1].Value = end;
            
            CardLogEntry entry = null;
            OleDbDataReader reader = null;
            try
            {
                cursor.Prepare();
                reader = cursor.ExecuteReader();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return data;
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Logide tabelit '" + TABLE_NAME + "' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
                return data;
            }

            if (reader == null)
            {
                Debug.Print("Something went wrong while reading.");
                return data;
            }

            if (reader.HasRows)
            {
                Debug.Print("ReadPersrepData: has rows");
                while (reader.Read())
                {
                    string company = reader.GetString(reader.GetOrdinal("Kompanii"));
                    int ohvitsere = (int) Math.Round(reader.GetDouble(reader.GetOrdinal("Ohvitsere")));
                    int allohvitsere = (int) Math.Round(reader.GetDouble(reader.GetOrdinal("Allohvitsere")));
                    int sodureid = (int) Math.Round(reader.GetDouble(reader.GetOrdinal("Sodureid")));
                    int tsiviliste = (int) Math.Round(reader.GetDouble(reader.GetOrdinal("Tsiviliste")));

                    Debug.Print("PERSREP Kompanii: {0} O {1} AO {2} S {3} TSIV {4}", company, ohvitsere, allohvitsere, sodureid, tsiviliste);
                    data.Add(new PersrepItem()
                    {
                        company = company,
                        ohvitsere = ohvitsere,
                        allohvitsere = allohvitsere,
                        sodureid = sodureid,
                        tsiviliste = tsiviliste
                    });
                }
            }
            else
            {
                Debug.Print("ReadPersrepData: no rows");
                return data;
            }
            return data;
        }

        public List<AttendanceItem> ReadAttendanceData(DateTime start, DateTime end, string ryhm = null)
        {
            Debug.Assert(start != null, "ReadAttendanceData: start time was null");
            Debug.Assert(end != null, "ReadAttendanceData: end time was null");
            var cursor = databaseConnection.CreateCommand();

            cursor.CommandText = ryhm == null ? ATTENDANCE_QUERY : ATTENDANCE_QUERY_PLATOON;

            cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
            cursor.Parameters[0].Value = start;

            cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
            cursor.Parameters[1].Value = end;

            var data = new List<AttendanceItem>();

            if (ryhm != null)
            {
                cursor.Parameters.Add(new OleDbParameter("@platoon", OleDbType.VarChar, ryhm.Length));
                cursor.Parameters[2].Value = ryhm;
            }

            CardLogEntry entry = null;
            OleDbDataReader reader = null;
            try
            {
                cursor.Prepare();
                reader = cursor.ExecuteReader();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return data;
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Logide tabelit '" + TABLE_NAME + "' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
                return data;
            }

            if (reader == null)
            {
                Debug.Print("Something went wrong while reading.");
                return data;
            }

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string name = reader.GetString(reader.GetOrdinal("Nimi"));
                    string platoon = reader.GetString(reader.GetOrdinal("Ryhm"));
                    data.Add(new AttendanceItem()
                    {
                        name = name,
                        platoon = platoon
                    });
                }
            }
            else
            {
                return data;
            }
            return data;
        }

        public List<Person> ReadTreeViewData(DateTime start, DateTime end)
        {
            Debug.Assert(start != null, "ReadTreeViewData: start time was null");
            Debug.Assert(end != null, "ReadTreeViewData: end time was null");
            var cursor = databaseConnection.CreateCommand();

            Debug.Print("ReadTreeViewData: start");

            var data = new List<Person>();

            cursor.CommandText = TREE_QUERY;

            cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
            cursor.Parameters[0].Value = start;

            cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
            cursor.Parameters[1].Value = end;

            CardLogEntry entry = null;
            OleDbDataReader reader = null;
            try
            {
                cursor.Prepare();
                reader = cursor.ExecuteReader();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return data;
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Logide või personali tabelit '" + TABLE_NAME + "' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
                return data;
            }

            if (reader == null)
            {
                Debug.Print("Something went wrong while reading.");
                return data;
            }

            if (reader.HasRows)
            {
                Debug.Print("ReadTreeViewData: has rows");
                while (reader.Read())
                {
                    string firstName = reader.GetString(reader.GetOrdinal("Eesnimi"));
                    string lastName = reader.GetString(reader.GetOrdinal("Perekonnanimi"));
                    string company = reader.GetString(reader.GetOrdinal("Kompanii"));
                    string platoon = reader.GetString(reader.GetOrdinal("Ryhm"));
                    string attends = reader.GetInt32(reader.GetOrdinal("Kohal")).ToString();

                    var p = new Person();
                    p.data.Add("Eesnimi", firstName);
                    p.data.Add("Perekonnanimi", lastName);
                    p.data.Add("Kompanii", company);
                    p.data.Add("Ryhm", platoon);
                    p.data.Add("Kohal", attends);
                    data.Add(p);
                }
            }
            else
            {
                Debug.Print("ReadTreeViewData: no rows");
                return data;
            }
            return data;
        }

    }

    /// <summary>
    /// Represents an item in the attendance report.
    /// </summary>
    public class AttendanceItem
    {
        /// <summary>
        /// The attending person's name.
        /// </summary>
        public string name;
        /// <summary>
        /// The attending person's platoon.
        /// </summary>
        public string platoon;
    }

    /// <summary>
    /// Represents a PERSREP row.
    /// </summary>
    public class PersrepItem
    {
        /// <summary>
        /// The company that this PERSREP item counts.
        /// Kompanii, mida on loendatud.
        /// </summary>
        public string company;

        /// <summary>
        /// The amount of officer-ranked people in the company who attended.
        /// Ohvitseride arv, kes selles kompaniis kohale tulid.
        /// </summary>
        public int ohvitsere;

        /// <summary>
        /// The amount of subofficer-ranked people in the company who attended.
        /// Allohvitseride arv, kes selles kompaniis kohale tulid.
        /// </summary>
        public int allohvitsere;

        /// <summary>
        /// The amount of soldiers in the company who attended.
        /// Sõdurite arv, kes kohale tulid.
        /// </summary>
        public int sodureid;

        /// <summary>
        /// The amount of civilians who attended.
        /// Tsivilistide arv, kes kohale tulid.
        /// </summary>
        public int tsiviliste;
    }
}
