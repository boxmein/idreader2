using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading;
using System.Text.RegularExpressions;
using System.ComponentModel;

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
        ReportSettings settings;
        LoggerState loggerState = LoggerState.Initial;
        int loggerScannedCount = 0;

        IReportWriter reportWriter;
        PersonMessageReader pmReader;
        Process loggerProcess;

        Regex firstNameRx = new Regex(@"FirstName=(\w+)");
        Regex lastNameRx = new Regex(@"LastName=(\w+)");
        Regex idCodeRx = new Regex(@"IDcode=(\d+)");

        IDCollectorForm idCollectorForm; 


        public Form1()
        {
            settings = new ReportSettings();
            InitializeComponent();
        }
        
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

        #region Error Logger Related Stuff
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
                    this.Invoke((MethodInvoker)(() => loggerOutputLabel.Text = e.Data));
                    break;
            }
        }

        private void handleNewPerson(string personStructure)
        {
            string firstName = null, lastName = null, idCode;

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

            if (pmReader != null && match.Groups.Count == 2)
            {
                idCode = match.Groups[1].Value;
                personMsgLabel.Text = pmReader.GetPersonMessage(idCode);
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
                MessageBox.Show("Viga ID-kaardi lugeja sulgemisel", String.Format("Viga ID-kaardi lugeja sulgemisel: Win32Exception {}, {}", e.NativeErrorCode, e.ToString()), MessageBoxButtons.OK);
            }

            loggerState = LoggerState.Initial;

            dataCollectionProgressPanel.Visible = false;
            startDataCollectionPanel.Visible = true;
        }

        #endregion


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
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (reportWriter != null)
            {
                reportWriter.CloseExcel();
            }
        }

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
        private void openPersonnelFileBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel spreadsheets|*.xlsx";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.personnelFileName = ofd.FileName;

                openPersonnelFileBtn.Visible = false;
                clearPersonnelFilesBtn.Visible = true;
                personnelFileLabel.Visible = true;

                personnelFileLabel.Text = Path.GetFileName(ofd.FileName);

                UpdateValidity();
            }
        }
        private void openDataFileBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Comma-separated values|*.csv";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.dataFiles = ofd.FileNames;

                openDataFileBtn.Visible = false;
                clearDataFilesBtn.Visible = true;
                dataFileLabel.Visible = true;

                dataFileLabel.Text = "Valitud " + ofd.FileNames.Length + " fail" + (ofd.FileNames.Length > 1 ? "i" : "");

                UpdateValidity();
            }
        }
        private void openReportFileBtn_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel spreadsheets|*.xlsx";
            var dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                settings.reportTemplate = ofd.FileName;

                openReportFileBtn.Visible = false;
                clearReportFileBtn.Visible = true;
                reportFileLabel.Visible = true;

                reportFileLabel.Text = Path.GetFileName(ofd.FileName);
                UpdateValidity();
            }
        }
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
        private void clearPersonnelFilesBtn_Click(object sender, EventArgs e)
        {
            personnelFileLabel.Visible = false;
            clearPersonnelFilesBtn.Visible = false;
            openPersonnelFileBtn.Visible = true;
            UpdateValidity();
        }
        private void clearDataFilesBtn_Click(object sender, EventArgs e)
        {
            dataFileLabel.Visible = false;
            clearDataFilesBtn.Visible = false;
            openDataFileBtn.Visible = true;
            UpdateValidity();
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
        private void clearReportFileBtn_Click(object sender, EventArgs e)
        {
            reportFileLabel.Visible = false;
            clearReportFileBtn.Visible = false;
            openReportFileBtn.Visible = true;
            UpdateValidity();
        }
        private void personalMsgMissingRedChk_CheckedChanged(object sender, EventArgs e)
        {

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
