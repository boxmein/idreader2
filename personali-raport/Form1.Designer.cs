namespace personali_raport
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (disposing && (personnelReader != null))
            {
                personnelReader.Dispose();
            }
            if (disposing && (writer != null))
            {
                writer.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startDataCollectionBtn = new System.Windows.Forms.Button();
            this.loggerErrorLabel = new System.Windows.Forms.Label();
            this.timeFilterEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.generatePersrepBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataSelectionEndDate = new System.Windows.Forms.DateTimePicker();
            this.dataSelectionStartDate = new System.Windows.Forms.DateTimePicker();
            this.timeFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.reportCreatorGroupBox = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.reportOptionAttendance = new System.Windows.Forms.RadioButton();
            this.reportOptionPersrep = new System.Windows.Forms.RadioButton();
            this.saveReportButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.openDatabaseButton = new System.Windows.Forms.Button();
            this.databaseConnectionErrorMsg = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.progressStatusLabel = new System.Windows.Forms.Label();
            this.hetkeseisBtn = new System.Windows.Forms.Button();
            this.timeFilterGroupBox.SuspendLayout();
            this.reportCreatorGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // startDataCollectionBtn
            // 
            this.startDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startDataCollectionBtn.Enabled = false;
            this.startDataCollectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDataCollectionBtn.Location = new System.Drawing.Point(3, 4);
            this.startDataCollectionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(350, 115);
            this.startDataCollectionBtn.TabIndex = 7;
            this.startDataCollectionBtn.Text = "Kogu ID-kaarte";
            this.startDataCollectionBtn.UseVisualStyleBackColor = true;
            this.startDataCollectionBtn.Click += new System.EventHandler(this.startDataCollectionBtn_Click);
            // 
            // loggerErrorLabel
            // 
            this.loggerErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.loggerErrorLabel.Location = new System.Drawing.Point(8, 312);
            this.loggerErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggerErrorLabel.Name = "loggerErrorLabel";
            this.loggerErrorLabel.Size = new System.Drawing.Size(317, 20);
            this.loggerErrorLabel.TabIndex = 1;
            this.loggerErrorLabel.Text = "Viga: kogumisprogramm on puudu.";
            this.loggerErrorLabel.Visible = false;
            // 
            // timeFilterEnabledCheckbox
            // 
            this.timeFilterEnabledCheckbox.AutoSize = true;
            this.timeFilterEnabledCheckbox.Location = new System.Drawing.Point(39, 32);
            this.timeFilterEnabledCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.timeFilterEnabledCheckbox.Name = "timeFilterEnabledCheckbox";
            this.timeFilterEnabledCheckbox.Size = new System.Drawing.Size(191, 21);
            this.timeFilterEnabledCheckbox.TabIndex = 12;
            this.timeFilterEnabledCheckbox.Text = "Filtreeri osalejaid aja järgi";
            this.timeFilterEnabledCheckbox.UseVisualStyleBackColor = true;
            this.timeFilterEnabledCheckbox.CheckedChanged += new System.EventHandler(this.timeFilterEnabledCheckbox_CheckedChanged);
            // 
            // generatePersrepBtn
            // 
            this.generatePersrepBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generatePersrepBtn.Enabled = false;
            this.generatePersrepBtn.Location = new System.Drawing.Point(148, 62);
            this.generatePersrepBtn.Margin = new System.Windows.Forms.Padding(4);
            this.generatePersrepBtn.Name = "generatePersrepBtn";
            this.generatePersrepBtn.Size = new System.Drawing.Size(180, 45);
            this.generatePersrepBtn.TabIndex = 8;
            this.generatePersrepBtn.Text = "Alusta >>>";
            this.generatePersrepBtn.UseVisualStyleBackColor = true;
            this.generatePersrepBtn.Click += new System.EventHandler(this.generatePersrepBtn_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(4, 106);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Lõpp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 65);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algus";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataSelectionEndDate
            // 
            this.dataSelectionEndDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionEndDate.Enabled = false;
            this.dataSelectionEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionEndDate.Location = new System.Drawing.Point(64, 106);
            this.dataSelectionEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.dataSelectionEndDate.Name = "dataSelectionEndDate";
            this.dataSelectionEndDate.Size = new System.Drawing.Size(228, 22);
            this.dataSelectionEndDate.TabIndex = 5;
            this.dataSelectionEndDate.ValueChanged += new System.EventHandler(this.dataSelectionEndDate_ValueChanged);
            // 
            // dataSelectionStartDate
            // 
            this.dataSelectionStartDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionStartDate.Enabled = false;
            this.dataSelectionStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionStartDate.Location = new System.Drawing.Point(64, 65);
            this.dataSelectionStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.dataSelectionStartDate.Name = "dataSelectionStartDate";
            this.dataSelectionStartDate.Size = new System.Drawing.Size(228, 22);
            this.dataSelectionStartDate.TabIndex = 4;
            this.dataSelectionStartDate.ValueChanged += new System.EventHandler(this.dataSelectionStartDate_ValueChanged);
            // 
            // timeFilterGroupBox
            // 
            this.timeFilterGroupBox.Controls.Add(this.timeFilterEnabledCheckbox);
            this.timeFilterGroupBox.Controls.Add(this.dataSelectionStartDate);
            this.timeFilterGroupBox.Controls.Add(this.dataSelectionEndDate);
            this.timeFilterGroupBox.Controls.Add(this.label4);
            this.timeFilterGroupBox.Controls.Add(this.label3);
            this.timeFilterGroupBox.Location = new System.Drawing.Point(4, 131);
            this.timeFilterGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.timeFilterGroupBox.Name = "timeFilterGroupBox";
            this.timeFilterGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.timeFilterGroupBox.Size = new System.Drawing.Size(343, 148);
            this.timeFilterGroupBox.TabIndex = 5;
            this.timeFilterGroupBox.TabStop = false;
            this.timeFilterGroupBox.Text = "Filtreeri aja järgi";
            // 
            // reportCreatorGroupBox
            // 
            this.reportCreatorGroupBox.Controls.Add(this.button1);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionAttendance);
            this.reportCreatorGroupBox.Controls.Add(this.generatePersrepBtn);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionPersrep);
            this.reportCreatorGroupBox.Controls.Add(this.saveReportButton);
            this.reportCreatorGroupBox.Location = new System.Drawing.Point(8, 8);
            this.reportCreatorGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.reportCreatorGroupBox.Name = "reportCreatorGroupBox";
            this.reportCreatorGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.reportCreatorGroupBox.Size = new System.Drawing.Size(339, 115);
            this.reportCreatorGroupBox.TabIndex = 6;
            this.reportCreatorGroupBox.TabStop = false;
            this.reportCreatorGroupBox.Text = "Rapordi liik";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(148, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(180, 45);
            this.button1.TabIndex = 9;
            this.button1.Text = "Ava põhi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // reportOptionAttendance
            // 
            this.reportOptionAttendance.AutoSize = true;
            this.reportOptionAttendance.Location = new System.Drawing.Point(13, 52);
            this.reportOptionAttendance.Margin = new System.Windows.Forms.Padding(4);
            this.reportOptionAttendance.Name = "reportOptionAttendance";
            this.reportOptionAttendance.Size = new System.Drawing.Size(91, 21);
            this.reportOptionAttendance.TabIndex = 2;
            this.reportOptionAttendance.Text = "Kohalolek";
            this.reportOptionAttendance.UseVisualStyleBackColor = true;
            this.reportOptionAttendance.CheckedChanged += new System.EventHandler(this.reportOptionAttendance_CheckedChanged);
            // 
            // reportOptionPersrep
            // 
            this.reportOptionPersrep.AutoSize = true;
            this.reportOptionPersrep.Checked = true;
            this.reportOptionPersrep.Location = new System.Drawing.Point(13, 23);
            this.reportOptionPersrep.Margin = new System.Windows.Forms.Padding(4);
            this.reportOptionPersrep.Name = "reportOptionPersrep";
            this.reportOptionPersrep.Size = new System.Drawing.Size(94, 21);
            this.reportOptionPersrep.TabIndex = 0;
            this.reportOptionPersrep.TabStop = true;
            this.reportOptionPersrep.Text = "PERSREP";
            this.reportOptionPersrep.UseVisualStyleBackColor = true;
            this.reportOptionPersrep.CheckedChanged += new System.EventHandler(this.reportOptionPersrep_CheckedChanged);
            // 
            // saveReportButton
            // 
            this.saveReportButton.Enabled = false;
            this.saveReportButton.Location = new System.Drawing.Point(147, 63);
            this.saveReportButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(180, 45);
            this.saveReportButton.TabIndex = 0;
            this.saveReportButton.Text = "Salvesta";
            this.saveReportButton.UseVisualStyleBackColor = true;
            this.saveReportButton.Click += new System.EventHandler(this.saveReportButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.openDatabaseButton);
            this.groupBox2.Location = new System.Drawing.Point(13, 399);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(362, 117);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Andmebaasiühendus";
            // 
            // openDatabaseButton
            // 
            this.openDatabaseButton.Location = new System.Drawing.Point(13, 31);
            this.openDatabaseButton.Margin = new System.Windows.Forms.Padding(4);
            this.openDatabaseButton.Name = "openDatabaseButton";
            this.openDatabaseButton.Size = new System.Drawing.Size(327, 37);
            this.openDatabaseButton.TabIndex = 0;
            this.openDatabaseButton.Text = "Ava andmebaas (.accdb)";
            this.openDatabaseButton.UseVisualStyleBackColor = true;
            this.openDatabaseButton.Click += new System.EventHandler(this.openDatabaseButton_Click);
            // 
            // databaseConnectionErrorMsg
            // 
            this.databaseConnectionErrorMsg.AutoSize = true;
            this.databaseConnectionErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.databaseConnectionErrorMsg.Location = new System.Drawing.Point(6, 332);
            this.databaseConnectionErrorMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.databaseConnectionErrorMsg.Name = "databaseConnectionErrorMsg";
            this.databaseConnectionErrorMsg.Size = new System.Drawing.Size(248, 17);
            this.databaseConnectionErrorMsg.TabIndex = 1;
            this.databaseConnectionErrorMsg.Text = "Andmebaasi laadimine ei õnnestunud.";
            this.databaseConnectionErrorMsg.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Enabled = false;
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(366, 382);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.hetkeseisBtn);
            this.tabPage1.Controls.Add(this.databaseConnectionErrorMsg);
            this.tabPage1.Controls.Add(this.loggerErrorLabel);
            this.tabPage1.Controls.Add(this.startDataCollectionBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(358, 353);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kogumine";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.progressStatusLabel);
            this.tabPage2.Controls.Add(this.reportCreatorGroupBox);
            this.tabPage2.Controls.Add(this.timeFilterGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(358, 353);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rapordid";
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.BackColor = System.Drawing.SystemColors.Control;
            this.progressStatusLabel.Location = new System.Drawing.Point(6, 291);
            this.progressStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressStatusLabel.Name = "progressStatusLabel";
            this.progressStatusLabel.Size = new System.Drawing.Size(342, 58);
            this.progressStatusLabel.TabIndex = 2;
            // 
            // hetkeseisBtn
            // 
            this.hetkeseisBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hetkeseisBtn.Location = new System.Drawing.Point(3, 126);
            this.hetkeseisBtn.Name = "hetkeseisBtn";
            this.hetkeseisBtn.Size = new System.Drawing.Size(348, 101);
            this.hetkeseisBtn.TabIndex = 8;
            this.hetkeseisBtn.Text = "Hetkeseis";
            this.hetkeseisBtn.UseVisualStyleBackColor = true;
            this.hetkeseisBtn.Click += new System.EventHandler(this.hetkeseisBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 528);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ID-kaardi lugeja ja PERSREPi koostaja 1.9";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.timeFilterGroupBox.ResumeLayout(false);
            this.timeFilterGroupBox.PerformLayout();
            this.reportCreatorGroupBox.ResumeLayout(false);
            this.reportCreatorGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dataSelectionEndDate;
        private System.Windows.Forms.DateTimePicker dataSelectionStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button generatePersrepBtn;
        private System.Windows.Forms.CheckBox timeFilterEnabledCheckbox;
        private System.Windows.Forms.GroupBox timeFilterGroupBox;
        private System.Windows.Forms.GroupBox reportCreatorGroupBox;
        private System.Windows.Forms.RadioButton reportOptionAttendance;
        private System.Windows.Forms.RadioButton reportOptionPersrep;
        private System.Windows.Forms.Button startDataCollectionBtn;
        private System.Windows.Forms.Label loggerErrorLabel;
        private System.Windows.Forms.Button saveReportButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button openDatabaseButton;
        private System.Windows.Forms.Label databaseConnectionErrorMsg;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label progressStatusLabel;
        private System.Windows.Forms.Button hetkeseisBtn;
    }
}

