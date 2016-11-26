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
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.nameLabel.Location = new System.Drawing.Point(12, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(667, 159);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Siia ilmub inimese nimi.";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // customTextLabel
            // 
            this.customTextLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.customTextLabel.Location = new System.Drawing.Point(12, 186);
            this.customTextLabel.Name = "customTextLabel";
            this.customTextLabel.Size = new System.Drawing.Size(667, 252);
            this.customTextLabel.TabIndex = 1;
            // 
            // stopDataCollectionBtn
            // 
            this.stopDataCollectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stopDataCollectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(186)));
            this.stopDataCollectionBtn.Location = new System.Drawing.Point(12, 441);
            this.stopDataCollectionBtn.Name = "stopDataCollectionBtn";
            this.stopDataCollectionBtn.Size = new System.Drawing.Size(186, 56);
            this.stopDataCollectionBtn.TabIndex = 2;
            this.stopDataCollectionBtn.Text = "Lõpeta kogumine";
            this.stopDataCollectionBtn.UseVisualStyleBackColor = true;
            this.stopDataCollectionBtn.Click += new System.EventHandler(this.stopDataCollectionBtn_Click);
            // 
            // IDCollectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 509);
            this.Controls.Add(this.stopDataCollectionBtn);
            this.Controls.Add(this.customTextLabel);
            this.Controls.Add(this.nameLabel);
            this.Name = "IDCollectorForm";
            this.Text = "IDCollectorForm";
            this.Load += new System.EventHandler(this.IDCollectorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label customTextLabel;
        private System.Windows.Forms.Button stopDataCollectionBtn;
    }
}