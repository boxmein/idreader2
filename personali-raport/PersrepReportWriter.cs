using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;
using System.IO;

using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using DialogResult = System.Windows.Forms.DialogResult;
using static personali_raport.AccessLogReader;
using System.Reflection;
using System.Runtime.InteropServices;

namespace personali_raport
{
    
    /// <summary>
    /// A class responsible for generating PERSREP reports.
    /// Depends on the PERSREP template provided. 
    /// The cell positions etc are hardcoded, so any drastic changes to the PERSREP format
    /// will mean changes to this program.
    /// </summary>
    public class PersrepReportWriter : IReportWriter
    {
        // Ainsad kohad mida see PERSREP raportija muudab on segmente A ja M (märkmed ja andmed). 
        // Ülejäänu jaoks on olemas Exceli valemid.
        const int PERSREP_MAX_GROUPS = 14;
        const int PERSREP_FIRST_GROUP_ROW = 10;
        const int PERSREP_GROUP_SIZE = 6;

        const string PERSREP_GROUP_COL = "B";
        const string PERSREP_DATA_COL = "F";

        const int PERSREP_OFFICER_ROW = 0;
        const int PERSREP_SUBOFFICER_ROW = 1;
        const int PERSREP_PRIVATE_ROW = 2;
        const int PERSREP_CIVILIAN_ROW = 3;

        const string PERSREP_UNGROUPED = "Liigitamata";

        // Actual is (M84) 24R x 10C but keep space for title and remark
        const int PERSREP_REMARKS_START_ROW = 86;
        const int PERSREP_REMARKS_HEIGHT = 22;
        const int PERSREP_REMARKS_WIDTH = 10;
        const string PERSREP_REMARKS_START_COL = "M";
        const string PERSREP_REMARKS_NAME_COL = "N";
        
        Worksheet worksheet;
        Workbook workbook;
        Application excelApp;

        /// <summary>
        /// Instantiates a new PersrepReportWriter along with opening the report 
        /// template.
        /// </summary>
        /// <param name="fileName">
        /// Report base filename.
        /// </param>
        public PersrepReportWriter(string fileName)
        {
            Debug.Assert(fileName != null, "PERSREP Report Writer filename was null");
            Debug.Assert(File.Exists(fileName), "PERSREPReportWriter filename was not a real file");
            excelApp = new Application();
            excelApp.DisplayAlerts = false;
            fileName = Path.GetFullPath(fileName);
            workbook = excelApp.Workbooks.Open(fileName);
            worksheet = excelApp.ActiveSheet;
        }

        /// <summary>
        /// Create the report.
        /// Asks the user for the report template and fills it up according to which report type was specified.
        /// </summary>
        /// <param name="personnel">The people that this report will include.</param>
        public bool WriteReport(List<PersrepItem> personnelCounts)
        {

            // 
            // Count groups, make sure we have enough Excel cell groups to cover them all
            // int personnelGroupCount = personnelByGroup.Keys.Count;
            
            
            if (personnelCounts.Count > PERSREP_MAX_GROUPS)
            {
                if (MessageBox.Show("Kogutud andmetes esineb üle 14 allüksuse liikmeid. Laiendan malli allapoole, kuid siis tuleb lõpus üldsummade funktsioonid käsitsi ümber muuta.\r\n\r\nVajuta OK et jätkata ja Cancel et lõpetada.", "Viga malli täitmisel", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return false;
                }

                int extraGroups = personnelCounts.Count - PERSREP_MAX_GROUPS;

                Debug.Print("Copying over {0} new groups", extraGroups);

                // copy the summary rows over
                Range sumRows = worksheet.Range["A94", "J106"];
                Range newRange = worksheet.Range[('A' + (94 + extraGroups * PERSREP_GROUP_SIZE)).ToString(), ('J' + (106 + extraGroups * PERSREP_GROUP_SIZE)).ToString()];
                sumRows.Copy(newRange);

                Range donorRows = worksheet.Range["B10", "J15"];

                for (int i = 0; i < extraGroups; i++)
                {
                    donorRows.Copy(worksheet.Range[('A' + (94 + i * PERSREP_GROUP_SIZE)).ToString(),('J' + (94 + (i + 1) * PERSREP_GROUP_SIZE)).ToString()]);
                }
            }
            

            //
            // Walk over each personnel group and write data into their group cells
            int currentGroup = 0;

            foreach (var count in personnelCounts)
            {
                var ohvitserid = count.ohvitsere;
                var allohvitserid = count.allohvitsere;
                var sodurid = count.sodureid;
                var tsiviilid = count.tsiviliste;

                Debug.Print("grupp {4}: ohvitsere {0}, allohvitsere {1}, sõdureid {2}, tsiviile {3}",
                        ohvitserid, allohvitserid, sodurid, tsiviilid, count.company);
                
                // Write O/AO/S/- counts
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_OFFICER_ROW, PERSREP_DATA_COL, ohvitserid.ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_SUBOFFICER_ROW, PERSREP_DATA_COL, allohvitserid.ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_PRIVATE_ROW, PERSREP_DATA_COL, sodurid.ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_CIVILIAN_ROW, PERSREP_DATA_COL, tsiviilid.ToString());

                // Write group name
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE, PERSREP_GROUP_COL, count.company);

                // B: Ajateenijad - ei pea

                currentGroup += 1;
            }

            return true;
        }

        public bool SaveFile(string fileName)
        {
            worksheet.SaveAs(fileName);
            excelApp.Quit();
            return true;
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
                Debug.Print("Excel app already closed?");
            }
            catch (Exception)
            {
                Debug.Print("Failed to close Excel app");
            }
        }

        public bool WriteReport(List<AttendanceItem> personnel)
        {
            Debug.Assert(false, "PersrepReportWriter cannot use AttendanceItems");
            throw new NotImplementedException();
        }

        public void HandleUnknownPeople(List<Person> personnel)
        {
            int startRow = PERSREP_REMARKS_START_ROW;
            foreach (var person in personnel)
            {
                SetValueToCell(startRow, PERSREP_REMARKS_START_COL, "Tundmatu: " + person.data["Isikukood"]);
                SetValueToCell(startRow, PERSREP_REMARKS_NAME_COL, person.data["Eesnimi"] + " " + person.data["Perekonnanimi"]);
            }
        }
    }
}
