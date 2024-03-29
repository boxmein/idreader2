﻿using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;

namespace personali_raport
{
    class AttendanceReportWriter : IReportWriter
    {

        const int START_ROW = 2;

        const string NAME_COL = "B";
        const string RANK_COL = "C";
        const string REMARK_COL = "D";
        
        Worksheet worksheet;
        Workbook workbook;
        Application excelApp;

        int currentRow = 1;

        /// <summary>
        /// Instantiates a new PersrepReportWriter along with opening the report 
        /// template.
        /// </summary>
        /// <param name="fileName">
        /// The report template file. This is opened and saved-as a new report.
        /// </param>
        public AttendanceReportWriter(string fileName)
        {
            excelApp = new Application();

            fileName = Path.GetFullPath(fileName);
            /// NOTE: COMExceptions here can be caused from a wrong installation of Excel.
            /// Install a clean Office 2010 and uninstall everything else, then reboot to fix.
            /// Also, don't use Office 16.0 Object Library.
            workbook = excelApp.Workbooks.Open(fileName);
            worksheet = excelApp.ActiveSheet;
        }

        public bool SaveFile(string fileName)
        {
            worksheet.SaveAs(fileName);
            excelApp.Quit();
            return true;
        }

        public bool WriteReport(List<AttendanceItem> personnel)
        {
            if (personnel.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Kohalolekukontrolli koostamise jaoks ei leitud logidest ühtki sobivat tulemust.", "Raporti ei saanud koostada");
                return false;
            }
            
            currentRow = START_ROW;
            foreach (var person in personnel)
            {
                SetValueToCell(currentRow, NAME_COL, person.name);
                SetValueToCell(currentRow, RANK_COL, person.platoon);
                currentRow += 1;
            }
            return true;
        }

        public void HandleUnknownPeople(List<Person> personnel)
        {
            foreach(var person in personnel)
            {
                SetValueToCell(currentRow, NAME_COL, person.data["Eesnimi"] + " " + person.data["Perekonnanimi"]);
                SetValueToCell(currentRow, REMARK_COL, "Tundmatu: " + person.data["Isikukood"]);
                currentRow += 1;
            }
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

        public void CloseExcel()
        {
            try
            {
                excelApp.Quit();
            }
            catch (COMException)
            {
                Debug.Print("Failed to close Excel app.");
            }
        }

        public bool WriteReport(List<PersrepItem> personnel)
        {
            Debug.Write("AttendanceReportWriter cannot use PersrepItems");
            throw new NotImplementedException();
        }
    }
}
