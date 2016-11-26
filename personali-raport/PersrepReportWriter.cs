using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Office.Interop.Excel;
using System.Linq;
using System.Diagnostics;

using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using DialogResult = System.Windows.Forms.DialogResult;

namespace personali_raport
{
    public class PersrepReportWriter : IReportWriter
    {
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
        const char PERSREP_REMARKS_START_COL = 'M';

        // A: tegevväelased + personal - touchy
        // B: ajateenijad    - no touchy
        // C: reservväelased - no touchy
        // D: KL tegevliikmed (mitte KVK) - no touchy
        // E: Allies  - no touchy
        // F: Summary - no touchy
        // G: Märkmed: M84:V106 - touchy. Tundmatute kirjete maa

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
        public bool WriteReport(List<Person> personnel)
        {

            Dictionary<string, List<Person>> personnelByGroup = new Dictionary<string, List<Person>>();

            string tempDataValue;
            string groupIndexStr;
            int groupIndex;
            
            // 
            // Collect all personnel into groups: 
            // { "group": [ person... ] }, where keys are ordered based on Excel
            foreach (Person person in personnel)
            {
                if (!person.data.TryGetValue("group", out tempDataValue))
                {
                    tempDataValue = PERSREP_UNGROUPED;
                }

                if (!personnelByGroup.ContainsKey(tempDataValue))
                {
                    if (!person.data.TryGetValue("group#", out groupIndexStr))
                    {
                        groupIndex = 0;
                    }
                    else
                    {
                        if (!Int32.TryParse(groupIndexStr, out groupIndex))
                        {
                            groupIndex = 0;
                        }
                    }

                    personnelByGroup[tempDataValue] = new List<Person>();
                }
                personnelByGroup[tempDataValue].Add(person);
            }

            // 
            // Count groups, make sure we have enough Excel cell groups to cover them all
            int personnelGroupCount = personnelByGroup.Keys.Count;
            
            if (personnelGroupCount > PERSREP_MAX_GROUPS)
            {
                if (MessageBox.Show("Kogutud andmetes esineb üle 14 allüksuse liikmeid. Laiendan malli allapoole, kuid siis tuleb lõpus üldsummade funktsioonid käsitsi ümber muuta.\r\n\r\nVajuta OK et jätkata ja Cancel et lõpetada.", "Viga malli täitmisel", MessageBoxButtons.OKCancel) != DialogResult.OK)
                {
                    return false;
                }

                int extraGroups = personnelGroupCount - PERSREP_MAX_GROUPS;

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

            foreach (KeyValuePair<string, List<Person>> personnelGroup in personnelByGroup.OrderBy(x => x.Value[0].data["group#"]))
            {
                List<Person> personnelGrp = personnelGroup.Value;

                // Tabel A: Tegevväelased ja tsiviilpersonal
                var ohvitserid = personnelGrp.Where(person => {
                    return person.data.TryGetValue("KKV OHV", out tempDataValue) && tempDataValue == "1";
                });

                var allohvitserid = personnelGrp.Where(person => {
                    return person.data.TryGetValue("KKV ALLOHV", out tempDataValue) && tempDataValue == "1";
                });

                var sodurid = personnelGrp.Where(person => {
                    return person.data.TryGetValue("KKV SÕDUR", out tempDataValue) && tempDataValue == "1";
                });

                var tsiviilid = personnelGrp.Where(person => {
                    return person.data.TryGetValue("RES OHV", out tempDataValue) && tempDataValue == "1" ||
                           person.data.TryGetValue("RES ALLOHV", out tempDataValue) && tempDataValue == "1" ||
                           person.data.TryGetValue("RES SÕDUR", out tempDataValue) && tempDataValue == "1" ||
                           person.data.TryGetValue("mitte KVK", out tempDataValue) && tempDataValue == "1";
                });

                Debug.Print("grupp {4}: ohvitsere {0}, allohvitsere {1}, sõdureid {2}, tsiviile {3}",
                        ohvitserid.Count(), allohvitserid.Count(), sodurid.Count(),
                        tsiviilid.Count(), personnelGroup.Key);

                // Write O/AO/S/- counts
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_OFFICER_ROW, PERSREP_DATA_COL, ohvitserid.Count().ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_SUBOFFICER_ROW, PERSREP_DATA_COL, allohvitserid.Count().ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_PRIVATE_ROW, PERSREP_DATA_COL, sodurid.Count().ToString());
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE + PERSREP_CIVILIAN_ROW, PERSREP_DATA_COL, tsiviilid.Count().ToString());

                // Write group name
                SetValueToCell(PERSREP_FIRST_GROUP_ROW + currentGroup * PERSREP_GROUP_SIZE, PERSREP_GROUP_COL, personnelGroup.Key);

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
            catch (Exception)
            {
                Debug.Print("Failed to close Excel app");
            }

        }

        public bool HandleUnknownPeople(List<Person> unknownPeople)
        {
            if (unknownPeople.Count > 0)
            {
                SetValueToCell(85, "M", "Järgnevate inimeste isikukoode on nähtud, kuid puuduvad üksuse tabelist:");
                int columnOffset = 0;
                int rowOffset = 0;
                bool outsideRemarksBox = false;

                foreach (var person in unknownPeople)
                {
                    SetValueToCell(PERSREP_REMARKS_START_ROW + rowOffset, 
                                  ((char) (PERSREP_REMARKS_START_COL + columnOffset)).ToString(), 
                                  person.data["Eesnimi"] + " " + person.data["Perekonnanimi"] + "; " + person.idCode);

                    rowOffset += 1;
                    if (rowOffset > PERSREP_REMARKS_HEIGHT)
                    {
                        rowOffset = 0;
                        columnOffset += 1;
                    }
                    if (columnOffset > PERSREP_REMARKS_WIDTH && !outsideRemarksBox)
                    {
                        if (MessageBox.Show("Liiga palju inimesi, keda pole tuvastatud. Ülejäänud kirjed jooksevad märkuste kastist välja.\r\n\r\nVajutage OK et jätkata ja Cancel et lõpetada.", "Viga tundmatute kirjete näitamisel raportis.", MessageBoxButtons.OKCancel) != DialogResult.OK) {
                            break;
                        }
                        outsideRemarksBox = true;
                    }
                }

                return true;
            }
            return false;
        }
    }
}
