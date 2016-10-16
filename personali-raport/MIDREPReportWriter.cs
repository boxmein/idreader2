using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;


namespace personali_raport
{
    class MIDREPReportWriter : IReportWriter
    {

        Worksheet worksheet;
        Workbook workbook;
        Application excelApp;

        /// <summary>
        /// Instantiates a new PersrepReportWriter along with opening the report 
        /// template.
        /// </summary>
        /// <param name="fileName">The Excel spreadsheet file name that the report will be based on.</param>
        public MIDREPReportWriter(string fileName)
        {
            excelApp = new Application();

            /*
            excelApp.Visible = true;
            excelApp.UserControl = true;
            */

            workbook = excelApp.Workbooks.Open(fileName);
            worksheet = excelApp.ActiveSheet;
        }

        public void SaveFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public void WriteReport(List<Person> personnel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set a value to a cell in the Excel spreadsheet.
        /// </summary>
        /// <param name="row">Row starting from 1</param>
        /// <param name="column">Column starting from "A"</param>
        /// <param name="value">Value to assign to the cell</param>
        /// <exception cref="ArgumentOutOfRangeException">Passing a value less than 1 as the row number. will fire this.</exception>
        private void SetValueToCell(int row, string column, string value)
        {
            if (row < 1)
            {
                throw new ArgumentOutOfRangeException("row", "Parameter 'row' cannot be less than 1");
            }

            if (value == null)
            {
                throw new ArgumentNullException();
            }
            worksheet.Cells[row, column].FormulaLocal = value;
        }
    }
}
