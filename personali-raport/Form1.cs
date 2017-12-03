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

        const string J_EMPTY_VALUE = "(puudub)";

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
        /// Have the parameters been changed?
        /// </summary>
        bool parametersChanged = false;


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
            if (conn != null && 
                conn.State == System.Data.ConnectionState.Open)
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
            
            // Report loading UX:
            // While a report is loading, show "Koostan..." and disable the button
            if (reportLoading)
            {
                Debug.Print("UX update: report loading!");
                generatePersrepBtn.Text = "Koostan...";
            }
            else if (parametersChanged)
            {
                Debug.Print("Parameters changed");
                generatePersrepBtn.Text = "Alusta >>>";
                parametersChanged = false;
                reportReady = false;
            }
            else
            {
                Debug.Print("UX update: Report not loading!");
                generatePersrepBtn.Text = "Alusta >>>";
            }

            // Report Ready UX: 
            // When a report is ready, hide the Generate button and instead show the Save button.

            generatePersrepBtn.Visible = !reportReady || parametersChanged || reportLoading;
            saveReportButton.Visible = saveReportButton.Enabled = reportReady && !reportLoading;


            // Update "Generate report" button: 
            // If the start & end time are not null and the template file exists, allow generating
            generatePersrepBtn.Enabled = settings.startOfReport != null &&
                                         settings.endOfReport != null &&
                                         !reportLoading &&
                                         settings.reportTemplate != null && 
                                         File.Exists(settings.reportTemplate);

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

            if (!j1Filter.Enabled)
            {
                j1Filter.SelectedIndex = -1;
            }

            // Is J2 enabled? If so, enable the J2 textbox.
            j2Filter.Enabled = j2FilterEnabled.Checked;

            if (!j2Filter.Enabled)
            {
                j2Filter.SelectedIndex = -1;
            }

            // Is company filter enabled? If so, enable the company filter dropdown.
            companyFilter.Enabled = companyFilterEnabled.Checked;

            if (!companyFilter.Enabled)
            {
                companyFilter.SelectedIndex = -1;
            }

            platoonFilterEnabled.Enabled = settings.reportType == ReportType.ATTENDANCE && 
                                           companyFilterEnabled.Checked && 
                                           companyFilter.Text != "";

            // Uncheck platoon when disabled
            if (!platoonFilterEnabled.Enabled)
            {
                platoonFilterEnabled.Checked = false;
            }

            // Is platoon filter enabled? If so, enable the dropdown. Only for ATTENDANCE reports.
            platoonFilter.Enabled = platoonFilterEnabled.Enabled && platoonFilterEnabled.Checked && settings.reportType == ReportType.ATTENDANCE;

            if (!platoonFilter.Enabled)
            {
                platoonFilter.SelectedIndex = -1;
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
        /// For PERSREP (settings.reportType == PERSREP), the filter is a name of a company.
        /// For ATTENDANCE, the filter is the name of a platoon.
        /// Respectively, only members of the company / platoon are included.
        /// </summary>
        /// <seealso cref="IReportWriter"/>
        /// <seealso cref="PersrepReportWriter"/>
        /// <seealso cref="AttendanceReportWriter"/>
        private bool GenerateReport()
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
                progressStatusLabel.Text = "Raporti põhi on puudu.";
                Debug.Print("Report template missing. {0}", settings.reportTemplate);
                reportReady = false;
                reportLoading = false;
                UpdateValidity();
                return false;
            }

            // Support J1 and J2 filters
            JFilter j1 = new JFilter();
            j1.enabled = false;

            if (j1FilterEnabled.Checked)
            {
                int x;
                j1.enabled = true;
                if (j1Filter.Text != J_EMPTY_VALUE)
                {
                    if (int.TryParse(j1Filter.Text, out x))
                    {
                        j1.desiredValue = x;
                    }
                    else
                    {
                        MessageBox.Show("J1 parameetris on viga. Ei saanud teisendada numbriks. Jätan arvestamata.", "Viga raporti genereerimisel");
                    }
                } else
                {
                    j1.desiredValue = null;
                    Debug.Print("J1 must be empty");
                }
            }

            JFilter j2 = new JFilter();
            j2.enabled = false;
            if (j2FilterEnabled.Checked)
            {
                j2.enabled = true;
                int x;
                if (j2Filter.Text != J_EMPTY_VALUE)
                {
                    if (int.TryParse(j2Filter.Text, out x))
                    {
                        j2.desiredValue = x;
                    }
                    else
                    {
                        MessageBox.Show("J2 parameetris on viga. Ei saanud teisendada numbriks. Jätan arvestamata.", "Viga raporti genereerimisel");
                    }
                } else
                {
                    Debug.Print("J2 must be empty");
                    j2.desiredValue = null;
                }
            } 

            // Support Company filter
            if (companyFilterEnabled.Enabled && companyFilterEnabled.Checked && settings.companyFilter == null)
            {
                settings.companyFilter = companyFilter.Text;
            }

            // Support Platoon filter

            bool success = false;

            if (settings.reportType == ReportType.PERSREP)
            {
                progressStatusLabel.Text = "Koostan PERSREPi...";
                reportWriter = new PersrepReportWriter(settings.reportTemplate);
                success = reportWriter.WriteReport(cardLogReader.ReadPersrepData(settings.startOfReport, settings.endOfReport, j1, j2, settings.companyFilter));
                if (success)
                {
                    progressStatusLabel.Text = "PERSREP koostatud!";
                }
            } else if (settings.reportType == ReportType.ATTENDANCE)
            {
                progressStatusLabel.Text = "Koostan kohalolekukontrolli...";
                reportWriter = new AttendanceReportWriter(settings.reportTemplate);
                success = reportWriter.WriteReport(cardLogReader.ReadAttendanceData(settings.startOfReport, settings.endOfReport, j1, j2, settings.companyFilter, settings.platoonFilter));
                if (success)
                {
                    progressStatusLabel.Text = "Kohalolekukontroll koostatud!";
                }
            }

            // Collect unknown people if no other filters than time are enabled
            if (settings.companyFilter == null && settings.platoonFilter == null && !j2.enabled && !j1.enabled)
            {
                progressStatusLabel.Text = "Leian tundmatud inimesed...";
                var unknowns = cardLogReader.ReadUnknownPeople(settings.startOfReport, settings.endOfReport);
                reportWriter.HandleUnknownPeople(unknowns);
                progressStatusLabel.Text = "Tundmatud leitud!";
            }

            reportReady = true;
            reportLoading = false;
            settings.companyFilter = settings.platoonFilter = null;
            UpdateValidity();

            progressStatusLabel.Text = "Raport on valmis salvestamiseks.";

            return success;
        }

        private void GetPlatoonList(string company)
        {
            if (company.Length == 0 || company == null)
            {
                Debug.Print("GetPlatoonList cannot fetch for null/empty company");
                return;
            }
            var cursor = conn.CreateCommand();
            cursor.CommandText = "SELECT DISTINCT Ryhm FROM Yksus WHERE Kompanii = @company;";
            var param = new OleDbParameter("@company", OleDbType.VarWChar, company.Length);
            param.Value = company;
            cursor.Parameters.Add(param);
            OleDbDataReader reader = null;
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
                platoonFilter.BeginUpdate();

                platoonFilter.Items.Clear();
                while (reader.Read())
                {
                    platoonFilter.Items.Add(reader.GetString(0));
                }

                platoonFilter.EndUpdate();
            }
            else
            {
                return;
            }
        }

        private void GetJ1List(string company = null, string platoon = null)
        {
            var cursor = conn.CreateCommand();
            OleDbDataReader reader = null;
            cursor.CommandText = "SELECT DISTINCT J1 FROM Yksus";

            if ((company != null && company.Length > 0) || (platoon != null && platoon.Length > 0))
            {
                cursor.CommandText += " WHERE ";
            }

            if (company != null && company.Length > 0)
            {
                Debug.Print("Using company filter for J1 list");
                cursor.CommandText += "Kompanii = @company";
                var param = new OleDbParameter("@company", OleDbType.VarWChar, company.Length);
                param.Value = company;
                cursor.Parameters.Add(param);
            }

            if (platoon != null && platoon.Length > 0)
            {
                Debug.Print("Using platoon filter for J1 list");
                if (company != null)
                {
                    cursor.CommandText += " AND ";
                }

                cursor.CommandText += "Ryhm = @platoon";
                var param = new OleDbParameter("@platoon", OleDbType.VarWChar, platoon.Length);
                param.Value = platoon;
                cursor.Parameters.Add(param);
            }

            cursor.CommandText += ";";

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
                j1Filter.BeginUpdate();

                j1Filter.Items.Clear();
                while (reader.Read())
                {
                    if (reader[0] == DBNull.Value)
                    {
                        j1Filter.Items.Add(J_EMPTY_VALUE);
                    } else
                    {
                        j1Filter.Items.Add(reader.GetInt32(0));
                    }
                }

                j1Filter.EndUpdate();
            }
            else
            {
                return;
            }
        }

        private void GetJ2List(string company = null, string platoon = null)
        {
            var cursor = conn.CreateCommand();
            OleDbDataReader reader = null;
            cursor.CommandText = "SELECT DISTINCT J2 FROM Yksus";

            if ((company != null && company.Length > 0) || (platoon != null && platoon.Length > 0))
            {
                cursor.CommandText += " WHERE ";
            }

            if (company != null && company.Length > 0)
            {
                Debug.Print("Using company filter for J2 list, <{0}>", company);
                cursor.CommandText += "Kompanii = @company";
                var param = new OleDbParameter("@company", OleDbType.VarWChar, company.Length);
                param.Value = company;
                cursor.Parameters.Add(param);
            }

            if (platoon != null && platoon.Length > 0)
            {
                Debug.Print("Using platoon filter for J2 list, <{0}>", platoon);
                if (company != null)
                {
                    cursor.CommandText += " AND ";
                }

                cursor.CommandText += "Ryhm = @platoon";
                var param = new OleDbParameter("@platoon", OleDbType.VarWChar, platoon.Length);
                param.Value = platoon;
                cursor.Parameters.Add(param);
            }

            cursor.CommandText += ";";

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
                j2Filter.BeginUpdate();

                j2Filter.Items.Clear();
                while (reader.Read())
                {
                    if (reader[0] == DBNull.Value)
                    {
                        j2Filter.Items.Add(J_EMPTY_VALUE);
                    } else {
                        j2Filter.Items.Add(reader.GetInt32(0));
                    }
                }

                j2Filter.EndUpdate();
            }
            else
            {
                return;
            }
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
            var company = e.Company;
            var platoon = e.Platoon;

            // Report parameters:
            // START DATE: same as in tree report
            settings.startOfReport = sender.StartDate;
            // END DATE: same as in tree report
            settings.endOfReport = sender.EndDate;
            // REPORT TYPE: Attendance
            settings.reportType = ReportType.ATTENDANCE;

            settings.companyFilter = company;
            settings.platoonFilter = platoon;

            if (storedAttendanceTemplate != null)
            {
                settings.reportTemplate = storedAttendanceTemplate;
            } else
            {
                return;
            }

            if (GenerateReport())
            {
                saveReportButton_Click(null, null);
            };
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
            parametersChanged = true;
            UpdateValidity();
        }
        private void dataSelectionEndDate_ValueChanged(object sender, EventArgs e)
        {
            settings.endOfReport = dataSelectionEndDate.Value;
            parametersChanged = true;
            UpdateValidity();
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

            parametersChanged = true;
            UpdateValidity();
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
            GetJ1List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
            parametersChanged = true;
            UpdateValidity();
        }
        private void j2FilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            GetJ2List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
            parametersChanged = true;
            UpdateValidity();
        }
        private void companyFilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            parametersChanged = true;
            UpdateValidity();
        }
        private void companyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            parametersChanged = true;
            GetPlatoonList(companyFilter.Text);
            GetJ1List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
            GetJ2List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
            UpdateValidity();
        }
        private void platoonFilterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            parametersChanged = true;
            UpdateValidity();
        }
        private void platoonFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetJ1List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
            GetJ2List(companyFilter.Enabled ? companyFilter.Text : null, platoonFilter.Enabled ? platoonFilter.Text : null);
        }
        private void j1Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            parametersChanged = true;
            UpdateValidity();
        }
        private void j2Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            parametersChanged = true;
            UpdateValidity();
        }
        #endregion

    }
    public struct JFilter
    {
        public int? desiredValue;
        public bool enabled;
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
