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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.personalMsgFileLabel = new System.Windows.Forms.Label();
            this.personMsgLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.personalMsgMissingRedChk = new System.Windows.Forms.CheckBox();
            this.startDataCollectionPanel = new System.Windows.Forms.Panel();
            this.startDataCollectionBtn = new System.Windows.Forms.Button();
            this.loggerErrorLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dataCollectionProgressPanel = new System.Windows.Forms.Panel();
            this.loggerOutputLabel = new System.Windows.Forms.Label();
            this.stopDataCollectionBtn = new System.Windows.Forms.Button();
            this.loggerCountLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.clearPersonMsgFile = new System.Windows.Forms.Button();
            this.personNameLabel = new System.Windows.Forms.Label();
            this.openPersonMsgFileBtn = new System.Windows.Forms.Button();
            this.timeFilterEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.generatePersrepBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataSelectionEndDate = new System.Windows.Forms.DateTimePicker();
            this.dataSelectionStartDate = new System.Windows.Forms.DateTimePicker();
            this.timeFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.reportCreatorGroupBox = new System.Windows.Forms.GroupBox();
            this.reportOptionAttendance = new System.Windows.Forms.RadioButton();
            this.reportOptionPersrep = new System.Windows.Forms.RadioButton();
            this.saveGroupBox = new System.Windows.Forms.GroupBox();
            this.saveReportButton = new System.Windows.Forms.Button();
            this.progressStatusLabel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.databaseConnectionErrorMsg = new System.Windows.Forms.Label();
            this.openDatabaseButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.startDataCollectionPanel.SuspendLayout();
            this.dataCollectionProgressPanel.SuspendLayout();
            this.timeFilterGroupBox.SuspendLayout();
            this.reportCreatorGroupBox.SuspendLayout();
            this.saveGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // personalMsgFileLabel
            // 
            this.personalMsgFileLabel.Location = new System.Drawing.Point(10, 361);
            this.personalMsgFileLabel.Name = "personalMsgFileLabel";
            this.personalMsgFileLabel.Size = new System.Drawing.Size(146, 20);
            this.personalMsgFileLabel.TabIndex = 17;
            this.personalMsgFileLabel.Text = "personalMsgFileLabel";
            this.personalMsgFileLabel.Visible = false;
            // 
            // personMsgLabel
            // 
            this.personMsgLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.personMsgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.personMsgLabel.ForeColor = System.Drawing.Color.Black;
            this.personMsgLabel.Location = new System.Drawing.Point(356, 280);
            this.personMsgLabel.Name = "personMsgLabel";
            this.personMsgLabel.Size = new System.Drawing.Size(178, 51);
            this.personMsgLabel.TabIndex = 1;
            this.personMsgLabel.Text = "Teade";
            this.personMsgLabel.Visible = false;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 336);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Teatetabel";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // personalMsgMissingRedChk
            // 
            this.personalMsgMissingRedChk.AutoSize = true;
            this.personalMsgMissingRedChk.Checked = true;
            this.personalMsgMissingRedChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.personalMsgMissingRedChk.Location = new System.Drawing.Point(70, 334);
            this.personalMsgMissingRedChk.Name = "personalMsgMissingRedChk";
            this.personalMsgMissingRedChk.Size = new System.Drawing.Size(149, 17);
            this.personalMsgMissingRedChk.TabIndex = 18;
            this.personalMsgMissingRedChk.Text = "Teate puudumisel punane";
            this.personalMsgMissingRedChk.UseVisualStyleBackColor = true;
            // 
            // startDataCollectionPanel
            // 
            this.startDataCollectionPanel.BackColor = System.Drawing.SystemColors.Control;
            this.startDataCollectionPanel.Controls.Add(this.startDataCollectionBtn);
            this.startDataCollectionPanel.Controls.Add(this.loggerErrorLabel);
            this.startDataCollectionPanel.Controls.Add(this.label6);
            this.startDataCollectionPanel.Location = new System.Drawing.Point(6, 162);
            this.startDataCollectionPanel.Name = "startDataCollectionPanel";
            this.startDataCollectionPanel.Size = new System.Drawing.Size(318, 155);
            this.startDataCollectionPanel.TabIndex = 8;
            // 
            // startDataCollectionBtn
            // 
            this.startDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startDataCollectionBtn.Enabled = false;
            this.startDataCollectionBtn.Location = new System.Drawing.Point(9, 116);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(304, 34);
            this.startDataCollectionBtn.TabIndex = 7;
            this.startDataCollectionBtn.Text = "Kogu ID-kaarte";
            this.startDataCollectionBtn.UseVisualStyleBackColor = true;
            this.startDataCollectionBtn.Click += new System.EventHandler(this.startDataCollectionBtn_Click);
            // 
            // loggerErrorLabel
            // 
            this.loggerErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.loggerErrorLabel.Location = new System.Drawing.Point(8, 23);
            this.loggerErrorLabel.Name = "loggerErrorLabel";
            this.loggerErrorLabel.Size = new System.Drawing.Size(238, 16);
            this.loggerErrorLabel.TabIndex = 1;
            this.loggerErrorLabel.Text = "Viga: kogumisprogramm on puudu.";
            this.loggerErrorLabel.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Alusta kogumist";
            // 
            // dataCollectionProgressPanel
            // 
            this.dataCollectionProgressPanel.BackColor = System.Drawing.SystemColors.Control;
            this.dataCollectionProgressPanel.Controls.Add(this.loggerOutputLabel);
            this.dataCollectionProgressPanel.Controls.Add(this.stopDataCollectionBtn);
            this.dataCollectionProgressPanel.Controls.Add(this.loggerCountLabel);
            this.dataCollectionProgressPanel.Controls.Add(this.label1);
            this.dataCollectionProgressPanel.Location = new System.Drawing.Point(6, 6);
            this.dataCollectionProgressPanel.Name = "dataCollectionProgressPanel";
            this.dataCollectionProgressPanel.Size = new System.Drawing.Size(318, 155);
            this.dataCollectionProgressPanel.TabIndex = 4;
            this.dataCollectionProgressPanel.Visible = false;
            // 
            // loggerOutputLabel
            // 
            this.loggerOutputLabel.Location = new System.Drawing.Point(11, 42);
            this.loggerOutputLabel.Name = "loggerOutputLabel";
            this.loggerOutputLabel.Size = new System.Drawing.Size(235, 17);
            this.loggerOutputLabel.TabIndex = 8;
            this.loggerOutputLabel.Text = "loggerOutputLabel";
            
            // 
            // loggerCountLabel
            // 
            this.loggerCountLabel.AutoSize = true;
            this.loggerCountLabel.Location = new System.Drawing.Point(8, 23);
            this.loggerCountLabel.Name = "loggerCountLabel";
            this.loggerCountLabel.Size = new System.Drawing.Size(48, 13);
            this.loggerCountLabel.TabIndex = 1;
            this.loggerCountLabel.Text = "0 inimest";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kogub andmeid...";
            // 
            // clearPersonMsgFile
            // 
            this.clearPersonMsgFile.Location = new System.Drawing.Point(157, 358);
            this.clearPersonMsgFile.Name = "clearPersonMsgFile";
            this.clearPersonMsgFile.Size = new System.Drawing.Size(26, 26);
            this.clearPersonMsgFile.TabIndex = 13;
            this.clearPersonMsgFile.Text = "X";
            this.clearPersonMsgFile.UseVisualStyleBackColor = true;
            this.clearPersonMsgFile.Visible = false;
            this.clearPersonMsgFile.Click += new System.EventHandler(this.clearPersonMsgFile_Click);
            // 
            // personNameLabel
            // 
            this.personNameLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.personNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.personNameLabel.Location = new System.Drawing.Point(356, 250);
            this.personNameLabel.Name = "personNameLabel";
            this.personNameLabel.Size = new System.Drawing.Size(178, 24);
            this.personNameLabel.TabIndex = 0;
            this.personNameLabel.Text = "Nimi";
            this.personNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timeFilterEnabledCheckbox
            // 
            this.timeFilterEnabledCheckbox.AutoSize = true;
            this.timeFilterEnabledCheckbox.Location = new System.Drawing.Point(29, 26);
            this.timeFilterEnabledCheckbox.Name = "timeFilterEnabledCheckbox";
            this.timeFilterEnabledCheckbox.Size = new System.Drawing.Size(142, 17);
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
            this.generatePersrepBtn.Location = new System.Drawing.Point(179, 42);
            this.generatePersrepBtn.Name = "generatePersrepBtn";
            this.generatePersrepBtn.Size = new System.Drawing.Size(76, 24);
            this.generatePersrepBtn.TabIndex = 8;
            this.generatePersrepBtn.Text = "Alusta >>>";
            this.generatePersrepBtn.UseVisualStyleBackColor = true;
            this.generatePersrepBtn.Click += new System.EventHandler(this.generatePersrepBtn_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(-4, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Lõpp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(-4, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algus";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataSelectionEndDate
            // 
            this.dataSelectionEndDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionEndDate.Enabled = false;
            this.dataSelectionEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionEndDate.Location = new System.Drawing.Point(48, 86);
            this.dataSelectionEndDate.Name = "dataSelectionEndDate";
            this.dataSelectionEndDate.Size = new System.Drawing.Size(172, 20);
            this.dataSelectionEndDate.TabIndex = 5;
            this.dataSelectionEndDate.ValueChanged += new System.EventHandler(this.dataSelectionEndDate_ValueChanged);
            // 
            // dataSelectionStartDate
            // 
            this.dataSelectionStartDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionStartDate.Enabled = false;
            this.dataSelectionStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionStartDate.Location = new System.Drawing.Point(48, 53);
            this.dataSelectionStartDate.Name = "dataSelectionStartDate";
            this.dataSelectionStartDate.Size = new System.Drawing.Size(172, 20);
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
            this.timeFilterGroupBox.Enabled = false;
            this.timeFilterGroupBox.Location = new System.Drawing.Point(8, 81);
            this.timeFilterGroupBox.Name = "timeFilterGroupBox";
            this.timeFilterGroupBox.Size = new System.Drawing.Size(259, 120);
            this.timeFilterGroupBox.TabIndex = 5;
            this.timeFilterGroupBox.TabStop = false;
            this.timeFilterGroupBox.Text = "Filtreeri aja järgi";
            // 
            // reportCreatorGroupBox
            // 
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionAttendance);
            this.reportCreatorGroupBox.Controls.Add(this.generatePersrepBtn);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionPersrep);
            this.reportCreatorGroupBox.Enabled = false;
            this.reportCreatorGroupBox.Location = new System.Drawing.Point(6, 6);
            this.reportCreatorGroupBox.Name = "reportCreatorGroupBox";
            this.reportCreatorGroupBox.Size = new System.Drawing.Size(261, 72);
            this.reportCreatorGroupBox.TabIndex = 6;
            this.reportCreatorGroupBox.TabStop = false;
            this.reportCreatorGroupBox.Text = "Rapordi liik";
            // 
            // reportOptionAttendance
            // 
            this.reportOptionAttendance.AutoSize = true;
            this.reportOptionAttendance.Location = new System.Drawing.Point(10, 42);
            this.reportOptionAttendance.Name = "reportOptionAttendance";
            this.reportOptionAttendance.Size = new System.Drawing.Size(72, 17);
            this.reportOptionAttendance.TabIndex = 2;
            this.reportOptionAttendance.Text = "Kohalolek";
            this.reportOptionAttendance.UseVisualStyleBackColor = true;
            this.reportOptionAttendance.CheckedChanged += new System.EventHandler(this.reportOptionAttendance_CheckedChanged);
            // 
            // reportOptionPersrep
            // 
            this.reportOptionPersrep.AutoSize = true;
            this.reportOptionPersrep.Checked = true;
            this.reportOptionPersrep.Location = new System.Drawing.Point(10, 19);
            this.reportOptionPersrep.Name = "reportOptionPersrep";
            this.reportOptionPersrep.Size = new System.Drawing.Size(76, 17);
            this.reportOptionPersrep.TabIndex = 0;
            this.reportOptionPersrep.TabStop = true;
            this.reportOptionPersrep.Text = "PERSREP";
            this.reportOptionPersrep.UseVisualStyleBackColor = true;
            this.reportOptionPersrep.CheckedChanged += new System.EventHandler(this.reportOptionPersrep_CheckedChanged);
            // 
            // saveGroupBox
            // 
            this.saveGroupBox.Controls.Add(this.saveReportButton);
            this.saveGroupBox.Controls.Add(this.progressStatusLabel);
            this.saveGroupBox.Enabled = false;
            this.saveGroupBox.Location = new System.Drawing.Point(7, 201);
            this.saveGroupBox.Name = "saveGroupBox";
            this.saveGroupBox.Size = new System.Drawing.Size(260, 63);
            this.saveGroupBox.TabIndex = 8;
            this.saveGroupBox.TabStop = false;
            this.saveGroupBox.Text = "Salvesta tulemused";
            // 
            // saveReportButton
            // 
            this.saveReportButton.Enabled = false;
            this.saveReportButton.Location = new System.Drawing.Point(174, 32);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(80, 25);
            this.saveReportButton.TabIndex = 0;
            this.saveReportButton.Text = "Salvesta";
            this.saveReportButton.UseVisualStyleBackColor = true;
            this.saveReportButton.Click += new System.EventHandler(this.saveReportButton_Click);
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.progressStatusLabel.Location = new System.Drawing.Point(7, 16);
            this.progressStatusLabel.Name = "progressStatusLabel";
            this.progressStatusLabel.Size = new System.Drawing.Size(161, 41);
            this.progressStatusLabel.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.databaseConnectionErrorMsg);
            this.groupBox2.Controls.Add(this.openDatabaseButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 84);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Andmebaasiühendus";
            // 
            // databaseConnectionErrorMsg
            // 
            this.databaseConnectionErrorMsg.AutoSize = true;
            this.databaseConnectionErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.databaseConnectionErrorMsg.Location = new System.Drawing.Point(11, 61);
            this.databaseConnectionErrorMsg.Name = "databaseConnectionErrorMsg";
            this.databaseConnectionErrorMsg.Size = new System.Drawing.Size(185, 13);
            this.databaseConnectionErrorMsg.TabIndex = 1;
            this.databaseConnectionErrorMsg.Text = "Andmebaasi laadimine ei õnnestunud.";
            this.databaseConnectionErrorMsg.Visible = false;
            // 
            // openDatabaseButton
            // 
            this.openDatabaseButton.Location = new System.Drawing.Point(10, 25);
            this.openDatabaseButton.Name = "openDatabaseButton";
            this.openDatabaseButton.Size = new System.Drawing.Size(245, 30);
            this.openDatabaseButton.TabIndex = 0;
            this.openDatabaseButton.Text = "Ava andmebaas (.accdb)";
            this.openDatabaseButton.UseVisualStyleBackColor = true;
            this.openDatabaseButton.Click += new System.EventHandler(this.openDatabaseButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 96);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(338, 415);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.startDataCollectionPanel);
            this.tabPage1.Controls.Add(this.dataCollectionProgressPanel);
            this.tabPage1.Controls.Add(this.personalMsgMissingRedChk);
            this.tabPage1.Controls.Add(this.clearPersonMsgFile);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.personalMsgFileLabel);
            this.tabPage1.Controls.Add(this.openPersonMsgFileBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(330, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kogumine";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.reportCreatorGroupBox);
            this.tabPage2.Controls.Add(this.timeFilterGroupBox);
            this.tabPage2.Controls.Add(this.saveGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(330, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rapordid";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 579);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.personMsgLabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.personNameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "ID-kaardi lugeja ja PERSREPi koostaja 1.9";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.startDataCollectionPanel.ResumeLayout(false);
            this.startDataCollectionPanel.PerformLayout();
            this.dataCollectionProgressPanel.ResumeLayout(false);
            this.dataCollectionProgressPanel.PerformLayout();
            this.timeFilterGroupBox.ResumeLayout(false);
            this.timeFilterGroupBox.PerformLayout();
            this.reportCreatorGroupBox.ResumeLayout(false);
            this.reportCreatorGroupBox.PerformLayout();
            this.saveGroupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel dataCollectionProgressPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label loggerCountLabel;
        private System.Windows.Forms.Button stopDataCollectionBtn;
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
        private System.Windows.Forms.Panel startDataCollectionPanel;
        private System.Windows.Forms.Button startDataCollectionBtn;
        private System.Windows.Forms.Label loggerErrorLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox saveGroupBox;
        private System.Windows.Forms.Button saveReportButton;
        private System.Windows.Forms.Label progressStatusLabel;
        private System.Windows.Forms.Label loggerOutputLabel;
        private System.Windows.Forms.Label personNameLabel;
        private System.Windows.Forms.Label personMsgLabel;
        private System.Windows.Forms.Button clearPersonMsgFile;
        private System.Windows.Forms.Button openPersonMsgFileBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label personalMsgFileLabel;
        private System.Windows.Forms.CheckBox personalMsgMissingRedChk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button openDatabaseButton;
        private System.Windows.Forms.Label databaseConnectionErrorMsg;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}

