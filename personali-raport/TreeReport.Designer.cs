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
            this.unitTree.Location = new System.Drawing.Point(0, 54);
            this.unitTree.Name = "unitTree";
            this.unitTree.Size = new System.Drawing.Size(583, 397);
            this.unitTree.TabIndex = 0;
            // 
            // startTime
            // 
            this.startTime.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTime.Location = new System.Drawing.Point(8, 25);
            this.startTime.Margin = new System.Windows.Forms.Padding(4);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(195, 22);
            this.startTime.TabIndex = 5;
            this.startTime.ValueChanged += new System.EventHandler(this.startTime_ValueChanged);
            // 
            // endTime
            // 
            this.endTime.CustomFormat = "dd. MM. yyyy. HH:mm:ss";
            this.endTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTime.Location = new System.Drawing.Point(208, 25);
            this.endTime.Margin = new System.Windows.Forms.Padding(4);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(198, 22);
            this.endTime.TabIndex = 6;
            this.endTime.ValueChanged += new System.EventHandler(this.endTime_ValueChanged);
            // 
            // showTree
            // 
            this.showTree.Location = new System.Drawing.Point(456, -1);
            this.showTree.Name = "showTree";
            this.showTree.Size = new System.Drawing.Size(127, 55);
            this.showTree.TabIndex = 7;
            this.showTree.Text = "Näita";
            this.showTree.UseVisualStyleBackColor = true;
            this.showTree.Click += new System.EventHandler(this.showTree_Click);
            // 
            // TreeReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 451);
            this.Controls.Add(this.showTree);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.unitTree);
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
    }
}