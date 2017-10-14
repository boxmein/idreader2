using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Diagnostics;

/*
    ==== MS Access Table Structure ====

    This class writes data into Access to the table called IDKaardid.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: IDKaardid
        Columns:
            - ID                created automatically (integer primary key)
            - Eesnimi           Text
            - Perekonnanimi     Text
            - Kellaaeg          Date/Time
            - Isikukood         Text
*/

namespace personali_raport
{
    /// <summary>
    /// A class that implements logging signins to a Microsoft Access table.
    /// </summary>
    public class AccessWriter : IDisposable
    {
        
        /// <summary>
        ///  The name of the table that the ID card logs will be put into.
        /// </summary>
        const string LOGS_TABLE_NAME = "IDKaardid";

        /// <summary>
        /// The INSERT statement to add a log, for use as a prepared statement.
        /// </summary>
        const string LOG_INSERT_STATEMENT = "INSERT INTO " + LOGS_TABLE_NAME + " (Eesnimi, Perekonnanimi, Isikukood, Kellaaeg) VALUES (?, ?, ?, ?);";

        /// <summary>
        /// Keeps track of the database connection for the Access database.
        /// </summary>
        OleDbConnection databaseConnection;

        /// <summary>
        /// Creates the AccessWriter object, creates a database connection to the Access database
        /// specified by filename.
        /// </summary>
        /// <param name="filename">An opened OLE DB Connection.</param>
        /// <seealso cref="OleDbConnection" />
        public AccessWriter(OleDbConnection connection)
        {
            databaseConnection = connection;
        }

        /// <summary>
        /// Add an entry to the ID card logs.
        /// Takes the first & last name and the ID code.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="idCode">The person's ID code.</param>
        /// <returns>true if the insert succeeded (and inserted 1 row), false if an InvalidOperationException happened.</returns>
        public bool log(string firstName, string lastName, string idCode)
        {
            var cursor = databaseConnection.CreateCommand();
            cursor.CommandText = LOG_INSERT_STATEMENT;
            cursor.Parameters.Add("Eesnimi", OleDbType.VarChar, 50);
            cursor.Parameters[0].Value = firstName;
            cursor.Parameters.Add("Perekonnanimi", OleDbType.VarChar, 50);
            cursor.Parameters[1].Value = lastName;
            cursor.Parameters.Add("Isikukood", OleDbType.VarChar, 12);
            cursor.Parameters[2].Value = idCode;
            cursor.Parameters.Add("Kellaaeg", OleDbType.Date, 4);
            cursor.Parameters[3].Value = DateTime.Now;

            try {
                cursor.Prepare();
                int returnValue = cursor.ExecuteNonQuery();
                Debug.Print("Cursor executed: {0} rows modified.", returnValue);
                return returnValue == 1;
            } catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return false;
            } catch (OleDbException ex)
            {
                Debug.Print(ex.ToString());
                return false;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    databaseConnection.Close();
                    databaseConnection.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AccessWriter() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
