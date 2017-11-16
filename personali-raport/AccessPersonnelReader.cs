using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    ==== MS Access Table Structure ====

    This class reads data from Access using a SELECT query.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: Yksus
        Columns:
            - ID            created automatically (integer primary key)
            - Eesnimi       Text
            - Perekonnanimi Text
            - Ametikoht     Text
            - Auaste        Text
            - Kompanii      Text
            - KKV           Text (KKV või RES)
            - Isikukood     Text
            - Elukoht       Text
            - Telefon       Text
*/


namespace personali_raport
{
    class AccessPersonnelReader : IPersonnelReader
    {
        const string TABLE_NAME = "Yksus";
        
        const string FIRST_NAME_FIELD = "Eesnimi";
        const string LAST_NAME_FIELD = "Perekonnanimi";
        const string POSITION_FIELD = "Ametikoht";
        const string RANK_FIELD = "Auaste";
        const string COMPANY_FIELD = "Kompanii";
        const string PLATOON_FIELD = "Ryhm";
        const string KKV_FIELD = "KKV";
        const string ID_CODE_FIELD = "Isikukood";
        const string ADDRESS_FIELD = "Elukoht";
        const string PHONE_FIELD = "Telefon";
        const string KUTSE_FIELD = "Kutse";

        const string TABLE_QUERY = "SELECT * FROM " + TABLE_NAME + " WHERE " + ID_CODE_FIELD + " = @idCode;";

        private OleDbConnection databaseConnection;

        public AccessPersonnelReader(OleDbConnection oleDb)
        {
            Debug.Assert(oleDb != null, "OleDbConnection was null in AccessPersonnelReader constructor");
            databaseConnection = oleDb;
        }
        ///
        /// Read personal data by executing a SQL query against an Access database.
        /// Expects data to be in a table in the format specified in AccessPersonnelReader.cs
        /// <param name="idCode">The person's ID code (Isikukood).</param>
        /// <returns> a Person object filled with personal data.</returns>
        Person IPersonnelReader.ReadPersonalData(string idCode)
        {
            Person person = new Person() { idCode = idCode };
            Debug.Print("Looking for ID code {0}", idCode);

            var cursor = databaseConnection.CreateCommand();
            cursor.CommandText = TABLE_QUERY;
            cursor.Parameters.Add(new OleDbParameter("@isikukood", OleDbType.VarChar, 12));
            cursor.Parameters[0].Value = idCode;
            try
            {
                cursor.Prepare();
                using (OleDbDataReader reader = cursor.ExecuteReader())
                {
                    if (reader.HasRows) {
                        reader.Read();
                        string firstName = reader.GetString(reader.GetOrdinal(FIRST_NAME_FIELD));
                        string lastName = reader.GetString(reader.GetOrdinal(LAST_NAME_FIELD));
                        string company = reader.GetString(reader.GetOrdinal(COMPANY_FIELD)); // Kompanii
                        string platoon = reader.GetString(reader.GetOrdinal(PLATOON_FIELD)); // Rühm
                        int kutse = reader.GetInt32(reader.GetOrdinal(KUTSE_FIELD)); // Kutse

                        Debug.Print("AccessPersonnelReader fetch: " + idCode);
                        person.data.Add("Kompanii", company);
                        person.data.Add("Ryhm", platoon);
                        person.data.Add("Eesnimi", firstName);
                        person.data.Add("Perekonnanimi", lastName);
                        person.data.Add("Kutse", kutse.ToString());

                        return person;
                    }
                    Debug.Print("AccessPersonnelReader executed: no row found");
                    return null;
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                return null;
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Üksuse tabelit '" + TABLE_NAME + "' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
                return null;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (databaseConnection != null)
                {
                    databaseConnection.Dispose();
                    databaseConnection = null;
                }
            }
        }
    }
}
