using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Office.Interop.Excel;

namespace personali_raport
{
    public class PersonnelReader
    {
        /// <summary>
        /// What row the headers start for the actual data.
        /// </summary>
        const int HEADER_ROW = 14;
        /// <summary>
        /// What column the ID code is located on the Excel sheet.
        /// </summary>
        const char ID_CODE_COLUMN = 'N';

        static readonly string GROUP_DATA_NAME = "group";
        static readonly string GROUP_ROWNUM_DATA_NAME = "group#";

        const char FIRST_PERSONAL_COLUMN = 'A';
        const int MAX_PERSONAL_DATA = 20;
        /// <summary>
        /// Maximum amount of personnel rows to search for the ID code.
        /// </summary>
        const int MAX_PERSONNEL_ROWS = 100000;
        /// <summary>
        /// Keeps track of all found properties of the person.
        /// </summary>
        Dictionary<string, char> personProperties;

        Worksheet worksheet;
        Workbook workbook;
        Application excelApp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personnelReport">the file name for the isikutabel</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the Excel workbook failed to open for some reason. Most likely,
        /// the file doesn't exist.
        /// </exception>
        public PersonnelReader(string personnelReport)
        {
            excelApp = new Application();

            /*
            excelApp.Visible = true;
            excelApp.UserControl = true;
            */

            try
            {
                workbook = excelApp.Workbooks.Open(personnelReport);
                worksheet = excelApp.ActiveSheet;

                personProperties = new Dictionary<string, char>();
                FindPersonProperties(FIRST_PERSONAL_COLUMN, MAX_PERSONAL_DATA);
            } catch (Exception)
            {
                Debug.Print("could not load personnel file");
                throw new ArgumentException("personnel report could not be loaded");
            }
        }

        /* 
        ~PersonnelReader()
        {
            excelApp.Quit();
        } 
        */

        /// <summary>
        /// Go over the opened Excel table and find all columns by walking right from the first "personal property".
        /// Execution stops when the last column is found - when going right the next column is empty.
        /// </summary>
        /// <param name="startCol">the leftmost personal property column</param>
        /// <param name="count">the maximum amount of parameters we support</param>
        public void FindPersonProperties(char startCol, int count)
        {
            char key = startCol;
            string value;
            Debug.Print("Finding person properties starting from column " + key);

            for (int i = 0; i < count; i++)
            {
                value = GetValueFromCell(HEADER_ROW, key);
                Debug.Print("Cell " + key + "" + HEADER_ROW + " has data " + value);
                if (value == "") {
                    break;
                }
                personProperties.Add(value, key);
                key++;
            }
        }

        /// <summary>
        /// Find the column for a particular person by their ID code.
        /// The search ends when there isn't a real "person row" anymore 
        /// (the A column of this row is empty)
        /// </summary>
        /// <param name="idCode">The person's ID code (isikukood)</param>
        /// <param name="count">Max amount of rows to search downward.</param>
        /// <returns>the column number of the person, or -1 if not found.</returns>
        public int FindPersonRow(long idCode)
        {
            string tableIdCode;
            // figure out the end of the table by the last row that has something in A column
            string tableHead;   
            string idCodes = idCode.ToString();
            for (int i = 1; i < MAX_PERSONNEL_ROWS; i++)
            {
                tableHead = GetValueFromCell(HEADER_ROW + i, 'A');
                tableIdCode = GetValueFromCell(HEADER_ROW + i, ID_CODE_COLUMN);

                if (tableHead == "")
                {
                    break;
                }
                else if (tableIdCode == idCodes)
                {
                    return HEADER_ROW + i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Given this person's table row, find which grouping they belong to according to the (blue, bold)
        /// data tables.
        /// People are grouped under blue "grouping" rows in the personnel file, so we can look up the 
        /// first grouping row we can find above the person's record. Use this result for that:
        /// <see cref="FindPersonRow"/>
        /// </summary>
        /// <param name="personRow">What row the person is on.</param>
        /// <returns>The found group row, or -1 if not found.</returns>
        public int FindPersonRowGroup(int personRow)
        {
            bool bold;

            for (int i = personRow; i > 0; i--)
            {
                bold = worksheet.Cells[i, "A"].Font.Bold;
                Debug.Print("A{0}: style is {1}", i, bold);

                if (bold)
                {
                    Debug.Print("Found the person group: {0}", i);
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get a value from the Excel spreadsheet.
        /// </summary>
        /// <param name="row">Row starting from 1</param>
        /// <param name="column">Column starting from "A"</param>
        /// <returns> the Cell's FormulaLocal property (which seems to work as its value)</returns>
        /// <exception cref="ArgumentOutOfRangeException">Passing a value less than 1 as the row number. will fire this.</exception>
        private string GetValueFromCell(int row, string column)
        {
            if (row < 1)
            {
                throw new ArgumentOutOfRangeException("row", "Parameter 'row' cannot be less than 1");
            }
            return (string) worksheet.Cells[row, column].FormulaLocal;
        }

        /// <summary>
        /// Get a value from the Excel spreadsheet.
        /// </summary>
        /// <param name="row">Row starting from 1</param>
        /// <param name="column">Column starting from "A"</param>
        /// <returns> the Cell's FormulaLocal property (which seems to work as its value)</returns>
        /// <exception cref="ArgumentOutOfRangeException">Passing a value less than 1 as the row number. will fire this.</exception>
        private string GetValueFromCell(int row, char column)
        {
            if (row < 1)
            {
                throw new ArgumentOutOfRangeException("row", "Parameter 'row' cannot be less than 1");
            }
            return (string)worksheet.Cells[row, column.ToString()].FormulaLocal;
        }

        /// <summary>
        /// Return all personal data from the personnel file, based on a person's ID code.
        /// </summary>
        /// <param name="idCode">The person's ID code (isikukood)</param>
        /// <returns>A person with populated "data" object, or null if not found.</returns>
        public Person ReadPersonalData(long idCode)
        {
            Person person = new Person();
            person.idCode = idCode;

            int personRow = FindPersonRow(idCode);

            if (personRow == -1)
            {
                return null;
            }

            // Collect person properties
            foreach (KeyValuePair<string, char> kvp in personProperties)
            {
                person.data.Add(kvp.Key, GetValueFromCell(personRow, kvp.Value));
            }
             
            // Collect the person's group if possible
            int group = FindPersonRowGroup(personRow);

            if (group == -1)
            {
                Debug.Print("Could not figure out which group the person is in");
            } else
            {
                person.data.Add("group", GetValueFromCell(group, "A"));
                person.data.Add("group#", group.ToString());
            }

            return person;
        }
        
        public void CloseExcel()
        {
            excelApp.Quit();
        }
    }
}
