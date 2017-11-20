namespace personali_raport
{
    partial class TreeReport
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
            this.unitTree = new System.Windows.Forms.TreeView();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.showTree = new System.Windows.Forms.Button();
            this.generateAttendanceReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // unitTree
            // 
            this.unitTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unitTree.BackColor = System.Drawing.SystemColors.Control;
            this.unitTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.unitTree.CheckBoxes = true;
            this.unitTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unitTree.Location = new System.Drawing.Point(0, 98);
            this.unitTree.Margin = new System.Windows.Forms.Padding(5);
            this.unitTree.Name = "unitTree";
            this.unitTree.Size = new System.Drawing.Size(1073, 503);
            this.unitTree.TabIndex = 0;
            this.unitTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.unitTree_AfterSelect);
            // 
            // startTime
            // 
            this.startTime.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.startTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTime.Location = new System.Drawing.Point(23, 16);
            this.startTime.Margin = new System.Windows.Forms.Padding(7);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(307, 34);
            this.startTime.TabIndex = 5;
            this.startTime.ValueChanged += new System.EventHandler(this.startTime_ValueChanged);
            // 
            // endTime
            // 
            this.endTime.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.endTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTime.Location = new System.Drawing.Point(344, 16);
            this.endTime.Margin = new System.Windows.Forms.Padding(7);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(300, 34);
            this.endTime.TabIndex = 6;
            this.endTime.ValueChanged += new System.EventHandler(this.endTime_ValueChanged);
            // 
            // showTree
            // 
            this.showTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showTree.Location = new System.Drawing.Point(656, 10);
            this.showTree.Margin = new System.Windows.Forms.Padding(5);
            this.showTree.Name = "showTree";
            this.showTree.Size = new System.Drawing.Size(222, 47);
            this.showTree.TabIndex = 7;
            this.showTree.Text = "Näita";
            this.showTree.UseVisualStyleBackColor = true;
            this.showTree.Click += new System.EventHandler(this.showTree_Click);
            // 
            // generateAttendanceReport
            // 
            this.generateAttendanceReport.Enabled = false;
            this.generateAttendanceReport.Location = new System.Drawing.Point(888, 10);
            this.generateAttendanceReport.Margin = new System.Windows.Forms.Padding(5);
            this.generateAttendanceReport.Name = "generateAttendanceReport";
            this.generateAttendanceReport.Size = new System.Drawing.Size(182, 47);
            this.generateAttendanceReport.TabIndex = 8;
            this.generateAttendanceReport.Text = "Kohalolek";
            this.generateAttendanceReport.UseVisualStyleBackColor = true;
            this.generateAttendanceReport.Click += new System.EventHandler(this.generateAttendanceReport_Click);
            // 
            // TreeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 601);
            this.Controls.Add(this.generateAttendanceReport);
            this.Controls.Add(this.showTree);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.unitTree);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "TreeReport";
            this.Text = "Hetkeseis";
            this.Load += new System.EventHandler(this.TreeReport_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView unitTree;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.Button showTree;
        private System.Windows.Forms.Button generateAttendanceReport;
    }
}