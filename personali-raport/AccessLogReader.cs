using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;


/*
    ==== MS Access Table Structure ====

    This class reads data from Access using a SELECT query.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: IDkaardid
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

        const string TABLE_NAME = "IDkaardid";
        const string TABLE_QUERY = "SELECT * FROM " + TABLE_NAME + ";";
        const string TABLE_QUERY_WITH_DATES = "SELECT * FROM " + TABLE_NAME + " WHERE " + DATETIME_FIELD + " > @start AND " + DATETIME_FIELD + " < @end;";
        private OleDbConnection databaseConnection;

        public AccessLogReader(OleDbConnection oleDb)
        {
            Debug.Assert(oleDb != null, "OleDbConnection was null in PersonMessageReader constructor");
            databaseConnection = oleDb;
        }

        public IEnumerable<CardLogEntry> ReadAllCardsInTimespan(DateTime start, DateTime end)
        {
            var cursor = databaseConnection.CreateCommand();

            if (start == null || start.Equals(DateTime.MinValue) ||
                end == null || end.Equals(DateTime.MaxValue))
            {
                cursor.CommandText = TABLE_QUERY;
            } else
            {
                cursor.CommandText = TABLE_QUERY_WITH_DATES;

                cursor.Parameters.Add(new OleDbParameter("@start", OleDbType.DBTimeStamp));
                cursor.Parameters[0].Value = start;

                cursor.Parameters.Add(new OleDbParameter("@end", OleDbType.DBTimeStamp));
                cursor.Parameters[0].Value = end;
            }
            
            CardLogEntry entry = null;

            try
            {
                cursor.Prepare();
                using (var reader = cursor.ExecuteReader())
                {
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

                            entry = new CardLogEntry() {
                                firstName = firstName,
                                lastName = lastName,
                                idCode = idCodeL,
                                datetime = dateTime
                            };
                        }
                    }
                }
                    
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
            }
            catch (OleDbException ex)
            {
                Debug.Print(ex.ToString());
            }

            yield return entry;
        }
    }
}

/*
 Unable to cast COM object of type 'Microsoft.Office.Interop.Excel.ApplicationClass' to interface type 'Microsoft.Office.Interop.Excel._Application'. This operation failed because the QueryInterface call on the COM component for the interface with IID '{000208D5-0000-0000-C000-000000000046}' failed due to the following error: Interface not registered (Exception from HRESULT: 0x80040155).
*/
