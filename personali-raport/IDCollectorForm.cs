using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace personali_raport
{
    public partial class IDCollectorForm : Form
    {
        /// <summary>
        /// Validates the ID code input box, as a 11 number long string with optional
        /// preceding and trailing whitespace (which will be trimmed anyway!)
        /// </summary>
        Regex idCodeInputRegex = new Regex(@"^\s*([0-9]{11})\s*$");
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
        /// Manages the idreader2 executable used to gather ID card data.
        /// </summary>
        Process loggerProcess;

        /// <summary>
        /// How many fools have I scanned today? Too many to count, don't get in my way!
        /// </summary>
        int loggerScannedCount = 0;

        /// <summary>
        /// An object that fetches the corresponding personal message for the scanned person.
        /// Stored in the Access database according to PersonMessageReader.
        /// These can be set per ID code, and will display on the big fullscreen
        /// reader window.
        /// </summary>
        PersonMessageReader pmReader;

        private AccessWriter writer;

        public IDCollectorForm(AccessWriter w, PersonMessageReader reader)
        {
            Debug.Assert(w != null, "AccessWriter instance was null in IDCollectorForm constructor");
            InitializeComponent();
            this.Shown += new EventHandler(windowWasShown);
            this.FormClosed += new FormClosedEventHandler(windowWasHidden);
            writer = w;
            pmReader = reader;
        }

        public bool showRedWhenNoMessage = false;

        private void IDCollectorForm_Load(object sender, EventArgs e)
        {
            stopDataCollectionBtn.Visible = false;
            startDataCollectionBtn.Visible = true;
        }

        private void windowWasShown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void windowWasHidden(object sender, EventArgs e)
        {
            stopCollector();
            destroyCollector();
        }

        private void stopDataCollectionBtn_Click(object sender, EventArgs e)
        { 
            startDataCollectionBtn.Visible = true;
            stopDataCollectionBtn.Visible = false;
            stopCollector();
        }

        /// <summary>
        /// Creates a new Process and starts it to collect ID cards.
        /// </summary>
        public void startCollector()
        {
            Debug.Print("Start data collection!");
            loggerScannedCount = 0;

            stopCollector();
            destroyCollector();

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

            try
            {
                loggerProcess.Start();
                loggerProcess.BeginErrorReadLine();
                loggerProcess.BeginOutputReadLine();
            }
            catch (InvalidOperationException ex)
            {
                Debug.Print("loggerProcess.Start() threw InvalidOperationException");
                Debug.Write(ex);
                startDataCollectionBtn.Enabled = false;
            }

            Debug.Print("CWD is: " + Directory.GetCurrentDirectory());

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(onLoggerProcessExited);
        }

        /// <summary>
        /// Stops data collecton with the ID card reader.
        /// </summary>
        public void stopCollector()
        {
            if (loggerProcess == null)
            {
                return;
            }

            Debug.Print("Stop data collection!");
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
        }

        /// <summary>
        /// Disposes the ID Card logger process.
        /// </summary>
        private void destroyCollector()
        {
            if (loggerProcess != null)
            {
                loggerProcess.Dispose();
                loggerProcess = null;
            }
        }

        /// <summary>
        /// Add the current person's details onto the form.
        /// </summary>
        /// <param name="name">The person's first and last name, concatenated. </param>
        /// <param name="customText">The person's custom message (Isikuteade).</param>
        public void ShowPerson(string name, string customText)
        {
            nameLabel.Text = name;
            if (customText != null && customText != "")
            {
                customTextLabel.Text = customText;
                customTextLabel.BackColor = System.Drawing.Color.Transparent;
            } else if (this.showRedWhenNoMessage)
            {
                customTextLabel.BackColor = System.Drawing.Color.Red;
                customTextLabel.Text = "Inimesel puudub individuaalteade!";
            } 
        }
        
        public void SetError(string error)
        {
            Debug.WriteLine(error);
            customTextLabel.Text = error;
        }

        private void incrementScannedCount()
        {
            loggerScannedCount += 1;
            Debug.Print("One more person scanned.");
            loggerScannedCountLabel.Text = String.Format("{0} inimest", loggerScannedCount);

        }

        private void button2_Click(object sender, EventArgs e)
        { 
            startCollector();
            startDataCollectionBtn.Visible = false;
            stopDataCollectionBtn.Visible = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (idCodeInputRegex.IsMatch(textBox1.Text))
            {
                saveHandwrittenID.Enabled = true;
            } else
            {
                saveHandwrittenID.Enabled = false;
            }
        }

        private void databaseReady()
        {
            Debug.Print("Database got hooked!");
            if (writer != null)
            {
                startDataCollectionBtn.Enabled = true;
            }
        }

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
            SetError(e.Data);
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

            switch (messageCode)
            {
                case 0: break; // Generic logs
                case 2: // ID card data
                    this.Invoke((MethodInvoker)(() => handleNewPerson(lineSplit[1])));
                    break;
                default: // 3 - "card has been scanned", among other things
                    // this.Invoke((MethodInvoker)(() => loggerOutputLabel.Text = String.Join(" ", lineSplit.Skip(1))));
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

            incrementScannedCount();
            
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

            match = idCodeRx.Match(personStructure);

            if (match.Groups.Count == 2)
            {
                idCode = match.Groups[1].Value;
            }

            Debug.Print("First name: {0}", firstName);
            Debug.Print("Last name: {0}", lastName);
            Debug.Print("ID code: {0}", match);

            if (firstName != null &&
                lastName != null &&
                idCode != null)
            {
                logPerson(firstName, lastName, idCode);
            }
            else
            {
                Debug.Print("Could not extract first name, last name or ID code from\n{0}", personStructure);
            }
        }

        /// <summary>
        /// Log the person and show them on the screen.
        /// If any of firstName, lastName or idCode are null, logs stuff to the Debug stream and returns early.
        /// </summary>
        /// <param name="firstName">The person's first name</param>
        /// <param name="lastName">The person's last name</param>
        /// <param name="idCode">The person's ID code (numeric string)</param>
        private void logPerson(string firstName, string lastName, string idCode)
        {
            if (firstName == null || lastName == null || idCode == null)
            {
                Debug.Print("!! logPerson has firstName or lastName or idCode null");
                Debug.Print("firstName={0} lastName={1} idCode={2}", firstName, lastName, idCode);
                return;
            }

            writer.log(firstName, lastName, idCode);
            ShowPerson(firstName + " " + lastName, pmReader.GetPersonMessage(idCode));
        }

        private void onLoggerProcessExited(object sender, EventArgs args)
        {
            MessageBox.Show("ID-kaardi lugeja lõpetas ootamatult töötamise.\nKogutud andmed võivad olla puudulikud või vigased, kuid logid on siiski alles.", "Viga logeri töös", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void saveHandwrittenID_Click(object sender, EventArgs e)
        {
            Debug.Assert(textBox1.Text != null, "Tried to save handwritten ID without textbox text");
            string idCode = textBox1.Text.Trim();
        }
    }
}
