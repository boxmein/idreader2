using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace personali_raport
{
    public partial class IDCollectorForm : Form
    {
        public IDCollectorForm()
        {
            InitializeComponent();
            this.Shown += new EventHandler(windowWasShown);
        }

        public bool showRedWhenNoMessage = false;

        private void IDCollectorForm_Load(object sender, EventArgs e)
        {
            
        }

        private void windowWasShown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void stopDataCollectionBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
    }
}
