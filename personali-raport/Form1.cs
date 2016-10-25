using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Threading;

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
        IReportWriter reportWriter;
        Process loggerProcess;
        
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

        private void GenerateReport()
        {
            PersonnelReader personnelReader = new PersonnelReader(settings.personnelFileName);
            CardLogReader cardLogReader = new CardLogReader();
            List<Person> logEntries = new List<Person>();

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
                            Debug.Print("Can't recognize ID code {0}", row.idCode);
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

            saveReportButton.Enabled = true;
        }

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
            sfd.FileName = "raport-" + settings.startOfReport.ToString("dd.MM.yyyy-HH.mm.ss") + ".xlsx";
            sfd.Filter = "Excel spreadsheet|*.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                progressStatusLabel.Text = "Salvestan...";
                reportWriter.SaveFile(sfd.FileName);
                progressStatusLabel.Text = "Raport on salvestatud! Vali veel andmeid või teist tüüpi mall.";
            }
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
                loggerState = LoggerState.CollectingData;
                dataCollectionProgressPanel.Visible = true;
                startDataCollectionPanel.Visible = false;
            } catch (InvalidOperationException ex)
            {
                Debug.Print("loggerProcess.Start() threw InvalidOperationException");
                Debug.Write(ex);
                loggerErrorLabel.Text = "Viga ID-kaardi lugeja käitamisel";
                startDataCollectionBtn.Enabled = false;
            }
        }

        private void stopDataCollectionBtn_Click(object sender, EventArgs e)
        {
            try {
                loggerProcess.Kill();
            } catch (InvalidOperationException)
            {
                Debug.Print("logger process was already killed");
            }
            loggerState = LoggerState.Initial;

            dataCollectionProgressPanel.Visible = false;
            startDataCollectionPanel.Visible = true;
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
            } else
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
