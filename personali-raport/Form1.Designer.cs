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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.startDataCollectionPanel = new System.Windows.Forms.Panel();
            this.startDataCollectionBtn = new System.Windows.Forms.Button();
            this.loggerErrorLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dataCollectionProgressPanel = new System.Windows.Forms.Panel();
            this.stopDataCollectionBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timeFilterEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.generatePersrepBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataSelectionEndDate = new System.Windows.Forms.DateTimePicker();
            this.dataSelectionStartDate = new System.Windows.Forms.DateTimePicker();
            this.timeFilterGroupBox = new System.Windows.Forms.GroupBox();
            this.reportCreatorGroupBox = new System.Windows.Forms.GroupBox();
            this.reportOptionAttendance = new System.Windows.Forms.RadioButton();
            this.reportOptionMidrep = new System.Windows.Forms.RadioButton();
            this.reportOptionPersrep = new System.Windows.Forms.RadioButton();
            this.dataSelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.clearPersonnelFilesBtn = new System.Windows.Forms.Button();
            this.clearDataFilesBtn = new System.Windows.Forms.Button();
            this.dataFileLabel = new System.Windows.Forms.Label();
            this.personnelFileLabel = new System.Windows.Forms.Label();
            this.openPersonnelFileBtn = new System.Windows.Forms.Button();
            this.openDataFileBtn = new System.Windows.Forms.Button();
            this.saveGroupBox = new System.Windows.Forms.GroupBox();
            this.progressStatusLabel = new System.Windows.Forms.Label();
            this.saveReportButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.startDataCollectionPanel.SuspendLayout();
            this.dataCollectionProgressPanel.SuspendLayout();
            this.timeFilterGroupBox.SuspendLayout();
            this.reportCreatorGroupBox.SuspendLayout();
            this.dataSelectionGroupBox.SuspendLayout();
            this.saveGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.startDataCollectionPanel);
            this.groupBox1.Controls.Add(this.dataCollectionProgressPanel);
            this.groupBox1.Location = new System.Drawing.Point(277, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Andmete kogumine";
            // 
            // startDataCollectionPanel
            // 
            this.startDataCollectionPanel.BackColor = System.Drawing.SystemColors.Control;
            this.startDataCollectionPanel.Controls.Add(this.startDataCollectionBtn);
            this.startDataCollectionPanel.Controls.Add(this.loggerErrorLabel);
            this.startDataCollectionPanel.Controls.Add(this.label6);
            this.startDataCollectionPanel.Location = new System.Drawing.Point(6, 18);
            this.startDataCollectionPanel.Name = "startDataCollectionPanel";
            this.startDataCollectionPanel.Size = new System.Drawing.Size(253, 108);
            this.startDataCollectionPanel.TabIndex = 8;
            // 
            // startDataCollectionBtn
            // 
            this.startDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startDataCollectionBtn.Enabled = false;
            this.startDataCollectionBtn.Location = new System.Drawing.Point(9, 69);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(239, 34);
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
            this.loggerErrorLabel.Size = new System.Drawing.Size(238, 41);
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
            this.dataCollectionProgressPanel.Controls.Add(this.stopDataCollectionBtn);
            this.dataCollectionProgressPanel.Controls.Add(this.label2);
            this.dataCollectionProgressPanel.Controls.Add(this.label1);
            this.dataCollectionProgressPanel.Location = new System.Drawing.Point(6, 19);
            this.dataCollectionProgressPanel.Name = "dataCollectionProgressPanel";
            this.dataCollectionProgressPanel.Size = new System.Drawing.Size(253, 108);
            this.dataCollectionProgressPanel.TabIndex = 4;
            this.dataCollectionProgressPanel.Visible = false;
            // 
            // stopDataCollectionBtn
            // 
            this.stopDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopDataCollectionBtn.Location = new System.Drawing.Point(9, 69);
            this.stopDataCollectionBtn.Name = "stopDataCollectionBtn";
            this.stopDataCollectionBtn.Size = new System.Drawing.Size(239, 34);
            this.stopDataCollectionBtn.TabIndex = 7;
            this.stopDataCollectionBtn.Text = "Lõpeta kogumine";
            this.stopDataCollectionBtn.UseVisualStyleBackColor = true;
            this.stopDataCollectionBtn.Click += new System.EventHandler(this.stopDataCollectionBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "0 inimest";
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
            this.generatePersrepBtn.Location = new System.Drawing.Point(175, 65);
            this.generatePersrepBtn.Name = "generatePersrepBtn";
            this.generatePersrepBtn.Size = new System.Drawing.Size(80, 23);
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
            this.timeFilterGroupBox.Location = new System.Drawing.Point(12, 113);
            this.timeFilterGroupBox.Name = "timeFilterGroupBox";
            this.timeFilterGroupBox.Size = new System.Drawing.Size(259, 116);
            this.timeFilterGroupBox.TabIndex = 5;
            this.timeFilterGroupBox.TabStop = false;
            this.timeFilterGroupBox.Text = "Filtreeri aja järgi";
            // 
            // reportCreatorGroupBox
            // 
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionAttendance);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionMidrep);
            this.reportCreatorGroupBox.Controls.Add(this.generatePersrepBtn);
            this.reportCreatorGroupBox.Controls.Add(this.reportOptionPersrep);
            this.reportCreatorGroupBox.Location = new System.Drawing.Point(8, 235);
            this.reportCreatorGroupBox.Name = "reportCreatorGroupBox";
            this.reportCreatorGroupBox.Size = new System.Drawing.Size(265, 100);
            this.reportCreatorGroupBox.TabIndex = 6;
            this.reportCreatorGroupBox.TabStop = false;
            this.reportCreatorGroupBox.Text = "Rapordi liik";
            // 
            // reportOptionAttendance
            // 
            this.reportOptionAttendance.AutoSize = true;
            this.reportOptionAttendance.Location = new System.Drawing.Point(10, 71);
            this.reportOptionAttendance.Name = "reportOptionAttendance";
            this.reportOptionAttendance.Size = new System.Drawing.Size(72, 17);
            this.reportOptionAttendance.TabIndex = 2;
            this.reportOptionAttendance.Text = "Kohalolek";
            this.reportOptionAttendance.UseVisualStyleBackColor = true;
            this.reportOptionAttendance.CheckedChanged += new System.EventHandler(this.reportOptionAttendance_CheckedChanged);
            // 
            // reportOptionMidrep
            // 
            this.reportOptionMidrep.AutoSize = true;
            this.reportOptionMidrep.Enabled = false;
            this.reportOptionMidrep.Location = new System.Drawing.Point(10, 50);
            this.reportOptionMidrep.Name = "reportOptionMidrep";
            this.reportOptionMidrep.Size = new System.Drawing.Size(67, 17);
            this.reportOptionMidrep.TabIndex = 1;
            this.reportOptionMidrep.Text = "MIDREP";
            this.reportOptionMidrep.UseVisualStyleBackColor = true;
            this.reportOptionMidrep.CheckedChanged += new System.EventHandler(this.reportOptionMidrep_CheckedChanged);
            // 
            // reportOptionPersrep
            // 
            this.reportOptionPersrep.AutoSize = true;
            this.reportOptionPersrep.Checked = true;
            this.reportOptionPersrep.Location = new System.Drawing.Point(10, 28);
            this.reportOptionPersrep.Name = "reportOptionPersrep";
            this.reportOptionPersrep.Size = new System.Drawing.Size(76, 17);
            this.reportOptionPersrep.TabIndex = 0;
            this.reportOptionPersrep.TabStop = true;
            this.reportOptionPersrep.Text = "PERSREP";
            this.reportOptionPersrep.UseVisualStyleBackColor = true;
            this.reportOptionPersrep.CheckedChanged += new System.EventHandler(this.reportOptionPersrep_CheckedChanged);
            // 
            // dataSelectionGroupBox
            // 
            this.dataSelectionGroupBox.Controls.Add(this.clearPersonnelFilesBtn);
            this.dataSelectionGroupBox.Controls.Add(this.clearDataFilesBtn);
            this.dataSelectionGroupBox.Controls.Add(this.dataFileLabel);
            this.dataSelectionGroupBox.Controls.Add(this.personnelFileLabel);
            this.dataSelectionGroupBox.Controls.Add(this.openPersonnelFileBtn);
            this.dataSelectionGroupBox.Controls.Add(this.openDataFileBtn);
            this.dataSelectionGroupBox.Location = new System.Drawing.Point(12, 12);
            this.dataSelectionGroupBox.Name = "dataSelectionGroupBox";
            this.dataSelectionGroupBox.Size = new System.Drawing.Size(259, 95);
            this.dataSelectionGroupBox.TabIndex = 7;
            this.dataSelectionGroupBox.TabStop = false;
            this.dataSelectionGroupBox.Text = "Üksuse tabel ja kogutud logid";
            // 
            // clearPersonnelFilesBtn
            // 
            this.clearPersonnelFilesBtn.Location = new System.Drawing.Point(12, 23);
            this.clearPersonnelFilesBtn.Name = "clearPersonnelFilesBtn";
            this.clearPersonnelFilesBtn.Size = new System.Drawing.Size(26, 26);
            this.clearPersonnelFilesBtn.TabIndex = 5;
            this.clearPersonnelFilesBtn.Text = "X";
            this.clearPersonnelFilesBtn.UseVisualStyleBackColor = true;
            this.clearPersonnelFilesBtn.Visible = false;
            this.clearPersonnelFilesBtn.Click += new System.EventHandler(this.clearPersonnelFilesBtn_Click);
            // 
            // clearDataFilesBtn
            // 
            this.clearDataFilesBtn.Location = new System.Drawing.Point(12, 61);
            this.clearDataFilesBtn.Name = "clearDataFilesBtn";
            this.clearDataFilesBtn.Size = new System.Drawing.Size(26, 26);
            this.clearDataFilesBtn.TabIndex = 4;
            this.clearDataFilesBtn.Text = "X";
            this.clearDataFilesBtn.UseVisualStyleBackColor = true;
            this.clearDataFilesBtn.Visible = false;
            this.clearDataFilesBtn.Click += new System.EventHandler(this.clearDataFilesBtn_Click);
            // 
            // dataFileLabel
            // 
            this.dataFileLabel.Location = new System.Drawing.Point(45, 67);
            this.dataFileLabel.Name = "dataFileLabel";
            this.dataFileLabel.Size = new System.Drawing.Size(208, 20);
            this.dataFileLabel.TabIndex = 3;
            this.dataFileLabel.Text = "dataFileLabel";
            this.dataFileLabel.Visible = false;
            // 
            // personnelFileLabel
            // 
            this.personnelFileLabel.Location = new System.Drawing.Point(45, 30);
            this.personnelFileLabel.Name = "personnelFileLabel";
            this.personnelFileLabel.Size = new System.Drawing.Size(195, 19);
            this.personnelFileLabel.TabIndex = 2;
            this.personnelFileLabel.Text = "personnelFileLabel";
            this.personnelFileLabel.Visible = false;
            // 
            // openPersonnelFileBtn
            // 
            this.openPersonnelFileBtn.Location = new System.Drawing.Point(10, 23);
            this.openPersonnelFileBtn.Name = "openPersonnelFileBtn";
            this.openPersonnelFileBtn.Size = new System.Drawing.Size(121, 30);
            this.openPersonnelFileBtn.TabIndex = 6;
            this.openPersonnelFileBtn.Text = "Ava personali tabel";
            this.openPersonnelFileBtn.UseVisualStyleBackColor = true;
            this.openPersonnelFileBtn.Click += new System.EventHandler(this.openPersonnelFileBtn_Click);
            // 
            // openDataFileBtn
            // 
            this.openDataFileBtn.Location = new System.Drawing.Point(10, 59);
            this.openDataFileBtn.Name = "openDataFileBtn";
            this.openDataFileBtn.Size = new System.Drawing.Size(121, 30);
            this.openDataFileBtn.TabIndex = 7;
            this.openDataFileBtn.Text = "Ava kogutud logid";
            this.openDataFileBtn.UseVisualStyleBackColor = true;
            this.openDataFileBtn.Click += new System.EventHandler(this.openDataFileBtn_Click);
            // 
            // saveGroupBox
            // 
            this.saveGroupBox.Controls.Add(this.progressStatusLabel);
            this.saveGroupBox.Controls.Add(this.saveReportButton);
            this.saveGroupBox.Location = new System.Drawing.Point(7, 341);
            this.saveGroupBox.Name = "saveGroupBox";
            this.saveGroupBox.Size = new System.Drawing.Size(264, 85);
            this.saveGroupBox.TabIndex = 8;
            this.saveGroupBox.TabStop = false;
            this.saveGroupBox.Text = "Salvesta tulemused";
            // 
            // progressStatusLabel
            // 
            this.progressStatusLabel.Location = new System.Drawing.Point(12, 21);
            this.progressStatusLabel.Name = "progressStatusLabel";
            this.progressStatusLabel.Size = new System.Drawing.Size(244, 15);
            this.progressStatusLabel.TabIndex = 2;
            this.progressStatusLabel.Text = "Loeb logisid..";
            // 
            // saveReportButton
            // 
            this.saveReportButton.Enabled = false;
            this.saveReportButton.Location = new System.Drawing.Point(176, 53);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(80, 23);
            this.saveReportButton.TabIndex = 0;
            this.saveReportButton.Text = "Salvesta";
            this.saveReportButton.UseVisualStyleBackColor = true;
            this.saveReportButton.Click += new System.EventHandler(this.saveReportButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 429);
            this.Controls.Add(this.saveGroupBox);
            this.Controls.Add(this.dataSelectionGroupBox);
            this.Controls.Add(this.reportCreatorGroupBox);
            this.Controls.Add(this.timeFilterGroupBox);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Personaliraport";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.startDataCollectionPanel.ResumeLayout(false);
            this.startDataCollectionPanel.PerformLayout();
            this.dataCollectionProgressPanel.ResumeLayout(false);
            this.dataCollectionProgressPanel.PerformLayout();
            this.timeFilterGroupBox.ResumeLayout(false);
            this.timeFilterGroupBox.PerformLayout();
            this.reportCreatorGroupBox.ResumeLayout(false);
            this.reportCreatorGroupBox.PerformLayout();
            this.dataSelectionGroupBox.ResumeLayout(false);
            this.saveGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel dataCollectionProgressPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.RadioButton reportOptionMidrep;
        private System.Windows.Forms.RadioButton reportOptionPersrep;
        private System.Windows.Forms.GroupBox dataSelectionGroupBox;
        private System.Windows.Forms.Label personnelFileLabel;
        private System.Windows.Forms.Label dataFileLabel;
        private System.Windows.Forms.Button clearDataFilesBtn;
        private System.Windows.Forms.Button clearPersonnelFilesBtn;
        private System.Windows.Forms.Panel startDataCollectionPanel;
        private System.Windows.Forms.Button startDataCollectionBtn;
        private System.Windows.Forms.Label loggerErrorLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button openPersonnelFileBtn;
        private System.Windows.Forms.Button openDataFileBtn;
        private System.Windows.Forms.GroupBox saveGroupBox;
        private System.Windows.Forms.Button saveReportButton;
        private System.Windows.Forms.Label progressStatusLabel;
    }
}

