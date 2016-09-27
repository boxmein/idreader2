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
            this.dataCollectionProgressPanel = new System.Windows.Forms.Panel();
            this.stopDataCollectionBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.forwardButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.generatingReportPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.selectDataPanel = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.generateAttendanceBtn = new System.Windows.Forms.Button();
            this.generateMidrepBtn = new System.Windows.Forms.Button();
            this.generatePersrepBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataSelectionEndDate = new System.Windows.Forms.DateTimePicker();
            this.dataSelectionStartDate = new System.Windows.Forms.DateTimePicker();
            this.startDataCollectionPanel = new System.Windows.Forms.Panel();
            this.startDataCollectionBtn = new System.Windows.Forms.Button();
            this.numberFilesOpen = new System.Windows.Forms.Label();
            this.openDataFilesBtn = new System.Windows.Forms.Button();
            this.openPersonnelListPanel = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.openPersonnelListBtn = new System.Windows.Forms.Button();
            this.openDataFilesPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.openReportTemplatePanel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.openReportTemplateButton = new System.Windows.Forms.Button();
            this.saveReportPanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.saveReportButton = new System.Windows.Forms.Button();
            this.reportProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.dataCollectionProgressPanel.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.generatingReportPanel.SuspendLayout();
            this.selectDataPanel.SuspendLayout();
            this.startDataCollectionPanel.SuspendLayout();
            this.openPersonnelListPanel.SuspendLayout();
            this.openDataFilesPanel.SuspendLayout();
            this.openReportTemplatePanel.SuspendLayout();
            this.saveReportPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.dataCollectionProgressPanel);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 108);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Andmete kogumine";
            // 
            // dataCollectionProgressPanel
            // 
            this.dataCollectionProgressPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.dataCollectionProgressPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dataCollectionProgressPanel.Controls.Add(this.stopDataCollectionBtn);
            this.dataCollectionProgressPanel.Controls.Add(this.label2);
            this.dataCollectionProgressPanel.Controls.Add(this.label1);
            this.dataCollectionProgressPanel.Location = new System.Drawing.Point(5, 19);
            this.dataCollectionProgressPanel.Name = "dataCollectionProgressPanel";
            this.dataCollectionProgressPanel.Size = new System.Drawing.Size(253, 83);
            this.dataCollectionProgressPanel.TabIndex = 4;
            // 
            // stopDataCollectionBtn
            // 
            this.stopDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stopDataCollectionBtn.Location = new System.Drawing.Point(9, 42);
            this.stopDataCollectionBtn.Name = "stopDataCollectionBtn";
            this.stopDataCollectionBtn.Size = new System.Drawing.Size(237, 34);
            this.stopDataCollectionBtn.TabIndex = 7;
            this.stopDataCollectionBtn.Text = "Lõpeta kogumine";
            this.stopDataCollectionBtn.UseVisualStyleBackColor = true;
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
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.reportProgress);
            this.groupBox2.Controls.Add(this.forwardButton);
            this.groupBox2.Controls.Add(this.backButton);
            this.groupBox2.Location = new System.Drawing.Point(11, 126);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 265);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Raport";
            // 
            // forwardButton
            // 
            this.forwardButton.Enabled = false;
            this.forwardButton.Location = new System.Drawing.Point(180, 229);
            this.forwardButton.Name = "forwardButton";
            this.forwardButton.Size = new System.Drawing.Size(75, 23);
            this.forwardButton.TabIndex = 8;
            this.forwardButton.Text = "Edasi >>>";
            this.forwardButton.UseVisualStyleBackColor = true;
            this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(6, 229);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 7;
            this.backButton.Text = "Tagasi <<<";
            this.backButton.UseVisualStyleBackColor = true;
            // 
            // generatingReportPanel
            // 
            this.generatingReportPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.generatingReportPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.generatingReportPanel.Controls.Add(this.label5);
            this.generatingReportPanel.Location = new System.Drawing.Point(533, 3);
            this.generatingReportPanel.Name = "generatingReportPanel";
            this.generatingReportPanel.Size = new System.Drawing.Size(249, 80);
            this.generatingReportPanel.TabIndex = 6;
            this.generatingReportPanel.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(9, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(230, 54);
            this.label5.TabIndex = 0;
            this.label5.Text = "Loon raportit...";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label5.Visible = false;
            // 
            // selectDataPanel
            // 
            this.selectDataPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.selectDataPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectDataPanel.Controls.Add(this.label8);
            this.selectDataPanel.Controls.Add(this.generateAttendanceBtn);
            this.selectDataPanel.Controls.Add(this.generateMidrepBtn);
            this.selectDataPanel.Controls.Add(this.generatePersrepBtn);
            this.selectDataPanel.Controls.Add(this.label4);
            this.selectDataPanel.Controls.Add(this.label3);
            this.selectDataPanel.Controls.Add(this.dataSelectionEndDate);
            this.selectDataPanel.Controls.Add(this.dataSelectionStartDate);
            this.selectDataPanel.Location = new System.Drawing.Point(278, 262);
            this.selectDataPanel.Name = "selectDataPanel";
            this.selectDataPanel.Size = new System.Drawing.Size(249, 202);
            this.selectDataPanel.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(197, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "4. Vali soovitud ajavahemik ja rapordi liik";
            // 
            // generateAttendanceBtn
            // 
            this.generateAttendanceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generateAttendanceBtn.Enabled = false;
            this.generateAttendanceBtn.Location = new System.Drawing.Point(167, 168);
            this.generateAttendanceBtn.Name = "generateAttendanceBtn";
            this.generateAttendanceBtn.Size = new System.Drawing.Size(75, 23);
            this.generateAttendanceBtn.TabIndex = 10;
            this.generateAttendanceBtn.Text = "Kohalolek";
            this.generateAttendanceBtn.UseVisualStyleBackColor = true;
            this.generateAttendanceBtn.Click += new System.EventHandler(this.generateAttendanceBtn_Click);
            // 
            // generateMidrepBtn
            // 
            this.generateMidrepBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generateMidrepBtn.Enabled = false;
            this.generateMidrepBtn.Location = new System.Drawing.Point(86, 168);
            this.generateMidrepBtn.Name = "generateMidrepBtn";
            this.generateMidrepBtn.Size = new System.Drawing.Size(75, 23);
            this.generateMidrepBtn.TabIndex = 9;
            this.generateMidrepBtn.Text = "MIDREP";
            this.generateMidrepBtn.UseVisualStyleBackColor = true;
            this.generateMidrepBtn.Click += new System.EventHandler(this.generateMidrepBtn_Click);
            // 
            // generatePersrepBtn
            // 
            this.generatePersrepBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.generatePersrepBtn.Location = new System.Drawing.Point(5, 168);
            this.generatePersrepBtn.Name = "generatePersrepBtn";
            this.generatePersrepBtn.Size = new System.Drawing.Size(75, 23);
            this.generatePersrepBtn.TabIndex = 8;
            this.generatePersrepBtn.Text = "PERSREP";
            this.generatePersrepBtn.UseVisualStyleBackColor = true;
            this.generatePersrepBtn.Click += new System.EventHandler(this.generatePersrepBtn_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(17, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Lõpp";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "Algus";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dataSelectionEndDate
            // 
            this.dataSelectionEndDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionEndDate.Location = new System.Drawing.Point(69, 138);
            this.dataSelectionEndDate.Name = "dataSelectionEndDate";
            this.dataSelectionEndDate.Size = new System.Drawing.Size(172, 20);
            this.dataSelectionEndDate.TabIndex = 5;
            this.dataSelectionEndDate.ValueChanged += new System.EventHandler(this.dataSelectionEndDate_ValueChanged);
            // 
            // dataSelectionStartDate
            // 
            this.dataSelectionStartDate.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.dataSelectionStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dataSelectionStartDate.Location = new System.Drawing.Point(69, 105);
            this.dataSelectionStartDate.Name = "dataSelectionStartDate";
            this.dataSelectionStartDate.Size = new System.Drawing.Size(172, 20);
            this.dataSelectionStartDate.TabIndex = 4;
            this.dataSelectionStartDate.ValueChanged += new System.EventHandler(this.dataSelectionStartDate_ValueChanged);
            // 
            // startDataCollectionPanel
            // 
            this.startDataCollectionPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.startDataCollectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.startDataCollectionPanel.Controls.Add(this.startDataCollectionBtn);
            this.startDataCollectionPanel.Location = new System.Drawing.Point(12, 397);
            this.startDataCollectionPanel.Name = "startDataCollectionPanel";
            this.startDataCollectionPanel.Size = new System.Drawing.Size(252, 84);
            this.startDataCollectionPanel.TabIndex = 3;
            // 
            // startDataCollectionBtn
            // 
            this.startDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startDataCollectionBtn.Location = new System.Drawing.Point(3, 3);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(244, 76);
            this.startDataCollectionBtn.TabIndex = 0;
            this.startDataCollectionBtn.Text = "Alusta andmete kogumist";
            this.startDataCollectionBtn.UseVisualStyleBackColor = true;
            // 
            // numberFilesOpen
            // 
            this.numberFilesOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numberFilesOpen.AutoSize = true;
            this.numberFilesOpen.Location = new System.Drawing.Point(136, 47);
            this.numberFilesOpen.Name = "numberFilesOpen";
            this.numberFilesOpen.Size = new System.Drawing.Size(66, 13);
            this.numberFilesOpen.TabIndex = 2;
            this.numberFilesOpen.Text = "Valitud 0 faili";
            // 
            // openDataFilesBtn
            // 
            this.openDataFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openDataFilesBtn.Location = new System.Drawing.Point(27, 40);
            this.openDataFilesBtn.Name = "openDataFilesBtn";
            this.openDataFilesBtn.Size = new System.Drawing.Size(103, 26);
            this.openDataFilesBtn.TabIndex = 1;
            this.openDataFilesBtn.Text = "Ava andmefailid";
            this.openDataFilesBtn.UseVisualStyleBackColor = true;
            this.openDataFilesBtn.Click += new System.EventHandler(this.openDataFilesBtn_Click);
            // 
            // openPersonnelListPanel
            // 
            this.openPersonnelListPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.openPersonnelListPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openPersonnelListPanel.Controls.Add(this.label7);
            this.openPersonnelListPanel.Controls.Add(this.openPersonnelListBtn);
            this.openPersonnelListPanel.Location = new System.Drawing.Point(278, 3);
            this.openPersonnelListPanel.Name = "openPersonnelListPanel";
            this.openPersonnelListPanel.Size = new System.Drawing.Size(249, 80);
            this.openPersonnelListPanel.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "1. Ava üksuse inimeste nimekiri";
            // 
            // openPersonnelListBtn
            // 
            this.openPersonnelListBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openPersonnelListBtn.Location = new System.Drawing.Point(69, 38);
            this.openPersonnelListBtn.Name = "openPersonnelListBtn";
            this.openPersonnelListBtn.Size = new System.Drawing.Size(110, 26);
            this.openPersonnelListBtn.TabIndex = 0;
            this.openPersonnelListBtn.Text = "Ava üksuse tabel...";
            this.openPersonnelListBtn.UseVisualStyleBackColor = true;
            this.openPersonnelListBtn.Click += new System.EventHandler(this.openPersonnelListBtn_Click);
            // 
            // openDataFilesPanel
            // 
            this.openDataFilesPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.openDataFilesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openDataFilesPanel.Controls.Add(this.label6);
            this.openDataFilesPanel.Controls.Add(this.openDataFilesBtn);
            this.openDataFilesPanel.Controls.Add(this.numberFilesOpen);
            this.openDataFilesPanel.Location = new System.Drawing.Point(278, 90);
            this.openDataFilesPanel.Name = "openDataFilesPanel";
            this.openDataFilesPanel.Size = new System.Drawing.Size(249, 80);
            this.openDataFilesPanel.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "2. Vali logitud andmed";
            // 
            // openReportTemplatePanel
            // 
            this.openReportTemplatePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.openReportTemplatePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.openReportTemplatePanel.Controls.Add(this.label9);
            this.openReportTemplatePanel.Controls.Add(this.openReportTemplateButton);
            this.openReportTemplatePanel.Location = new System.Drawing.Point(278, 176);
            this.openReportTemplatePanel.Name = "openReportTemplatePanel";
            this.openReportTemplatePanel.Size = new System.Drawing.Size(249, 80);
            this.openReportTemplatePanel.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "3. Ava PERSREPi mall";
            // 
            // openReportTemplateButton
            // 
            this.openReportTemplateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openReportTemplateButton.Location = new System.Drawing.Point(47, 41);
            this.openReportTemplateButton.Name = "openReportTemplateButton";
            this.openReportTemplateButton.Size = new System.Drawing.Size(155, 27);
            this.openReportTemplateButton.TabIndex = 0;
            this.openReportTemplateButton.Text = "Ava PERSREP dokument...";
            this.openReportTemplateButton.UseVisualStyleBackColor = true;
            this.openReportTemplateButton.Click += new System.EventHandler(this.openPersrepFileBtn_Click);
            // 
            // saveReportPanel
            // 
            this.saveReportPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.saveReportPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.saveReportPanel.Controls.Add(this.label10);
            this.saveReportPanel.Controls.Add(this.saveReportButton);
            this.saveReportPanel.Location = new System.Drawing.Point(533, 89);
            this.saveReportPanel.Name = "saveReportPanel";
            this.saveReportPanel.Size = new System.Drawing.Size(249, 80);
            this.saveReportPanel.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(165, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "5. Salvesta PERSREP uue failina";
            // 
            // saveReportButton
            // 
            this.saveReportButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.saveReportButton.Location = new System.Drawing.Point(47, 41);
            this.saveReportButton.Name = "saveReportButton";
            this.saveReportButton.Size = new System.Drawing.Size(155, 27);
            this.saveReportButton.TabIndex = 0;
            this.saveReportButton.Text = "Salvesta...";
            this.saveReportButton.UseVisualStyleBackColor = true;
            this.saveReportButton.Click += new System.EventHandler(this.saveReportButton_Click);
            // 
            // reportProgress
            // 
            this.reportProgress.Location = new System.Drawing.Point(6, 18);
            this.reportProgress.Name = "reportProgress";
            this.reportProgress.Size = new System.Drawing.Size(249, 13);
            this.reportProgress.Step = 1;
            this.reportProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.reportProgress.TabIndex = 8;
            this.reportProgress.Value = 55;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 493);
            this.Controls.Add(this.saveReportPanel);
            this.Controls.Add(this.openReportTemplatePanel);
            this.Controls.Add(this.selectDataPanel);
            this.Controls.Add(this.generatingReportPanel);
            this.Controls.Add(this.openPersonnelListPanel);
            this.Controls.Add(this.startDataCollectionPanel);
            this.Controls.Add(this.openDataFilesPanel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Personaliraport";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.dataCollectionProgressPanel.ResumeLayout(false);
            this.dataCollectionProgressPanel.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.generatingReportPanel.ResumeLayout(false);
            this.selectDataPanel.ResumeLayout(false);
            this.selectDataPanel.PerformLayout();
            this.startDataCollectionPanel.ResumeLayout(false);
            this.openPersonnelListPanel.ResumeLayout(false);
            this.openPersonnelListPanel.PerformLayout();
            this.openDataFilesPanel.ResumeLayout(false);
            this.openDataFilesPanel.PerformLayout();
            this.openReportTemplatePanel.ResumeLayout(false);
            this.openReportTemplatePanel.PerformLayout();
            this.saveReportPanel.ResumeLayout(false);
            this.saveReportPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel startDataCollectionPanel;
        private System.Windows.Forms.Panel dataCollectionProgressPanel;
        private System.Windows.Forms.Panel selectDataPanel;
        private System.Windows.Forms.Panel openPersonnelListPanel;
        private System.Windows.Forms.Panel openDataFilesPanel;
        private System.Windows.Forms.Panel generatingReportPanel;
        private System.Windows.Forms.Button startDataCollectionBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button stopDataCollectionBtn;
        private System.Windows.Forms.Button openPersonnelListBtn;
        private System.Windows.Forms.Button openDataFilesBtn;
        private System.Windows.Forms.Label numberFilesOpen;
        private System.Windows.Forms.DateTimePicker dataSelectionEndDate;
        private System.Windows.Forms.DateTimePicker dataSelectionStartDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button generateAttendanceBtn;
        private System.Windows.Forms.Button generateMidrepBtn;
        private System.Windows.Forms.Button generatePersrepBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel openReportTemplatePanel;
        private System.Windows.Forms.Button openReportTemplateButton;
        private System.Windows.Forms.Button forwardButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel saveReportPanel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button saveReportButton;
        private System.Windows.Forms.ProgressBar reportProgress;
    }
}

