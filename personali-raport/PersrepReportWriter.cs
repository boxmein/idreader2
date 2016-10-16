using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;

namespace personali_raport
{
    public class PersrepReportWriter : IReportWriter
    {

        Worksheet worksheet;
        Workbook workbook;
        Application excelApp;

        /// <summary>
        /// Instantiates a new PersrepReportWriter along with opening the report 
        /// template.
        /// </summary>
        /// <param name="fileName">The Excel spreadsheet file name that the report will be based on.</param>
        public PersrepReportWriter(string fileName)
        {
            excelApp = new Application();

            /*
            excelApp.Visible = true;
            excelApp.UserControl = true;
            */

            workbook = excelApp.Workbooks.Open(fileName);
            worksheet = excelApp.ActiveSheet;
        }

        /// <summary>
        /// Create the report.
        /// Asks the user for the report template and fills it up according to which report type was specified.
        /// </summary>
        /// <param name="personnel">The people that this report will include.</param>
        public void WriteReport(List<Person> personnel)
        {
            // Tabel A: Tegevväelased ja tsiviilpersonal

            var ohvitserid = personnel.Where(person => {
                var kategooria = person.data["KAT O/AO/S/-"];
                return kategooria?.ToUpper() == "O";
            });

            var allohvitserid = personnel.Where(person => {
                var kategooria = person.data["KAT O/AO/S/-"];
                return kategooria?.ToUpper() == "AO";
            });

            var sodurid = personnel.Where(person => {
                var kategooria = person.data["KAT O/AO/S/-"];
                return kategooria?.ToUpper() == "S";
            });

            var tsiviilid = personnel.Where(person => {
                var kategooria = person.data["KAT O/AO/S/-"];
                return kategooria != "O" && kategooria != "AO";
            });

            Debug.Print("ohvitsere {0}, allohvitsere {1}, sõdureid {2}, tsiviile {3}",
                ohvitserid.Count(), allohvitserid.Count(), sodurid.Count(),
                tsiviilid.Count());

            SetValueToCell(10, "F", ohvitserid.Count().ToString());
            SetValueToCell(11, "F", allohvitserid.Count().ToString());
            SetValueToCell(12, "F", sodurid.Count().ToString());
            SetValueToCell(13, "F", tsiviilid.Count().ToString());
        }

        public void SaveFile(string fileName)
        {
            worksheet.SaveAs(fileName);
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
