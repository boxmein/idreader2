using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;

namespace personali_raport
{
    enum LoggerState { Initial, CollectingData };
    enum ReporterState { OpenPersonnelList, SelectDataFile, SelectDateRange, OpenReportTemplate, LoadingData, SaveReport, Done };

    public partial class Form1 : Form
    {
        ReportSettings settings;
        ReporterState reporterState = ReporterState.OpenPersonnelList;

        IReportWriter reportWriter;
        Panel[] reporterPanels = null;
        
        public Form1()
        {
            settings = new ReportSettings();
            InitializeComponent();
        }

        public void UpdateReportMenu()
        { 
            /*
            foreach (Panel r in reporterPanels)
            {
                r.Visible = false;
            }

            switch (reporterState)
            {
                case ReporterState.OpenPersonnelList:
                    openPersonnelListPanel.Visible = true;
                    break;
                case ReporterState.SelectDataFile:
                    openDataFilesPanel.Visible = true;
                    break;

                case ReporterState.SelectDateRange:
                    selectDataPanel.Visible = true;
                    break;

                case ReporterState.LoadingData:
                    generatingReportPanel.Visible = true;
                    break;

                case ReporterState.OpenReportTemplate:
                    openReportTemplatePanel.Visible = true;
                    break;

                case ReporterState.SaveReport:
                    saveReportPanel.Visible = true;
                    break;

                case ReporterState.Done:
                    break;

                default:
                    break;
            } 
            */
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            reporterPanels = new Panel[] { openPersonnelListPanel, openDataFilesPanel,
                selectDataPanel, generatingReportPanel, openReportTemplatePanel,
                saveReportPanel };

            UpdateReportMenu();

            settings.startOfReport = dataSelectionStartDate.Value;
            settings.endOfReport = dataSelectionEndDate.Value;
        }

        private void GenerateReport()
        {
            PersonnelReader personnelReader = new PersonnelReader(settings.personnelFileName);
            CardLogReader cardLogReader = new CardLogReader();
            List<Person> logEntries = new List<Person>();

            int entriesNotRecognized = 0;
            int entriesIterated = 0;
            

            foreach (IEnumerable<CardLogEntry> rows in cardLogReader.LoadAllFiles(settings.dataFiles))
            {
                foreach (CardLogEntry row in rows)
                {
                    reportProgress.Value = entriesIterated * 100 / settings.dataFiles.Length;

                    if (row == null)
                    {
                        Debug.Print("Did not receive a valid row in GenerateReport() :(");
                        continue;
                    }
                    if (settings.startOfReport < row.datetime &&
                        settings.endOfReport > row.datetime)
                    {
            
                        // null if no person was found
                        Person foundPerson = personnelReader.ReadPersonalData(row.idCode);

                        if (foundPerson == null)
                        {
                            entriesNotRecognized++;
                            continue;
                        }

                        foundPerson.signedInOn = row.datetime;
                        logEntries.Add(foundPerson);
                    }
                }
            }
        
            // Count has capital C because he's important
            Debug.Print("Found {0} entries (of which {1} were not translated to persons)", logEntries.Count, entriesNotRecognized);

            if (settings.reportType == ReportType.PERSREP)
            {
                reportWriter = new PersrepReportWriter(settings.reportTemplate);
                reportWriter.WriteReport(logEntries);
            }
        }

        private void openPersonnelListBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel spreadsheets|*.xlsx";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.personnelFileName = ofd.FileName;
                // forwardButton.Enabled = true;
            }
        }

        private void openDataFilesBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Comma-separated values|*.csv";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.dataFiles = ofd.FileNames;
                numberFilesOpen.Text = "Valitud " + ofd.FileNames.Length + " fail" + (ofd.FileNames.Length > 1 ? "i" : "");
                // forwardButton.Enabled = true;
            }
        }

        private void generatePersrepBtn_Click(object sender, EventArgs e)
        {
            settings.reportType = ReportType.PERSREP;
            reporterState = ReporterState.LoadingData;
            UpdateReportMenu();
            GenerateReport();
        }

        private void generateMidrepBtn_Click(object sender, EventArgs e)
        {
            settings.reportType = ReportType.PERSREP;
            reporterState = ReporterState.LoadingData;
            UpdateReportMenu();
            GenerateReport();
        }

        private void generateAttendanceBtn_Click(object sender, EventArgs e)
        {
            settings.reportType = ReportType.ATTENDANCE;
            reporterState = ReporterState.LoadingData;
            UpdateReportMenu();
            GenerateReport();
        }

        private void dataSelectionStartDate_ValueChanged(object sender, EventArgs e)
        {
            settings.startOfReport = dataSelectionStartDate.Value;
        }

        private void dataSelectionEndDate_ValueChanged(object sender, EventArgs e)
        {
            settings.endOfReport = dataSelectionEndDate.Value;
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            switch (reporterState)
            {
                case ReporterState.OpenPersonnelList:
                    reporterState = ReporterState.SelectDataFile;
                    break;
                case ReporterState.SelectDataFile:
                    reporterState = ReporterState.SelectDateRange;
                    break;
                case ReporterState.SelectDateRange:
                    reporterState = ReporterState.OpenReportTemplate;
                    break;
                // after report template has been picked, start creating data
                case ReporterState.OpenReportTemplate:
                    reporterState = ReporterState.LoadingData;
                    break;
            }
            forwardButton.Enabled = false;
            UpdateReportMenu();
        }

        private void openPersrepFileBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel spreadsheets|*.xlsx";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.reportTemplate = ofd.FileName;
                // forwardButton.Enabled = true;
            } 
        }

        private void saveReportButton_Click(object sender, EventArgs e)
        {

            var sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.CheckPathExists = true;
            sfd.FileName = "raport-" + settings.startOfReport.ToString("dd.MM.yyyy-HH.mm.ss") + ".xlsx";
            sfd.Filter = "Excel spreadsheet|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                reportWriter.SaveFile(sfd.FileName);
            }
        }
    }
}
