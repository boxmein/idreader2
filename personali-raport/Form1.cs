using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Data.OleDb;

namespace personali_raport
{
    enum ReporterState {
            OpenPersonnelList = 0,
            SelectDataFile = 1,
            SelectDateRange = 2,
            OpenReportTemplate = 3,
            LoadingData = 4,
            SaveReport = 5,
            Done = 6
    };

    public partial class Form1 : Form
    {
        /// <summary>
        /// Allows access to write to a MS Access database. (ha, get it)
        /// </summary>
        AccessWriter writer;

        /// <summary>
        /// Reader for the ID Collector to get personal messages from the database.
        /// </summary>
        PersonMessageReader pmReader;

        /// <summary>
        /// The user-provided settings to compose the report from.
        /// This contains the files to base the report on, plus the report type
        /// (PERSREP or Attendance), plus the date limits.
        /// </summary>
        ReportSettings settings;

        
        /// <summary>
        /// The Report Writer that will be used to compose the report.
        /// </summary>
        IReportWriter reportWriter;

        /// <summary>
        /// A Form that shows up when the ID card reader is collecting data.
        /// Shows the current person's name, ID code and personal message.
        /// Uses the AccessWriter and PersonMessageReader.
        /// </summary>
        IDCollectorForm idCollectorForm;
        
        /// <summary>
        /// The MS Access DB connection used to read and write. Created by the user when they open the database file.
        /// Passed to IDCollectorForm for further passing to AccessWriter & PersonMessageReader.
        /// </summary>
        OleDbConnection conn;

        bool hasIDReaderBackend = false;

        /// <summary>
        /// Constructs the Form.
        /// Initializes the ReportSettings object.
        /// </summary>
        /// <seealso cref="ReportSettings"/>
        public Form1()
        {
            settings = new ReportSettings();
            InitializeComponent();
        }
        
        /// <summary>
        /// Initializes the ID card reader:
        /// Fills in the report settings with their default values.
        /// Shows the "Start data collection" UI.
        /// Hides the "Data collection progress" UI.
        /// Sets start & end of report to time filter settings or start/end of time.
        /// Sets up the subprocess structure for idreader2 (./idreader2.exe), and adds hooks.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize settings
            settings.reportType = ReportType.PERSREP;
            settings.personnelFileName = null;
            settings.reportFileName = null;
            settings.reportTemplate = null;

            startDataCollectionPanel.Visible = true;
            dataCollectionProgressPanel.Visible = false;

            // Initialize time filter settings
            if (timeFilterEnabledCheckbox.Checked)
            {
                settings.startOfReport = dataSelectionStartDate.Value;
                settings.endOfReport = dataSelectionEndDate.Value;
            }
            else {
                settings.startOfReport = DateTime.MinValue;
                settings.endOfReport = DateTime.MaxValue;
            }

            // If we have actually got a logger program, we can allow user to control it
            if (File.Exists("idreader2.exe"))
            {
                Debug.Print("Found idreader2.exe");
                hasIDReaderBackend = true;
            }
            else
            {
                Debug.Print("Missing CWD/idreader2.exe, cannot logger");
                hasIDReaderBackend = false;
                loggerErrorLabel.Text = "Viga: puudub vajalik programm, et ID-kaardi andmeid koguda.";
                loggerErrorLabel.Visible = true;
            }
        }

        /// <summary>
        /// Validate the selected options and decide whether we can generate the required
        /// report.
        /// Sets generatePersrepBtn.Enabled to true if everything is valid:
        /// - the user has selected logs to include in the report
        /// - each of the log files exists
        /// - start & end of report are not null
        /// - personnel file name & report template file name are not null and the files exist
        /// </summary>
        private void UpdateValidity()
        {
            if (settings.dataFiles != null && settings.dataFiles.Length > 0 &&
                settings.dataFiles.All(dataFile => File.Exists(dataFile)) &&
                settings.startOfReport != null &&
                settings.endOfReport != null &&
                settings.personnelFileName != null && File.Exists(settings.personnelFileName) &&
                settings.reportTemplate != null && File.Exists(settings.reportTemplate))
            {
                generatePersrepBtn.Enabled = true;
            }
            else
            {
                generatePersrepBtn.Enabled = false;
            }

            progressStatusLabel.Text = "";
        }

        /// <summary>
        /// When the process closes, close Excel with it.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void OnExit(object sender, EventArgs e)
        {
            if (reportWriter != null)
            {
                reportWriter.CloseExcel();
            }
        }

        /// <summary>
        /// Collect personnel file data and feed them to the ReportWriter.
        /// </summary>
        /// <seealso cref="IReportWriter"/>
        /// <seealso cref="PersrepReportWriter"/>
        /// <seealso cref="AttendanceReportWriter"/>
        private void GenerateReport()
        {
            ExcelPersonnelReader personnelReader = new ExcelPersonnelReader(settings.personnelFileName);
            CardLogReader cardLogReader = new CardLogReader();
            List<Person> logEntries = new List<Person>();
            List<Person> unknownPeople = new List<Person>();

            int entriesNotRecognized = 0;
            
            foreach (IEnumerable<CardLogEntry> rows in cardLogReader.LoadAllFiles(settings.dataFiles))
            {
                foreach (CardLogEntry row in rows)
                {

                    if (row == null)
                    {
                        Debug.Print("Did not receive a valid row in GenerateReport() :(");
                        continue;
                    }
                    
                    if (settings.startOfReport < row.datetime &&
                        settings.endOfReport > row.datetime)
                    {
                        Debug.Print("Processing person with ID code {0}", row.idCode);

                        // null if no person was found
                        Person foundPerson = personnelReader.ReadPersonalData(row.idCode);

                        if (foundPerson == null)
                        {
                            Debug.Print("Unknown person, adding to unknown list", row.idCode);

                            var p = new Person() { idCode = row.idCode };
                            p.data["Eesnimi"] = row.firstName;
                            p.data["Perekonnanimi"] = row.lastName;
                            unknownPeople.Add(p);

                            entriesNotRecognized++;
                            continue;
                        }

                        foundPerson.signedInOn = row.datetime;
                        logEntries.Add(foundPerson);
                    }
                }
            }

            progressStatusLabel.Text = String.Format("Edukalt loetud {0} kirjet.", logEntries.Count);
            if (entriesNotRecognized > 0)
            {
                progressStatusLabel.Text += String.Format(" Ei suutnud tuvastada {0} kirjet.", entriesNotRecognized);
            }
            Debug.Print("Found {0} entries (and {1} were not translated to persons)", logEntries.Count, entriesNotRecognized);
            
            if (reportWriter != null)
            {
                reportWriter.CloseExcel();
                reportWriter = null;
            }

            if (settings.reportType == ReportType.PERSREP)
            {
                reportWriter = new PersrepReportWriter(settings.reportTemplate);
            } else if (settings.reportType == ReportType.ATTENDANCE)
            {
                reportWriter = new AttendanceReportWriter(settings.reportTemplate);
                
            } 


            personnelReader.CloseExcel();
            personnelReader = null;

            reportWriter.WriteReport(logEntries);
            if (unknownPeople.Count > 0)
            {
                reportWriter.HandleUnknownPeople(unknownPeople);
            }

            saveReportButton.Enabled = true;
        }

        /// <summary>
        /// Get the connection string needed to create the OLE DB connection to the Access DB.
        /// </summary>
        /// <param name="filename">The full path to the .accdb file: "C:\access\Database.accdb"</param>
        /// <returns>The connection string (OLE DB 12.0) with the correct Access DB.</returns>
        static string GetConnectionString(string filename)
        {
            return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}", filename);
        }

        #region Form Event Handlers
        private void dataSelectionStartDate_ValueChanged(object sender, EventArgs e)
        {
            settings.startOfReport = dataSelectionStartDate.Value;
        }
        private void dataSelectionEndDate_ValueChanged(object sender, EventArgs e)
        {
            settings.endOfReport = dataSelectionEndDate.Value;
        }
        private void generatePersrepBtn_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }
        private void saveReportButton_Click(object sender, EventArgs e)
        {

            var sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.CheckPathExists = true;

            var datetime = settings.startOfReport;

            // If time filtering is turned off, startOfReport is DateTime.MinValue
            if (settings.startOfReport == DateTime.MinValue)
            {
                datetime = DateTime.Now;
            }

            sfd.FileName = "raport-" + datetime.ToString("dd.MM.yyyy-HH.mm.ss") + ".xlsx";
            sfd.Filter = "Excel spreadsheet|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                progressStatusLabel.Text = "Salvestan...";
                reportWriter.SaveFile(sfd.FileName);
                progressStatusLabel.Text = "Raport on salvestatud! Vali veel andmeid või teist tüüpi mall.";
            }
        }
        private void timeFilterEnabledCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            dataSelectionStartDate.Enabled = dataSelectionEndDate.Enabled = timeFilterEnabledCheckbox.Checked;

            if (timeFilterEnabledCheckbox.Checked)
            {
                settings.startOfReport = dataSelectionStartDate.Value;
                settings.endOfReport = dataSelectionEndDate.Value;
            } else {
                settings.startOfReport = DateTime.MinValue;
                settings.endOfReport = DateTime.MaxValue;
            }
        }
        private void reportOptionPersrep_CheckedChanged(object sender, EventArgs e)
        {
            if (reportOptionPersrep.Checked)
            {
                settings.reportType = ReportType.PERSREP;
                Debug.Print("Report type is now PERSREP");
            }
        }
        private void reportOptionAttendance_CheckedChanged(object sender, EventArgs e)
        {
            if (reportOptionAttendance.Checked)
            {
                settings.reportType = ReportType.ATTENDANCE;
                Debug.Print("Report type is now Attendance");
            }
        }

        private void clearPersonMsgFile_Click(object sender, EventArgs e)
        {
            openPersonMsgFileBtn.Visible = true;
            clearPersonMsgFile.Visible = false;

            personalMsgFileLabel.Text = "";
            personalMsgFileLabel.Visible = false;

            personMsgLabel.Visible = false;
        }
        private void openDatabaseButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Access databases|*.accdb";
            
            if (ofd.ShowDialog() == DialogResult.OK && !ofd.FileName.Equals(""))
            {
                try {
                    conn = new OleDbConnection(GetConnectionString(ofd.FileName));
                    conn.Open();
                    writer = new AccessWriter(conn);
                    pmReader = new PersonMessageReader(conn);

                    Debug.Print("Database connected to: " + ofd.FileName);
                    databaseConnectionErrorMsg.Visible = false;
                    openDatabaseButton.Enabled = false;
                } catch (InvalidOperationException ex) {
                    Debug.Print(ex.ToString());
                    openDatabaseButton.Enabled = false;
                    databaseConnectionErrorMsg.Visible = true;
                    databaseConnectionErrorMsg.Text = "Ühendus andmebaasiga ei olnud võimalik.";
                } catch (OleDbException ex)
                {
                    Debug.Print(ex.ToString());
                    databaseConnectionErrorMsg.Visible = true;
                    databaseConnectionErrorMsg.Text = "Ühendus andmebaasiga ei õnnestunud.";
                }
            }
        }

        private void startDataCollectionBtn_Click(object sender, EventArgs e)
        {
            Debug.Assert(writer != null, "Database writer was null during start data collection");
            Debug.Assert(pmReader != null, "PersonMessageReader was null during start data collection");
            idCollectorForm = new IDCollectorForm(writer, pmReader);
            idCollectorForm.showRedWhenNoMessage = personalMsgMissingRedChk.Checked;
            idCollectorForm.Show();
        }
        #endregion
    }
}

/*
Random cake
            ,:/+/-
            /M/              .,-=;//;-
        .:/= ;MH/,    ,=/+%$XH@MM#@:
        -$##@+$###@H@MMM#######H:.    -/H#
    .,H@H@ X######@ -H#####@+-     -+H###@X
    .,@##H;      +XM##M/,     =%@###@X;-
X%-  :M##########$.    .:%M###@%:
M##H,   +H@@@$/-.  ,;$M###@%,          -
M####M=,,---,.-%%H####M$:          ,+@##
@##################@/.         :%H##@$-
M###############H,         ;HM##M$=
#################.    .=$M##M$=
################H..;XM##M$=          .:+
M###################@%=           =+@MH%
@#################M/.         =+H#X%=
=+M###############M,      ,/X#H+:,
    .;XM###########H=   ,/X#H+:;
        .=+HM#######M+/+HM@+=.
            ,:/%XM####H/.
                ,.:=-.
*/
