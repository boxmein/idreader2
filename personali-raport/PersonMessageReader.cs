using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;


/*
    ==== MS Access Table Structure ====

    This class reads data from Access using a SELECT query.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: Isikuteated
        Columns:
            - ID            created automatically (integer primary key)
            - Isikukood     Text
            - Teade         Memo
*/

namespace personali_raport
{
    public class PersonMessageReader
    {
        const string TABLE_NAME = "Isikuteated";
        const string COLUMN_NAME = "Teade";
        const string TABLE_QUERY = "SELECT " + COLUMN_NAME + " FROM " + TABLE_NAME + " WHERE Isikukood = ?;";
        private OleDbConnection databaseConnection;

        public PersonMessageReader(OleDbConnection oleDb)
        {
            Debug.Assert(oleDb != null, "OleDbConnection was null in PersonMessageReader constructor");
            databaseConnection = oleDb;
        }

        /// <summary>
        /// Read the person's personal message if available. Returns null if not found in the database.
        /// </summary>
        /// <param name="idCode">The person's ID code, as a string. Used to match against the FIRST column.</param>
        /// <returns>The person's message, as a string.</returns>
        public string GetPersonMessage(string idCode)
        {
            var cursor = databaseConnection.CreateCommand();
            cursor.CommandText = TABLE_QUERY;
            cursor.Parameters.Add(new OleDbParameter("Isikukood", OleDbType.VarChar, 12));
            cursor.Parameters[0].Value = idCode;
            try {
                cursor.Prepare();
                string message = (string)cursor.ExecuteScalar();
                Debug.Print("Person Message fetch: " + message);
                return message;
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return "(Midagi läks valesti sõnumi laadimisel.)";
            }
            catch (OleDbException ex)
            {
                Debug.Print(ex.ToString());
                return "(Midagi läks valesti sõnumi laadimisel.)";
            }
        }
    }
}
