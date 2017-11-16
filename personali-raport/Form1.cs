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

    /// <summary>
    /// The entry point form to the application.
    /// Allows the user to open the IDCollectorForm to collect ID card logs, 
    /// open the Access database the rest of the app depends on, and create 
    /// reports.
    /// Additionally, maintains the Access database connection, and instances of
    /// IPersonnelReader & IReportWriter.
    /// </summary>
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
        /// A reader that fetches personal data from the Excel or Access database.
        /// </summary>
        IPersonnelReader personnelReader;

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
        /// The Card Log Reader reads card logs and maps to various data structures for UX and report purposes.
        /// </summary>
        ILogReader cardLogReader;


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

        /// <summary>
        /// Stores the settings in an INI file.
        /// </summary>
        IniFile storedSettings;

        string storedPersrepTemplate;
        string storedAttendanceTemplate;

        /// <summary>
        /// Is idreader2.exe present?
        /// </summary>
        bool hasIDReaderBackend = false;
        /// <summary>
        /// Has the report been written and can it be saved?
        /// </summary>
        bool reportReady = false;
        /// <summary>
        /// Is the report currently being generated?
        /// </summary>
        bool reportLoading = false;
        /// <summary>
        /// Constructs the Form.
        /// Initializes the ReportSettings object.
        /// </summary>
        /// <seealso cref="ReportSettings"/>
        public Form1()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            settings = new ReportSettings();
            InitializeComponent();
        }

        /// <summary>
        /// Destroy Excel with lasers!
        /// Kills all Excel processes.
        /// </summary>
        private void DestroyAllExcel()
        {
            foreach (var process in Process.GetProcessesByName("EXCEL"))
            {
                Debug.Print("Stopping Excel: {0}", process.Id);
                process.CloseMainWindow();
            }
        }

        /// <summary>
        /// Callback to call when the process exits.
        /// </summary>
        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Debug.Print("Process exiting. Taking idreader2, and Access connection with.");
            
            // Close the database connection
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
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
            DestroyAllExcel();
            // Initialize settings
            settings.reportType = ReportType.PERSREP;

            storedSettings = new IniFile();

            // Load Access DB from the INI file if possible
            if (storedSettings.KeyExists("AccessDatabase") && File.Exists(storedSettings.Read("AccessDatabase")))
            {
                Debug.Print("LOAD: Found stored database! Connecting.");
                openDatabase(storedSettings.Read("AccessDatabase"));
            }

            if (storedSettings.KeyExists("PersrepTemplate") && File.Exists(storedSettings.Read("PersrepTemplate")))
            {
                Debug.Print("LOAD: Found stored PERSREP template file");
                storedPersrepTemplate = storedSettings.Read("PersrepTemplate");
                settings.reportTemplate = storedPersrepTemplate;
                button1.Enabled = false;
                // The default option is to generate a PERSREP, so enable generate button too.
                generatePersrepBtn.Enabled = true;
            }

            if (storedSettings.KeyExists("AttendanceTemplate") && File.Exists(storedSettings.Read("AttendanceTemplate")))
            {
                Debug.Print("LOAD: Found stored attendance template file");
                storedAttendanceTemplate = storedSettings.Read("AttendanceTemplate");
                button1.Enabled = false;
                generatePersrepBtn.Enabled = true;
            }

            settings.personnelFileName = null;
            settings.reportFileName = null;

            // Initialize time filter settings
            if (timeFilterEnabledCheckbox.Checked)
            {
                settings.startOfReport = dataSelectionStartDate.Value;
                settings.endOfReport = dataSelectionEndDate.Value;
            }
            else {
                dataSelectionStartDate.Value = DateTime.Today.Subtract(TimeSpan.FromDays(1));
                dataSelectionEndDate.Value = DateTime.Today;
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

            UpdateValidity();
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
            // Update "Ava põhi" button:
            // If there's stored template for PERSREP / ATTENDANCE, don't allow picking template.
            if (settings.reportType == ReportType.PERSREP && storedPersrepTemplate != null)
            {
                Debug.Print("UX update: PERSREP selected and stored template not null");
                button1.Enabled = false;
            }
            else if (settings.reportType == ReportType.ATTENDANCE && storedAttendanceTemplate != null)
            {
                Debug.Print("UX update: ATTENDANCE selected and stored template not null");
                button1.Enabled = false;
            }
            else
            {
                Debug.Print("UX update: {0} selected and template is missing, allowing to open file", settings.reportType == ReportType.PERSREP ? "PERSREP" : "ATTENDANCE");
                button1.Enabled = true;
            }

            // Database related UX: 
            // Hide the database section & disable open DB button when DB is loaded
            // Allow collecting logs and hide the errors when DB is loaded
            if (conn.State == System.Data.ConnectionState.Open)
            {
                Debug.Print("UX update: DB open, allowing DB functionality");
                openDatabaseButton.Enabled = false;
                groupBox2.Visible = false;

                databaseConnectionErrorMsg.Visible = false;

                startDataCollectionBtn.Enabled = true;

                tabControl1.Enabled = true;
            } else
            {
                Debug.Print("UX update: DB closed, disabling DB functionality");
                openDatabaseButton.Enabled = true;
                groupBox2.Visible = true;

                startDataCollectionBtn.Enabled = false;

                tabControl1.Enabled = false;
            }

            // Update "Generate report" button: 
            // If the start & end time are not null and the template file exists, allow generating
            if (settings.startOfReport != null &&
                settings.endOfReport != null &&
                settings.reportTemplate != null && File.Exists(settings.reportTemplate))
            {
                Debug.Print("UX update: All OK for report, allowing generation");
                generatePersrepBtn.Enabled = true;
            }
            else
            {
                Debug.Print("UX update: Report times invalid OR report template missing, not allowing generate report");
                generatePersrepBtn.Enabled = false;
            }

            // Report loading UX:
            // While a report is loading, show "Koostan..." and disable the button
            if (reportLoading)
            {
                Debug.Print("UX update: report loading!");
                generatePersrepBtn.Visible = true;
                generatePersrepBtn.Enabled = false;
                saveReportButton.Visible = false;
                generatePersrepBtn.Text = "Koostan...";
            }
            else
            {
                Debug.Print("UX update: Report not loading!");
                generatePersrepBtn.Visible = false;
                generatePersrepBtn.Enabled = true;
                saveReportButton.Visible = true;
                generatePersrepBtn.Text = "Alusta >>>";
            }

            // Report Ready UX: 
            // When a report is ready, hide the Generate button and instead show the Save button.
            if (reportReady)
            {
                Debug.Print("UX update: report ready!");
                generatePersrepBtn.Visible = false;
                saveReportButton.Visible = true;
                saveReportButton.Enabled = true;
            } else
            {
                Debug.Print("UX update: report not ready!");
                generatePersrepBtn.Visible = true;
                saveReportButton.Visible = false;
                saveReportButton.Enabled = false;
            }



            if (hasIDReaderBackend)
            {
                Debug.Print("UX update: ID reader backend found!");
                startDataCollectionBtn.Enabled = true;
            } else
            {
                Debug.Print("UX update: ID reader backend missing!");
                startDataCollectionBtn.Enabled = false;
            }
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

            DestroyAllExcel();
        }

        /// <summary>
        /// Collect personnel file data and feed them to the ReportWriter.
        /// </summary>
        /// <seealso cref="IReportWriter"/>
        /// <seealso cref="PersrepReportWriter"/>
        /// <seealso cref="AttendanceReportWriter"/>
        private void GenerateReport()
        {
            Debug.Assert(conn != null, "Database connection was null in GenerateReport()");
            Debug.Assert(personnelReader != null, "personnel reader not initialized in generateReport()");

            reportReady = false;
            reportLoading = true;
            UpdateValidity();


            if (timeFilterEnabledCheckbox.Checked)
            {
                settings.startOfReport = dataSelectionStartDate.Value;
                settings.endOfReport = dataSelectionEndDate.Value;
            } else
            {
                settings.startOfReport = DateTime.MinValue;
                settings.endOfReport = DateTime.MaxValue; 
            }

            if (reportWriter != null)
            {
                reportWriter.CloseExcel();
                reportWriter = null;
            }

            if (!File.Exists(settings.reportTemplate))
            {
                Debug.Print("Report template missing. {0}", settings.reportTemplate);
                return;
            }

            if (settings.reportType == ReportType.PERSREP)
            {
                reportWriter = new PersrepReportWriter(settings.reportTemplate);
                reportWriter.WriteReport(cardLogReader.ReadPersrepData(settings.startOfReport, settings.endOfReport));
            } else if (settings.reportType == ReportType.ATTENDANCE)
            {
                reportWriter = new AttendanceReportWriter(settings.reportTemplate);
                reportWriter.WriteReport(cardLogReader.ReadAttendanceData(settings.startOfReport, settings.endOfReport));
            }

            reportReady = true;
            reportLoading = false;
            UpdateValidity();
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

        private void OpenTreeReport()
        {
            var tree = new TreeReport(cardLogReader);
            tree.Show();
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

            var sfd = new SaveFileDialog() {
                AddExtension = true,
                CheckPathExists = true,
                Filter = "Excel spreadsheet|*.xlsx"
            };

            var datetime = settings.startOfReport;

            // If time filtering is turned off, startOfReport is DateTime.MinValue
            if (settings.startOfReport == DateTime.MinValue)
            {
                datetime = DateTime.Now;
            }

            sfd.FileName = "raport-" + datetime.ToString("dd.MM.yyyy-HH.mm.ss") + ".xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                progressStatusLabel.Text = "Salvestan...";
                reportWriter.SaveFile(sfd.FileName);
                progressStatusLabel.Text = "Raport on salvestatud!";
                reportReady = false;

                UpdateValidity();
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
                if (storedPersrepTemplate != null)
                {
                    settings.reportTemplate = storedPersrepTemplate;
                }

                reportReady = false;
                Debug.Print("Report type is now PERSREP");
                UpdateValidity();
            }
        }
        private void reportOptionAttendance_CheckedChanged(object sender, EventArgs e)
        {
            if (reportOptionAttendance.Checked)
            {
                settings.reportType = ReportType.ATTENDANCE;
                if (storedAttendanceTemplate != null)
                {
                    settings.reportTemplate = storedAttendanceTemplate;
                }
                reportReady = false;
                Debug.Print("Report type is now Attendance");
                UpdateValidity();
            }
        }
        private void openDatabaseButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Access databases|*.accdb";
            
            if (ofd.ShowDialog() == DialogResult.OK && !ofd.FileName.Equals(""))
            {
                storedSettings.Write("AccessDatabase", ofd.FileName);
                openDatabase(ofd.FileName);

                UpdateValidity();
            }
        }
        private void openDatabase(string databaseFileName)
        {
            try
            {
                conn = new OleDbConnection(GetConnectionString(databaseFileName));
                conn.Open();
                writer = new AccessWriter(conn);
                pmReader = new PersonMessageReader(conn);
                personnelReader = new AccessPersonnelReader(conn);
                cardLogReader = new AccessLogReader(conn);
                Debug.Print("Database connected to: " + databaseFileName);
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
                databaseConnectionErrorMsg.Visible = true;
                databaseConnectionErrorMsg.Text = "Ühendus andmebaasiga ei olnud võimalik.";
            }
            catch (OleDbException ex)
            {
                Debug.Print(ex.ToString());
                databaseConnectionErrorMsg.Visible = true;
                databaseConnectionErrorMsg.Text = "Ühendus andmebaasiga ei olnud võimalik.";
            }

            UpdateValidity();
        }
        private void startDataCollectionBtn_Click(object sender, EventArgs e)
        {
            Debug.Assert(writer != null, "Database writer was null during start data collection");
            Debug.Assert(pmReader != null, "PersonMessageReader was null during start data collection");
            Debug.Assert(personnelReader != null, "PersonnelReader was null during start data collection");
            idCollectorForm = new IDCollectorForm(writer, pmReader, personnelReader);
            idCollectorForm.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() { Filter = "Excel worksheets|*.xls;*.xlsx" };

            if (ofd.ShowDialog() == DialogResult.OK && !ofd.FileName.Equals(""))
            {
                settings.reportTemplate = ofd.FileName;
                if (settings.reportType == ReportType.PERSREP)
                {
                    storedSettings.Write("PersrepTemplate", ofd.FileName);
                } else if (settings.reportType == ReportType.ATTENDANCE)
                {
                    storedSettings.Write("AttendanceTemplate", ofd.FileName);
                }
                UpdateValidity();
            }
        }
        private void tabPage2_Click(object sender, EventArgs e) {}
        private void hetkeseisBtn_Click(object sender, EventArgs e)
        {
            OpenTreeReport();
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
