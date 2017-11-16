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
    public partial class TreeReport : Form
    {
        const string FIRST_NAME_FIELD = "Eesnimi";
        const string LAST_NAME_FIELD = "Perekonnanimi";
        const string COMPANY_FIELD = "Kompanii";
        const string PLATOON_FIELD = "Ryhm";
        const string KOHAL_FIELD = "Kohal";

        ILogReader reader;

        public TreeReport(ILogReader r)
        {
            reader = r;
            InitializeComponent();

            endTime.Value = DateTime.Today;
            startTime.Value = DateTime.Today - TimeSpan.FromDays(1);
        }

        public void SetPersonnel(List<Person> personnel)
        {
            unitTree.BeginUpdate();
            unitTree.Nodes.Clear();

            // Tekita iga kompanii jaoks haru
            foreach (var company in personnel.GroupBy(person => person.data[COMPANY_FIELD]))
            {
                var companyNode = unitTree.Nodes.Add(company.Key);

                // Iga kompanii alla rühma jaoks haru
                foreach (var platoon in company.GroupBy(person => person.data[PLATOON_FIELD]))
                {
                    var platoonNode = companyNode.Nodes.Add(platoon.Key);
                    platoonNode.Nodes.Add("Kohalolekukontroll");
                    // Iga rühma alla inimeste nimistu
                    foreach (var person in platoon)
                    {
                        var name = person.data[FIRST_NAME_FIELD] + " " + person.data[LAST_NAME_FIELD];
                        var kohal = person.data[KOHAL_FIELD];
                        var personNode = platoonNode.Nodes.Add(name);
                        if (kohal != "0")
                        {
                            personNode.Checked = true;
                            platoonNode.Checked = true;
                            companyNode.Checked = true;
                        }
                    }
                }
            }

            unitTree.EndUpdate();
        }


        private void showTree_Click(object sender, EventArgs e)
        {
            DateTime start = startTime.Value;
            DateTime end = endTime.Value;
            showTree.Enabled = false;
            SetPersonnel(reader.ReadTreeViewData(start, end));
        }

        private void startTime_ValueChanged(object sender, EventArgs e)
        {
            showTree.Enabled = true;
        }
        private void endTime_ValueChanged(object sender, EventArgs e)
        {
            showTree.Enabled = true;
        }
        private void TreeReport_Load(object sender, EventArgs e) { } 
    }
}
