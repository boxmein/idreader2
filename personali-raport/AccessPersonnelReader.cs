using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    ==== MS Access Table Structure ====

    This class reads data from Access using a SELECT query.

    This is how to create the expected table structure in Access:

    Access: 
    Create Table >
        Name: Raffas
        Columns:
            - ID            created automatically (integer primary key)
            - Eesnimi       Text
            - Perekonnanimi Text
            - Ametikoht     Text
            - Auaste        Text
            - Grupp         Text
            - KKV           Text (KKV või RES)
            - Isikukood     Text
            - Elukoht       Text
            - Telefon       Text
*/


namespace personali_raport
{
    class AccessPersonnelReader : IPersonnelReader
    {
        const string TABLE_NAME = "Raffas";
        
        const string FIRST_NAME_FIELD = "Eesnimi";
        const string LAST_NAME_FIELD = "Perekonnanimi";
        const string POSITION_FIELD = "Ametikoht";
        const string RANK_FIELD = "Auaste";
        const string GROUP_FIELD = "Grupp";
        const string KKV_FIELD = "KKV";
        const string ID_CODE_FIELD = "Isikukood";
        const string ADDRESS_FIELD = "Elukoht";
        const string PHONE_FIELD = "Telefon";

        const string TABLE_QUERY = "SELECT * FROM " + TABLE_NAME + " WHERE " + ID_CODE_FIELD + " = ?;";


        private OleDbConnection databaseConnection;

        public AccessPersonnelReader(OleDbConnection oleDb)
        {
            Debug.Assert(oleDb != null, "OleDbConnection was null in AccessPersonnelReader constructor");
            databaseConnection = oleDb;
        }
        Person IPersonnelReader.ReadPersonalData(string idCode)
        {
            Person person = new Person();
            person.idCode = idCode;

            var cursor = databaseConnection.CreateCommand();
            cursor.CommandText = TABLE_QUERY;
            cursor.Parameters.Add(new OleDbParameter("Isikukood", OleDbType.VarChar, 12));
            cursor.Parameters[0].Value = idCode;
            try
            {
                cursor.Prepare();
                OleDbDataReader reader = cursor.ExecuteReader();

                if (reader.HasRows)
                {

                }
                Debug.Print("AccessPersonnelReader fetch: " + idCode);
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

            return person;
        }
    }
}
