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
            customTextLabel.Text = customText;
        }
        
        public void SetError(string error)
        {
            Debug.WriteLine(error);
            customTextLabel.Text = error;
        }
    }
}
