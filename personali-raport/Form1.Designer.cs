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
            this.companyFilter = new System.Windows.Forms.ComboBox();
            this.progressStatusLabel = new System.Windows.Forms.Label();
            this.companyFilterEnabled = new System.Windows.Forms.CheckBox();
            this.j2FilterEnabled = new System.Windows.Forms.CheckBox();
            this.j1FilterEnabled = new System.Windows.Forms.CheckBox();
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
            this.hetkeseisBtn = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.platoonFilter = new System.Windows.Forms.ComboBox();
            this.platoonFilterEnabled = new System.Windows.Forms.CheckBox();
            this.j1Filter = new System.Windows.Forms.ComboBox();
            this.j2Filter = new System.Windows.Forms.ComboBox();
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
            this.startDataCollectionBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.startDataCollectionBtn.Enabled = false;
            this.startDataCollectionBtn.FlatAppearance.BorderSize = 0;
            this.startDataCollectionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startDataCollectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDataCollectionBtn.Location = new System.Drawing.Point(6, 6);
            this.startDataCollectionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(396, 112);
            this.startDataCollectionBtn.TabIndex = 7;
            this.startDataCollectionBtn.Text = "Kogu ID-kaarte";
            this.startDataCollectionBtn.UseVisualStyleBackColor = false;
            this.startDataCollectionBtn.Click += new System.EventHandler(this.startDataCollectionBtn_Click);
            // 
            // loggerErrorLabel
            // 
            this.loggerErrorLabel.ForeColor = System.Drawing.Color.Red;
            this.loggerErrorLabel.Location = new System.Drawing.Point(8, 226);
            this.loggerErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggerErrorLabel.Name = "loggerErrorLabel";
            this.loggerErrorLabel.Size = new System.Drawing.Size(350, 65);
            this.loggerErrorLabel.TabIndex = 1;
            this.loggerErrorLabel.Text = "Viga: kogumisprogramm on puudu.";
            this.loggerErrorLabel.Visible = false;
            // 
            // timeFilterEnabledCheckbox
            // 
            this.timeFilterEnabledCheckbox.AutoSize = true;
            this.timeFilterEnabledCheckbox.ForeColor = System.Drawing.SystemColors.Desktop;
            this.timeFilterEnabledCheckbox.Location = new System.Drawing.Point(9, 35);
            this.timeFilterEnabledCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.timeFilterEnabledCheckbox.Name = "timeFilterEnabledCheckbox";
            this.timeFilterEnabledCheckbox.Size = new System.Drawing.Size(313, 33);
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
            this.generatePersrepBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.generatePersrepBtn.Location = new System.Drawing.Point(225, 62);
            this.generatePersrepBtn.Margin = new System.Windows.Forms.Padding(4);
            this.generatePersrepBtn.Name = "generatePersrepBtn";
            this.generatePersrepBtn.Size = new System.Drawing.Size(160, 45);
            this.generatePersrepBtn.TabIndex = 8;
            this.generatePersrepBtn.Text = "Alusta >>>";
            this.generatePersrepBtn.UseVisualStyleBackColor = true;
            this.generatePersrepBtn.Click += new System.EventHandler(this.generatePersrepBtn_Click);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label4.Location = new System.Drawing.Point(-4, 121);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 35);
            this.label4.TabIndex = 7;
            this.label4.Text = "Lõpp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label3.Location = new System.Drawing.Point(-4, 80);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 34);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algus";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataSelectionEndDate
            // 
            this.dataSelectionEndDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionEndDate.Enabled = false;
            this.dataSelectionEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionEndDate.Location = new System.Drawing.Point(90, 122);
            this.dataSelectionEndDate.Margin = new System.Windows.Forms.Padding(4);
            this.dataSelectionEndDate.Name = "dataSelectionEndDate";
            this.dataSelectionEndDate.Size = new System.Drawing.Size(302, 34);
            this.dataSelectionEndDate.TabIndex = 5;
            this.dataSelectionEndDate.ValueChanged += new System.EventHandler(this.dataSelectionEndDate_ValueChanged);
            // 
            // dataSelectionStartDate
            // 
            this.dataSelectionStartDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionStartDate.Enabled = false;
            this.dataSelectionStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionStartDate.Location = new System.Drawing.Point(90, 80);
            this.dataSelectionStartDate.Margin = new System.Windows.Forms.Padding(4);
            this.dataSelectionStartDate.Name = "dataSelectionStartDate";
            this.dataSelectionStartDate.Size = new System.Drawing.Size(302, 34);
            this.dataSelectionStartDate.TabIndex = 4;
            this.dataSelectionStartDate.ValueChanged += new System.EventHandler(this.dataSelectionStartDate_ValueChanged);
            // 
            // timeFilterGroupBox
            // 
            this.timeFilterGroupBox.Controls.Add(this.j2Filter);
            this.timeFilterGroupBox.Controls.Add(this.j1Filter);
            this.timeFilterGroupBox.Controls.Add(this.platoonFilter);
            this.timeFilterGroupBox.Controls.Add(this.platoonFilterEnabled);
            this.timeFilterGroupBox.Controls.Add(this.companyFilter);
            this.timeFilterGroupBox.Controls.Add(this.progressStatusLabel);
            this.timeFilterGroupBox.Controls.Add(this.companyFilterEnabled);
            this.timeFilterGroupBox.Controls.Add(this.j2FilterEnabled);
            this.timeFilterGroupBox.Controls.Add(this.j1FilterEnabled);
            this.timeFilterGroupBox.Controls.Add(this.timeFilterEnabledCheckbox);
            this.timeFilterGroupBox.Controls.Add(this.dataSelectionStartDate);
            this.timeFilterGroupBox.Controls.Add(this.dataSelectionEndDate);
            this.timeFilterGroupBox.Controls.Add(this.label4);
            this.timeFilterGroupBox.Controls.Add(this.label3);
            this.timeFilterGroupBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.timeFilterGroupBox.Location = new System.Drawing.Point(4, 131);
            this.timeFilterGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.timeFilterGroupBox.Name = "timeFilterGroupBox";
            this.timeFilterGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.timeFilterGroupBox.Size = new System.Drawing.Size(400, 392);
            this.timeFilterGroupBox.TabIndex = 5;
            this.timeFilterGroupBox.TabStop = false;
            this.timeFilterGroupBox.Text = "Filtreerimine";
            // 
            // companyFilter
            // 
            this.companyFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.companyFilter.FormattingEnabled = true;
            this.companyFilter.Location = new System.Drawing.Point(208, 170);
            this.companyFilter.Name = "companyFilter";
            this.companyFilter.Size = new System.Drawing.Size(185, 37);
            this.companyFilter.TabIndex = 18;
            this.companyFilter.SelectedIndexChanged += new System.EventHandler(this.companyFilter_SelectedIndexChanged);
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.BackColor = System.Drawing.SystemColors.Control;
            this.progressStatusLabel.Location = new System.Drawing.Point(8, 334);
            this.progressStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.progressStatusLabel.Name = "progressStatusLabel";
            this.progressStatusLabel.Size = new System.Drawing.Size(388, 43);
            this.progressStatusLabel.TabIndex = 2;
            // 
            // companyFilterEnabled
            // 
            this.companyFilterEnabled.AutoSize = true;
            this.companyFilterEnabled.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.companyFilterEnabled.Location = new System.Drawing.Point(7, 170);
            this.companyFilterEnabled.Name = "companyFilterEnabled";
            this.companyFilterEnabled.Size = new System.Drawing.Size(137, 33);
            this.companyFilterEnabled.TabIndex = 15;
            this.companyFilterEnabled.Text = "Kompanii";
            this.companyFilterEnabled.UseVisualStyleBackColor = true;
            this.companyFilterEnabled.CheckedChanged += new System.EventHandler(this.companyFilterEnabled_CheckedChanged);
            // 
            // j2FilterEnabled
            // 
            this.j2FilterEnabled.AutoSize = true;
            this.j2FilterEnabled.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.j2FilterEnabled.Location = new System.Drawing.Point(9, 297);
            this.j2FilterEnabled.Name = "j2FilterEnabled";
            this.j2FilterEnabled.Size = new System.Drawing.Size(72, 33);
            this.j2FilterEnabled.TabIndex = 14;
            this.j2FilterEnabled.Text = "J2: ";
            this.j2FilterEnabled.UseVisualStyleBackColor = true;
            this.j2FilterEnabled.CheckedChanged += new System.EventHandler(this.j2FilterEnabled_CheckedChanged);
            // 
            // j1FilterEnabled
            // 
            this.j1FilterEnabled.AutoSize = true;
            this.j1FilterEnabled.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.j1FilterEnabled.Location = new System.Drawing.Point(9, 254);
            this.j1FilterEnabled.Name = "j1FilterEnabled";
            this.j1FilterEnabled.Size = new System.Drawing.Size(72, 33);
            this.j1FilterEnabled.TabIndex = 13;
            this.j1FilterEnabled.Text = "J1: ";
            this.j1FilterEnabled.UseVisualStyleBackColor = true;
            this.j1FilterEnabled.CheckedChanged += new System.EventHandler(this.j1FilterEnabled_CheckedChanged);
            // 
            // reportCreatorGroupBox
            // 
            this.reportCreatorGroupBox.Controls.Add(this.button1);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionAttendance);
            this.reportCreatorGroupBox.Controls.Add(this.generatePersrepBtn);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionPersrep);
            this.reportCreatorGroupBox.Controls.Add(this.saveReportButton);
            this.reportCreatorGroupBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.reportCreatorGroupBox.Location = new System.Drawing.Point(8, 8);
            this.reportCreatorGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.reportCreatorGroupBox.Name = "reportCreatorGroupBox";
            this.reportCreatorGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.reportCreatorGroupBox.Size = new System.Drawing.Size(396, 115);
            this.reportCreatorGroupBox.TabIndex = 6;
            this.reportCreatorGroupBox.TabStop = false;
            this.reportCreatorGroupBox.Text = "Rapordi liik";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(225, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 45);
            this.button1.TabIndex = 9;
            this.button1.Text = "Ava põhi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // reportOptionAttendance
            // 
            this.reportOptionAttendance.AutoSize = true;
            this.reportOptionAttendance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.reportOptionAttendance.Location = new System.Drawing.Point(5, 74);
            this.reportOptionAttendance.Margin = new System.Windows.Forms.Padding(4);
            this.reportOptionAttendance.Name = "reportOptionAttendance";
            this.reportOptionAttendance.Size = new System.Drawing.Size(142, 33);
            this.reportOptionAttendance.TabIndex = 2;
            this.reportOptionAttendance.Text = "Kohalolek";
            this.reportOptionAttendance.UseVisualStyleBackColor = true;
            this.reportOptionAttendance.CheckedChanged += new System.EventHandler(this.reportOptionAttendance_CheckedChanged);
            // 
            // reportOptionPersrep
            // 
            this.reportOptionPersrep.AutoSize = true;
            this.reportOptionPersrep.Checked = true;
            this.reportOptionPersrep.ForeColor = System.Drawing.SystemColors.ControlText;
            this.reportOptionPersrep.Location = new System.Drawing.Point(5, 33);
            this.reportOptionPersrep.Margin = new System.Windows.Forms.Padding(4);
            this.reportOptionPersrep.Name = "reportOptionPersrep";
            this.reportOptionPersrep.Size = new System.Drawing.Size(148, 33);
            this.reportOptionPersrep.TabIndex = 0;
            this.reportOptionPersrep.TabStop = true;
            this.reportOptionPersrep.Text = "PERSREP";
            this.reportOptionPersrep.UseVisualStyleBackColor = true;
            this.reportOptionPersrep.CheckedChanged += new System.EventHandler(this.reportOptionPersrep_CheckedChanged);
            // 
            // saveReportButton
            // 
            this.saveReportButton.BackColor = System.Drawing.SystemColors.Highlight;
            this.saveReportButton.Enabled = false;
            this.saveReportButton.FlatAppearance.BorderSize = 0;
            this.saveReportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveReportButton.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.saveReportButton.Location = new System.Drawing.Point(225, 62);
            this.saveReportButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(160, 45);
            this.saveReportButton.TabIndex = 0;
            this.saveReportButton.Text = "Salvesta";
            this.saveReportButton.UseVisualStyleBackColor = false;
            this.saveReportButton.Click += new System.EventHandler(this.saveReportButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.openDatabaseButton);
            this.groupBox2.Location = new System.Drawing.Point(16, 586);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(412, 96);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Andmebaasiühendus";
            // 
            // openDatabaseButton
            // 
            this.openDatabaseButton.Location = new System.Drawing.Point(8, 23);
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
            this.databaseConnectionErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.databaseConnectionErrorMsg.Location = new System.Drawing.Point(8, 219);
            this.databaseConnectionErrorMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.databaseConnectionErrorMsg.Name = "databaseConnectionErrorMsg";
            this.databaseConnectionErrorMsg.Size = new System.Drawing.Size(350, 67);
            this.databaseConnectionErrorMsg.TabIndex = 1;
            this.databaseConnectionErrorMsg.Text = "Andmebaasi laadimine ei õnnestunud.";
            this.databaseConnectionErrorMsg.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Enabled = false;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(420, 570);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.hetkeseisBtn);
            this.tabPage1.Controls.Add(this.databaseConnectionErrorMsg);
            this.tabPage1.Controls.Add(this.loggerErrorLabel);
            this.tabPage1.Controls.Add(this.startDataCollectionBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(412, 528);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Kogumine";
            // 
            // hetkeseisBtn
            // 
            this.hetkeseisBtn.FlatAppearance.BorderSize = 0;
            this.hetkeseisBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.hetkeseisBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hetkeseisBtn.Location = new System.Drawing.Point(6, 123);
            this.hetkeseisBtn.Name = "hetkeseisBtn";
            this.hetkeseisBtn.Size = new System.Drawing.Size(397, 85);
            this.hetkeseisBtn.TabIndex = 8;
            this.hetkeseisBtn.Text = "Hetkeseis";
            this.hetkeseisBtn.UseVisualStyleBackColor = false;
            this.hetkeseisBtn.Click += new System.EventHandler(this.hetkeseisBtn_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.reportCreatorGroupBox);
            this.tabPage2.Controls.Add(this.timeFilterGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(412, 528);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Rapordid";
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // platoonFilter
            // 
            this.platoonFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.platoonFilter.Enabled = false;
            this.platoonFilter.FormattingEnabled = true;
            this.platoonFilter.Location = new System.Drawing.Point(208, 211);
            this.platoonFilter.Name = "platoonFilter";
            this.platoonFilter.Size = new System.Drawing.Size(185, 37);
            this.platoonFilter.TabIndex = 22;
            this.platoonFilter.SelectedIndexChanged += new System.EventHandler(this.platoonFilter_SelectedIndexChanged);
            // 
            // platoonFilterEnabled
            // 
            this.platoonFilterEnabled.AutoSize = true;
            this.platoonFilterEnabled.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.platoonFilterEnabled.Location = new System.Drawing.Point(8, 211);
            this.platoonFilterEnabled.Name = "platoonFilterEnabled";
            this.platoonFilterEnabled.Size = new System.Drawing.Size(98, 33);
            this.platoonFilterEnabled.TabIndex = 21;
            this.platoonFilterEnabled.Text = "Rühm";
            this.platoonFilterEnabled.UseVisualStyleBackColor = true;
            this.platoonFilterEnabled.CheckedChanged += new System.EventHandler(this.platoonFilterEnabled_CheckedChanged);
            // 
            // j1Filter
            // 
            this.j1Filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.j1Filter.FormattingEnabled = true;
            this.j1Filter.Location = new System.Drawing.Point(208, 252);
            this.j1Filter.Name = "j1Filter";
            this.j1Filter.Size = new System.Drawing.Size(188, 37);
            this.j1Filter.TabIndex = 23;
            this.j1Filter.SelectedIndexChanged += new System.EventHandler(this.j1Filter_SelectedIndexChanged);
            // 
            // j2Filter
            // 
            this.j2Filter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.j2Filter.FormattingEnabled = true;
            this.j2Filter.Location = new System.Drawing.Point(208, 293);
            this.j2Filter.Name = "j2Filter";
            this.j2Filter.Size = new System.Drawing.Size(188, 37);
            this.j2Filter.TabIndex = 24;
            this.j2Filter.SelectedIndexChanged += new System.EventHandler(this.j2Filter_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 699);
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
        private System.Windows.Forms.CheckBox j2FilterEnabled;
        private System.Windows.Forms.CheckBox j1FilterEnabled;
        private System.Windows.Forms.CheckBox companyFilterEnabled;
        private System.Windows.Forms.ComboBox companyFilter;
        private System.Windows.Forms.ComboBox platoonFilter;
        private System.Windows.Forms.CheckBox platoonFilterEnabled;
        private System.Windows.Forms.ComboBox j2Filter;
        private System.Windows.Forms.ComboBox j1Filter;
    }
}

