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
    enum LoggerState { Initial = 0, CollectingData = 1, Erroring = 2 };
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
        /// The user-provided settings to compose the report from.
        /// This contains the files to base the report on, plus the report type
        /// (PERSREP or Attendance), plus the date limits.
        /// </summary>
        ReportSettings settings;

        /// <summary>
        /// A state machine describing the ID card logger.
        /// Initial = "nothing going on"
        /// CollectingData = "Actively collecting data"
        /// Erroring = "There was an error"
        /// </summary>
        LoggerState loggerState = LoggerState.Initial;
        
        /// <summary>
        /// How many fools have I scanned today?
        /// (Not too many to count!)
        /// </summary>
        int loggerScannedCount = 0;

        /// <summary>
        /// The Report Writer that will be used to compose the report.
        /// </summary>
        IReportWriter reportWriter;

        /// <summary>
        /// An object that allows access to a CSV file filled with Personal Messages.
        /// These can be set per ID code, and will display on the big fullscreen
        /// reader window.
        /// </summary>
        PersonMessageReader pmReader;

        /// <summary>
        /// Manages the idreader2 executable used to gather ID card data.
        /// </summary>
        Process loggerProcess;

        /// <summary>
        /// The regex to match the first name from the ID card reader output.
        /// </summary>
        Regex firstNameRx = new Regex(@"FirstName=(\w+)");
        
        /// <summary>
        /// The regex to match the last name from the ID card reader output.
        /// </summary>
        Regex lastNameRx = new Regex(@"LastName=(\w+)");

        /// <summary>
        /// The regex to match the ID code from the ID card reader output.
        /// </summary>
        Regex idCodeRx = new Regex(@"IDcode=([0-9]{11})");

        /// <summary>
        /// A Form that shows up when the ID card reader is collecting data.
        /// Shows the current person's name, ID code and personal message.
        /// </summary>
        IDCollectorForm idCollectorForm; 

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


            // Initialize subprocess for logger
            loggerProcess = new Process();
            loggerProcess.StartInfo.FileName = "idreader2.exe";
            loggerProcess.StartInfo.CreateNoWindow = true;
            loggerProcess.StartInfo.UseShellExecute = false;
            loggerProcess.StartInfo.RedirectStandardOutput = true;
            loggerProcess.StartInfo.RedirectStandardError = true;

            loggerProcess.OutputDataReceived += LoggerProcess_OutputDataReceived;
            loggerProcess.ErrorDataReceived += LoggerProcess_ErrorDataReceived;

            loggerProcess.Exited += new EventHandler(this.onLoggerProcessExited);

            Debug.Print("CWD is: " + Directory.GetCurrentDirectory());

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnExit);

            // If we have actually got a logger program, we can allow user to control it
            if (File.Exists("idreader2.exe"))
            {
                Debug.Print("Found idreader2.exe");
                startDataCollectionBtn.Enabled = true;
            }
            else
            {
                Debug.Print("Missing CWD/idreader2.exe, cannot logger");
                startDataCollectionBtn.Enabled = false;
                loggerErrorLabel.Text = "Viga: puudub vajalik programm, et ID-kaardi andmeid koguda.";
                loggerErrorLabel.Visible = true;
            }
        }

        #region Card Reader callbacks
        /// <summary>
        /// Called when idreader2 sends messages to stderr.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void LoggerProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                Console.WriteLine("LoggerProcess_ErrorDataReceived: e.Data == null!");
                return;
            }

            var lineSplit = e.Data.Split(' ');
            int messageCode = 0;

            int.TryParse(lineSplit[0], out messageCode);
            this.Invoke((MethodInvoker) (() => this.idCollectorForm.SetError(e.Data)));
        }

        /// <summary>
        /// Called when idreader2 sends messages to stdout.
        /// (Messages such as collected ID cards!)
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void LoggerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);

            if (e.Data == null)
            {
                Console.WriteLine("LoggerProcess_OutputDataReceived: e.Data == null!");
                return;
            }

            var lineSplit = e.Data.Split(new[] { ' ' }, 2);
            int messageCode = 0;

            int.TryParse(lineSplit[0], out messageCode);

            loggerOutputLabel.ForeColor = System.Drawing.Color.Black;

            switch (messageCode)
            {
                case 0: break; // Generic logs
                case 2: // ID card data
                    this.Invoke((MethodInvoker)(() => handleNewPerson(lineSplit[1])));
                    break;
                default: // 3 - "card has been scanned", among other things
                    this.Invoke((MethodInvoker)(() => loggerOutputLabel.Text = String.Join(" ", lineSplit.Skip(1))));
                    break;
            }
        }

        /// <summary>
        /// When a collected ID card message is sent, parse it into personal details and log them
        /// to Access.
        /// 
        /// Additionally, update the personnel counter and displayed name string.
        /// 
        /// If a person's first or last name are not found, the name will not get updated.
        /// If a person's first or last name or ID code are not found, the person will not get logged.
        /// </summary>
        /// <param name="personStructure">A string that matches firstNameRx, lastNameRx, and idCodeRx.</param>
        /// <seealso cref="LoggerProcess_OutputDataReceived(object, DataReceivedEventArgs)"/>
        /// <seealso cref="firstNameRx" />
        /// <seealso cref="lastNameRx" />
        /// <seealso cref="idCodeRx" />
        private void handleNewPerson(string personStructure)
        {
            string firstName = null;
            string lastName = null;
            string idCode = null;

            loggerScannedCount++;
            loggerCountLabel.Text = loggerScannedCount + " inimest";

            var match = firstNameRx.Match(personStructure);
            
            if (match.Groups.Count == 2)
            {
                firstName = match.Groups[1].Value;
            }

            match = lastNameRx.Match(personStructure);

            if (match.Groups.Count == 2)
            {
                lastName = match.Groups[1].Value;
            }

            if (firstName == null || lastName == null)
            {
                personNameLabel.Text = "";
            }
            else
            {
                personNameLabel.Text = firstName + " " + lastName;
            }



            match = idCodeRx.Match(personStructure);

            if (match.Groups.Count == 2)
            {
                idCode = match.Groups[1].Value;

                if (pmReader != null)
                {
                    personMsgLabel.Text = pmReader.GetPersonMessage(idCode);
                }
            }

            Debug.Print("First name: {0}", firstName);
            Debug.Print("Last name: {0}", lastName);
            Debug.Print("ID code: {0}", match);

            if (firstName != null &&
                lastName != null &&
                idCode != null)
            {
                writer.log(firstName, lastName, idCode);
            } else
            {
                Debug.Print("Could not extract first name, last name or ID code from\n{0}", personStructure);
            }
            this.idCollectorForm?.ShowPerson(personNameLabel.Text, personMsgLabel.Text);
        }


        private void onLoggerProcessExited(object sender, EventArgs args)
        {
            MessageBox.Show("ID-kaardi lugeja lõpetas ootamatult töötamise.\nKogutud andmed võivad olla puudulikud või vigased, kuid logid on siiski alles.", "Viga logeri töös", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            loggerState = LoggerState.Initial;
        }

        private void startDataCollectionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                loggerProcess.Start();

                idCollectorForm = new IDCollectorForm();
                idCollectorForm.showRedWhenNoMessage = personalMsgMissingRedChk.Checked;
                idCollectorForm.Show();
                idCollectorForm.FormClosed += new FormClosedEventHandler(idCollectorForm_Closed);

                loggerProcess.BeginErrorReadLine();
                loggerProcess.BeginOutputReadLine();

                loggerState = LoggerState.CollectingData;
                dataCollectionProgressPanel.Visible = true;
                startDataCollectionPanel.Visible = false;
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print("loggerProcess.Start() threw InvalidOperationException");
                Debug.Write(ex);
                loggerErrorLabel.Text = "Viga ID-kaardi lugeja käitamisel";
                startDataCollectionBtn.Enabled = false;
            }
        }

        private void idCollectorForm_Closed(object sender, EventArgs e)
        {
            StopCollection();
        }

        private void stopDataCollectionBtn_Click(object sender, EventArgs e)
        {
            if (idCollectorForm != null && idCollectorForm.Visible)
            {
                idCollectorForm.Close();
            }
            StopCollection();
        }   

        private void StopCollection()
        {
            try
            {
                loggerProcess.CancelErrorRead();
                loggerProcess.CancelOutputRead();
                loggerProcess.Kill();
            }
            catch (InvalidOperationException)
            {
                Debug.Print("logger process was already killed");
            }
            catch (Win32Exception e)
            {
                MessageBox.Show("Viga ID-kaardi lugeja sulgemisel", String.Format("Viga ID-kaardi koguja sulgemisel: Win32Exception {0}, {1}", e.NativeErrorCode, e.ToString()), MessageBoxButtons.OK);
            }

            loggerState = LoggerState.Initial;

            dataCollectionProgressPanel.Visible = false;
            startDataCollectionPanel.Visible = true;
        }

        #endregion

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
            PersonnelReader personnelReader = new PersonnelReader(settings.personnelFileName);
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

        private void openPersonMsgFileBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Comma-separated values|*.csv";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                openPersonMsgFileBtn.Visible = false;
                clearPersonMsgFile.Visible = true;
                
                personMsgLabel.Visible = true;
                personalMsgFileLabel.Visible = true;
                personalMsgFileLabel.Text = Path.GetFileName(ofd.FileName);

                pmReader = new PersonMessageReader(ofd.FileName);
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
                    writer = new AccessWriter(ofd.FileName);
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
