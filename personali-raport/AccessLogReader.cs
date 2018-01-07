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
        /// Select all serving people and whether or not they have been signed in.
        /// This query also includes the people that are not in the üksuse tabel.
        /// </summary>
        const string TREE_QUERY = @"SELECT 
                                        Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi, 
                                        Yksus.Kompanii, Yksus.Ryhm, Yksus.Ametikoht, COUNT(Logi.Kellaaeg) AS Kohal
                                    FROM Yksus 
                                    LEFT OUTER JOIN Logi 
                                        ON Logi.Isikukood = Yksus.Isikukood 
                                    WHERE (Logi.Kellaaeg >= @start
                                        AND Logi.Kellaaeg <= @end) OR Logi.Kellaaeg IS NULL
                                    GROUP BY Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi, 
                                             Yksus.Kompanii, Yksus.Ryhm, Yksus.Ametikoht
                                    ORDER BY Yksus.Kompanii, Yksus.Ryhm ASC;";

        /// <summary>
        /// Select all people from the list that are not in the üksuse tabel.
        /// </summary>
        const string UNKNOWN_PEOPLE_QUERY = @"SELECT DISTINCT Logi.Eesnimi, Logi.Perekonnanimi, Logi.Isikukood FROM Logi 
                                    LEFT OUTER JOIN Yksus 
                                    ON Yksus.Isikukood = Logi.Isikukood 
                                    WHERE Yksus.ID IS NULL
                                      AND Logi.Kellaaeg >= @start 
                                      AND Logi.Kellaaeg <= @end;";

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

        /// <summary>
        /// Compose a PERSREP query.
        /// </summary>
        private string ComposePersrepQuery(bool hasCompanyFilter, JFilter j1, JFilter j2)
        {
            return "SELECT " + (hasCompanyFilter ? "Logi2.Ryhm" : "Logi2.Kompanii") + @",
                        SUM(IIF(Yksus.KKV = 'O', 1, 0)) AS Ohvitsere,
                        SUM(IIF(Yksus.KKV = 'AO', 1, 0)) AS Allohvitsere,
                        SUM(IIF(Yksus.KKV = 'S', 1, 0)) AS Sodureid,
                        SUM(IIF(Yksus.KKV = 'TSIV', 1, 0)) AS Tsiviliste
                    FROM (SELECT
                                Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi,
                                Yksus.Kompanii, Yksus.Ryhm, Yksus.KKV, COUNT(Logi.Kellaaeg) AS Kohal
                            FROM Yksus
                            LEFT OUTER JOIN Logi
                                ON Logi.Isikukood = Yksus.Isikukood
                            WHERE Logi.Kellaaeg >= @start
                              AND Logi.Kellaaeg <= @end
                              " + (hasCompanyFilter ? " AND Yksus.Kompanii = @company" : "")
                                + (j1.enabled ? (j1.desiredValue == null ? " AND (Yksus.J1 IS NULL)" : " AND Yksus.J1 = @j1") : "")
                                + (j2.enabled ? (j2.desiredValue == null ? " AND (Yksus.J2 IS NULL)" : " AND Yksus.J2 = @j2") : "")
                    +    @" GROUP BY Yksus.Isikukood, Yksus.Eesnimi, Yksus.Perekonnanimi,
                                    Yksus.Kompanii, Yksus.Ryhm, Yksus.Ametikoht, Yksus.KKV
                            ORDER BY Yksus.Kompanii, Yksus.Ryhm ASC) Logi2 
                    WHERE Logi2.Kohal > 0 
                    GROUP BY " + (hasCompanyFilter ? "Logi2.Ryhm" : "Logi2.Kompanii") + ";";
        }

        /// <summary>
        /// Compose an attendance query.
        /// </summary>
        private string ComposeAttendanceQuery(bool hasCompanyFilter, bool hasPlatoonFilter, JFilter j1, JFilter j2)
        {
            return @"SELECT 
                        Yksus.Eesnimi, Yksus.Perekonnanimi, Yksus.Ryhm
                    FROM Yksus
                    INNER JOIN Logi
                        ON Logi.Isikukood = Yksus.Isikukood
                    WHERE Logi.Kellaaeg >= @start 
                      AND Logi.Kellaaeg <= @end"
                    + (hasCompanyFilter ? " AND Yksus.Kompanii = @company" : "")
                    + (hasPlatoonFilter ? " AND Yksus.Ryhm = @platoon" : "") 
                    + (j1.enabled ? (j1.desiredValue == null ? " AND Yksus.J1 IS NULL" : " AND Yksus.J1 = @j1") : "") 
                    + (j2.enabled ? (j2.desiredValue == null ? " AND Yksus.J2 IS NULL" : " AND Yksus.J2 = @j2") : "")
                    + " ORDER BY Logi.Kellaaeg ASC;";
        }

        /// <summary>
        /// Read data for the PERSREP. 
        /// Uses the PERSREP_QUERY.
        /// The PERSREP report can be limited by the start & end dates, and by string matching a company.
        /// </summary>
        /// <param name="start">The minimum signin date, inclusive</param>
        /// <param name="end">The maximum signin date, exclusive</param>
        /// <param name="companyFilter">If present, limit people to only one company.</param>
        /// <returns>List of PERSREP columns.</returns>
        public List<PersrepItem> ReadPersrepData(DateTime start, DateTime end, JFilter j1, JFilter j2, string companyFilter = null) {
            Debug.Assert(start != null, "ReadPersrepData: start time was null");
            Debug.Assert(end != null, "ReadPersrepData: end time was null");
            var cursor = databaseConnection.CreateCommand();

            Debug.Print("ReadPersrepData: start");
            
            var data = new List<PersrepItem>();

            cursor.CommandText = ComposePersrepQuery(companyFilter != null, j1, j2);


            Debug.Print(cursor.CommandText);
            Debug.Print("company filter: {0}", companyFilter);
            Debug.Print("j1 filter: {0}", j1.enabled);
            Debug.Print("j2 filter: {0}", j2.enabled);

            // NOTE: Parameters are order-specific only in access SQL. the @names don't matter, but are good for clarification.

            cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
            cursor.Parameters[0].Value = start;

            cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
            cursor.Parameters[1].Value = end;

            if (companyFilter != null)
            {
                Debug.Print("Company filter active");
                var param = new OleDbParameter("@company", OleDbType.VarWChar, companyFilter.Length);
                param.Value = companyFilter;
                cursor.Parameters.Add(param);
            }

            if (j1.enabled && j1.desiredValue != null)
            {
                Debug.Print("J1 filter active");
                var param = new OleDbParameter("@j1", OleDbType.Integer);
                param.Value = j1.desiredValue;
                cursor.Parameters.Add(param);
            }

            if (j2.enabled && j2.desiredValue != null)
            {
                Debug.Print("J2 filter active");
                var param = new OleDbParameter("@j1", OleDbType.Integer);
                if (j2.desiredValue == null)
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = j2.desiredValue;
                }
                cursor.Parameters.Add(param);
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
                Debug.Print("ReadPersrepData: has rows");
                while (reader.Read())
                {
                    string company;
                    if (companyFilter == null)
                    {
                        company = reader.GetString(reader.GetOrdinal("Kompanii"));
                    } else
                    {
                        company = reader.GetString(reader.GetOrdinal("Ryhm"));
                    }
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

        public List<AttendanceItem> ReadAttendanceData(DateTime start, DateTime end, JFilter j1, JFilter j2, string companyFilter = null, string platoonFilter = null)
        {
            Debug.Assert(start != null, "ReadAttendanceData: start time was null");
            Debug.Assert(end != null, "ReadAttendanceData: end time was null");
            var cursor = databaseConnection.CreateCommand();

            cursor.CommandText = ComposeAttendanceQuery(companyFilter != null, platoonFilter != null, j1, j2);

            Debug.Print("ReadAttendanceData: {0}", cursor.CommandText);
            Debug.Print("ReadAttendanceData: Start: {0}, End: {1} ", start, end);
            Debug.Print("ReadAttendanceData: company filter: {0}", companyFilter);
            Debug.Print("ReadAttendanceData: platoon filter: {0}", platoonFilter);
            Debug.Print("ReadAttendanceData: j1 filter: {0}", j1.enabled);
            Debug.Print("ReadAttendanceData: j2 filter: {0}", j2.enabled);

            cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.Date));
            cursor.Parameters[0].Value = start;

            cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.Date));
            cursor.Parameters[1].Value = end;

            var data = new List<AttendanceItem>();

            if (companyFilter != null)
            {
                Debug.Print("ReadAttendanceData: Using company filter");
                var param = new OleDbParameter("@company", OleDbType.VarWChar, companyFilter.Length);
                param.Value = companyFilter;
                cursor.Parameters.Add(param);
            }

            if (platoonFilter != null)
            {
                Debug.Assert(companyFilter != null, "Company filter cannot be null when platoon filter is active");
                Debug.Print("ReadAttendanceData: Using attendance for platoon: '{0}'", platoonFilter);
                var param = new OleDbParameter("@platoon", OleDbType.VarWChar, platoonFilter.Length);
                param.Value = platoonFilter;
                cursor.Parameters.Add(param);
            }

            if (j1.enabled && j1.desiredValue != null)
            {
                Debug.Print("Using J1 filter in attendance");
                var param = new OleDbParameter("@j1", OleDbType.Integer);
                if (j1.desiredValue == null)
                {
                    param.Value = DBNull.Value;
                } else
                {
                    param.Value = j1.desiredValue;
                }

                cursor.Parameters.Add(param);
            }

            if (j2.enabled && j2.desiredValue != null)
            {
                Debug.Print("Using J2 filter in attendance");
                var param = new OleDbParameter("@j2", OleDbType.Integer);
                if (j2.desiredValue == null)
                {
                    param.Value = DBNull.Value;
                }
                else
                {
                    param.Value = j2.desiredValue;
                }
                cursor.Parameters.Add(param);
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
                    string name = reader.GetString(reader.GetOrdinal("Eesnimi")) + " " + reader.GetString(reader.GetOrdinal("Perekonnanimi"));
                    string platoon = reader.GetString(reader.GetOrdinal("Ryhm"));

                    if (data.Exists((item) => item.name == name))
                    {
                        Debug.Print("{0} on juba nimekirjas", name);
                        continue;
                    }

                    data.Add(new AttendanceItem()
                    {
                        name = name,
                        platoon = platoon
                    });
                }
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
                    string position = reader.GetString(reader.GetOrdinal("Ametikoht"));
                    string attends = reader.GetInt32(reader.GetOrdinal("Kohal")).ToString();

                    var p = new Person();
                    p.data.Add("Eesnimi", firstName);
                    p.data.Add("Perekonnanimi", lastName);
                    p.data.Add("Kompanii", company);
                    p.data.Add("Ryhm", platoon);
                    p.data.Add("Kohal", attends);
                    p.data.Add("Ametikoht", position);
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
        public List<Person> ReadUnknownPeople(DateTime start, DateTime end)
        {
            Debug.Assert(start != null, "ReadUnknownPeople: start time was null");
            Debug.Assert(end != null, "ReadUnknownPeople: end time was null");
            var cursor = databaseConnection.CreateCommand();

            Debug.Print("ReadUnknownPeople: start");

            var data = new List<Person>();

            cursor.CommandText = UNKNOWN_PEOPLE_QUERY;

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
                    string idCode = reader.GetString(reader.GetOrdinal("Isikukood"));

                    var p = new Person();
                    p.data.Add("Eesnimi", firstName);
                    p.data.Add("Perekonnanimi", lastName);
                    p.data.Add("Isikukood", idCode);
                    p.idCode = idCode;
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
        /// The company OR PLATOON that this PERSREP item counts.
        /// Kompanii VÕI RÜHM, mida on loendatud.
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
