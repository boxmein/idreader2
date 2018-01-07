using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace personali_raport
{
    public partial class PersonNameEntry : Form
    {
        public string firstName = "";
        public string lastName = "";

        public PersonNameEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            firstName = textBox2.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            lastName = textBox1.Text;
        }
    }
}
