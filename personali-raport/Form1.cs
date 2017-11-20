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
        /// A form that shows the tree view (hetkeseis) to the user.
        /// </summary>
        TreeReport treeReportForm;
        
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


        event EventHandler<OleDbConnection> DatabaseConnected;

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
                process.Kill();
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
            
            DatabaseConnected += (s, conn) => writer = new AccessWriter(conn);
            DatabaseConnected += (s, conn) => pmReader = new PersonMessageReader(conn);
            DatabaseConnected += (s, conn) => personnelReader = new AccessPersonnelReader(conn);
            DatabaseConnected += (s, conn) => cardLogReader = new AccessLogReader(conn);
            DatabaseConnected += (s, conn) => UpdateValidity();

            DatabaseConnected += LoadCompanyList;

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
            }

            if (storedSettings.KeyExists("AttendanceTemplate") && File.Exists(storedSettings.Read("AttendanceTemplate")))
            {
                Debug.Print("LOAD: Found stored attendance template file");
                storedAttendanceTemplate = storedSettings.Read("AttendanceTemplate");
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
        /// Loads the list of companies to be displayed on the company filter box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="conn"></param>
        private void LoadCompanyList(object sender, OleDbConnection conn)
        {
            var cursor = conn.CreateCommand();
            OleDbDataReader reader = null;
            cursor.CommandText = "SELECT DISTINCT Yksus.Kompanii FROM Yksus;";
            try
            {
                cursor.Prepare();
                reader = cursor.ExecuteReader();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print(ex.ToString());
            }
            catch (OleDbException ex)
            {
                if ((uint)ex.HResult == 0x80040E37)
                {
                    MessageBox.Show("Üksuse tabelit 'Yksus' ei eksisteeri.\nVeateade:\n" + ex.Message, "Viga Accessi andmebaasis");
                }
                Debug.Print(ex.ToString());
            }

            if (reader != null && reader.HasRows)
            {
                companyFilter.BeginUpdate();

                companyFilter.Items.Clear();
                while (reader.Read())
                {
                    companyFilter.Items.Add(reader.GetString(0));
                }

                companyFilter.EndUpdate();
            } else
            {
                return;
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
            if (conn?.State == System.Data.ConnectionState.Open)
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

            // Does idreader2.exe exist?
            if (hasIDReaderBackend)
            {
                Debug.Print("UX update: ID reader backend found!");
                startDataCollectionBtn.Enabled = true;
            } else
            {
                Debug.Print("UX update: ID reader backend missing!");
                startDataCollectionBtn.Enabled = false;
            }

            // Is J1 enabled? If so, enable the J1 textbox.
            j1Filter.Enabled = j1FilterEnabled.Checked;

            // Is J2 enabled? If so, enable the J2 textbox.
            j2Filter.Enabled = j2FilterEnabled.Checked;

            // Is company filter enabled? If so, enable the company filter dropdown.
            companyFilter.Enabled = companyFilterEnabled.Checked;
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
        /// For PERSREP (settings.reportType == PERSREP), the filter is a name of a company.
        /// For ATTENDANCE, the filter is the name of a platoon.
        /// Respectively, only members of the company / platoon are included.
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

            DestroyAllExcel();

            if (!File.Exists(settings.reportTemplate))
            {
                Debug.Print("Report template missing. {0}", settings.reportTemplate);
                return;
            }

            // Support J1 and J2 filters
            int? j1 = null;

            if (j1FilterEnabled.Checked)
            {
                j1 = (int) j1Filter.Value;
            }

            int? j2 = null;
            if (j2FilterEnabled.Checked)
            {
                j2 = (int) j2Filter.Value;
            } 

            // Support Company filter
            if (companyFilterEnabled.Checked && settings.filter == null)
            {
                settings.filter = companyFilter.Text;
            }


            if (settings.reportType == ReportType.PERSREP)
            {
                reportWriter = new PersrepReportWriter(settings.reportTemplate);
                reportWriter.WriteReport(cardLogReader.ReadPersrepData(settings.startOfReport, settings.endOfReport, settings.filter, j1, j2));
            } else if (settings.reportType == ReportType.ATTENDANCE)
            {
                reportWriter = new AttendanceReportWriter(settings.reportTemplate);
                reportWriter.WriteReport(cardLogReader.ReadAttendanceData(settings.startOfReport, settings.endOfReport, settings.filter, j1, j2));
            }

            // Collect unknown people if no other filters than time are enabled
            if (settings.filter == null && j2 == null && j1 == null)
            {
                var unknowns = cardLogReader.ReadUnknownPeople(settings.startOfReport, settings.endOfReport);
                reportWriter.HandleUnknownPeople(unknowns);
            }

            reportReady = true;
            reportLoading = false;
            settings.filter = null;
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
            if (treeReportForm != null)
            {
                Debug.Print("Tree report already open, will not open a new one");
                treeReportForm.BringToFront();
                treeReportForm.ShowPeople();

                return;
            }
            treeReportForm = new TreeReport(cardLogReader);
            treeReportForm.Show();

            // If the form closes, overwrite it with a null reference
            treeReportForm.FormClosed += (sender, args) => treeReportForm = null;

            treeReportForm.AttendanceReportRequested += tree_AttendanceReportRequested;
            
            treeReportForm.ShowPeople();
        }

        private void tree_AttendanceReportRequested(TreeReport sender, AttendanceReportRequestEventArgs e)
        {
            var platoon = e.Platoon;

            // Report parameters:
            // START DATE: same as in tree report
            settings.startOfReport = sender.StartDate;
            // END DATE: same as in tree report
            settings.endOfReport = sender.EndDate;
            // REPORT TYPE: Attendance
            settings.reportType = ReportType.ATTENDANCE;

            settings.filter = platoon;

            if (storedAttendanceTemplate != null)
            {
                settings.reportTemplate = storedAttendanceTemplate;
            } else
            {
                return;
            }

            GenerateReport();
            saveReportButton_Click(null, null);
        }

        private void openDatabase(string databaseFileName)
        {
            try
            {
                conn = new OleDbConnection(GetConnectionString(databaseFileName));
                conn.Open();
                DatabaseConnected?.Invoke(this, conn);
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
        private void startDataCollectionBtn_Click(object sender, EventArgs e)
        {
            Debug.Assert(writer != null, "Database writer was null during start data collection");
            Debug.Assert(pmReader != null, "PersonMessageReader was null during start data collection");
            Debug.Assert(personnelReader != null, "PersonnelReader was null during start data collection");
            idCollectorForm = new IDCollectorForm(writer, pmReader, personnelReader);
            idCollectorForm.Show();

            // When someone clicks "Hetkeseis" in tree view, open the tree report
            idCollectorForm.TreeViewRequested += (s, e_) => OpenTreeReport();
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
        private void j1FilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateValidity();
        }
        private void j2FilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateValidity();
        }
        private void companyFilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            UpdateValidity();
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
