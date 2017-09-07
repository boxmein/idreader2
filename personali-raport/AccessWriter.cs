using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Diagnostics;

namespace personali_raport
{
    /// <summary>
    /// A class that allows connecting to MS Access databases and logging data there.
    /// </summary>
    public class AccessWriter : IDisposable
    {
        
        /// <summary>
        ///  The name of the table that the ID card logs will be put into.
        ///  
        ///  Must have the following fields:
        ///     Eesnimi (string)
        ///     Perekonnanimi (string)
        ///     Isikukood (string)
        ///     Kellaaeg (Date & Time)
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
        /// Get the connection string needed to create the OLE DB connection to the Access DB.
        /// </summary>
        /// <param name="filename">The full path to the .accdb file: "C:\access\Database.accdb"</param>
        /// <returns>The connection string (OLE DB 12.0) with the correct Access DB.</returns>
        static string GetConnectionString(string filename)
        {
            return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", filename);
        }

        /// <summary>
        /// Creates the AccessWriter object, creates a database connection to the Access database
        /// specified by filename.
        /// </summary>
        /// <param name="filename">The full path filename of the Access Database, eg "C:\Access\Database.accdb"</param>
        /// <seealso cref="OleDbConnection" />
        public AccessWriter(string filename)
        {
            databaseConnection = new OleDbConnection(GetConnectionString(filename));
            databaseConnection.Open();
            Debug.Print("Database connected to: " + filename);
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
