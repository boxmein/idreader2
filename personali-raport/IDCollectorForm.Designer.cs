namespace personali_raport
{
    partial class IDCollectorForm
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.customTextLabel = new System.Windows.Forms.Label();
            this.stopDataCollectionBtn = new System.Windows.Forms.Button();
            this.startDataCollectionBtn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.saveHandwrittenID = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.loggerScannedCountLabel = new System.Windows.Forms.Label();
            this.treeViewBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.BackColor = System.Drawing.SystemColors.Control;
            this.nameLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.nameLabel.Location = new System.Drawing.Point(193, 20);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(711, 45);
            this.nameLabel.TabIndex = 0;
            // 
            // customTextLabel
            // 
            this.customTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customTextLabel.BackColor = System.Drawing.SystemColors.Control;
            this.customTextLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.customTextLabel.Location = new System.Drawing.Point(193, 81);
            this.customTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.customTextLabel.Name = "customTextLabel";
            this.customTextLabel.Size = new System.Drawing.Size(711, 231);
            this.customTextLabel.TabIndex = 1;
            // 
            // stopDataCollectionBtn
            // 
            this.stopDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopDataCollectionBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(20)))), ((int)(((byte)(13)))));
            this.stopDataCollectionBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.stopDataCollectionBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(16)))), ((int)(((byte)(11)))));
            this.stopDataCollectionBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(23)))), ((int)(((byte)(15)))));
            this.stopDataCollectionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.stopDataCollectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.stopDataCollectionBtn.Location = new System.Drawing.Point(676, 361);
            this.stopDataCollectionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.stopDataCollectionBtn.Name = "stopDataCollectionBtn";
            this.stopDataCollectionBtn.Size = new System.Drawing.Size(229, 50);
            this.stopDataCollectionBtn.TabIndex = 2;
            this.stopDataCollectionBtn.Text = "Lõpeta kogumine";
            this.stopDataCollectionBtn.UseVisualStyleBackColor = false;
            this.stopDataCollectionBtn.Click += new System.EventHandler(this.stopDataCollectionBtn_Click);
            // 
            // startDataCollectionBtn
            // 
            this.startDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startDataCollectionBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(209)))), ((int)(((byte)(70)))));
            this.startDataCollectionBtn.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.startDataCollectionBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(255)))), ((int)(((byte)(174)))));
            this.startDataCollectionBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(255)))), ((int)(((byte)(82)))));
            this.startDataCollectionBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startDataCollectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.startDataCollectionBtn.Location = new System.Drawing.Point(676, 360);
            this.startDataCollectionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.startDataCollectionBtn.Name = "startDataCollectionBtn";
            this.startDataCollectionBtn.Size = new System.Drawing.Size(229, 50);
            this.startDataCollectionBtn.TabIndex = 4;
            this.startDataCollectionBtn.Text = "Alusta kogumist";
            this.startDataCollectionBtn.UseVisualStyleBackColor = false;
            this.startDataCollectionBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.textBox1.Location = new System.Drawing.Point(193, 361);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 10, 4, 10);
            this.textBox1.MaxLength = 15;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(301, 49);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // saveHandwrittenID
            // 
            this.saveHandwrittenID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveHandwrittenID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(135)))), ((int)(((byte)(255)))));
            this.saveHandwrittenID.Enabled = false;
            this.saveHandwrittenID.FlatAppearance.BorderSize = 0;
            this.saveHandwrittenID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveHandwrittenID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.saveHandwrittenID.Location = new System.Drawing.Point(501, 361);
            this.saveHandwrittenID.Margin = new System.Windows.Forms.Padding(4);
            this.saveHandwrittenID.Name = "saveHandwrittenID";
            this.saveHandwrittenID.Size = new System.Drawing.Size(132, 50);
            this.saveHandwrittenID.TabIndex = 6;
            this.saveHandwrittenID.Text = "Salvesta";
            this.saveHandwrittenID.UseVisualStyleBackColor = false;
            this.saveHandwrittenID.Click += new System.EventHandler(this.saveHandwrittenID_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label1.Location = new System.Drawing.Point(109, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 31);
            this.label1.TabIndex = 7;
            this.label1.Text = "Nimi";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.label2.Location = new System.Drawing.Point(89, 80);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 31);
            this.label2.TabIndex = 8;
            this.label2.Text = "Teade";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // loggerScannedCountLabel
            // 
            this.loggerScannedCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loggerScannedCountLabel.AutoSize = true;
            this.loggerScannedCountLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.loggerScannedCountLabel.Location = new System.Drawing.Point(767, 325);
            this.loggerScannedCountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.loggerScannedCountLabel.Name = "loggerScannedCountLabel";
            this.loggerScannedCountLabel.Size = new System.Drawing.Size(122, 31);
            this.loggerScannedCountLabel.TabIndex = 9;
            this.loggerScannedCountLabel.Text = "0 inimest";
            this.loggerScannedCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // treeViewBtn
            // 
            this.treeViewBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.treeViewBtn.FlatAppearance.BorderSize = 0;
            this.treeViewBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.treeViewBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.treeViewBtn.Location = new System.Drawing.Point(37, 361);
            this.treeViewBtn.Margin = new System.Windows.Forms.Padding(4);
            this.treeViewBtn.Name = "treeViewBtn";
            this.treeViewBtn.Size = new System.Drawing.Size(148, 50);
            this.treeViewBtn.TabIndex = 10;
            this.treeViewBtn.Text = "Hetkeseis";
            this.treeViewBtn.UseVisualStyleBackColor = false;
            this.treeViewBtn.Click += new System.EventHandler(this.treeViewBtn_Click);
            // 
            // IDCollectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 426);
            this.Controls.Add(this.treeViewBtn);
            this.Controls.Add(this.loggerScannedCountLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveHandwrittenID);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.startDataCollectionBtn);
            this.Controls.Add(this.stopDataCollectionBtn);
            this.Controls.Add(this.customTextLabel);
            this.Controls.Add(this.nameLabel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(762, 326);
            this.Name = "IDCollectorForm";
            this.Text = "IDCollectorForm";
            this.Load += new System.EventHandler(this.IDCollectorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label customTextLabel;
        private System.Windows.Forms.Button stopDataCollectionBtn;
        private System.Windows.Forms.Button startDataCollectionBtn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button saveHandwrittenID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label loggerScannedCountLabel;
        private System.Windows.Forms.Button treeViewBtn;
    }
}