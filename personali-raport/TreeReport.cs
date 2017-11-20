using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        const string POSITION_FIELD = "Ametikoht";
        const string KOHAL_FIELD = "Kohal";

        public delegate void AttendanceReportHandler(TreeReport sender, AttendanceReportRequestEventArgs e);
        public event AttendanceReportHandler AttendanceReportRequested;

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
                    // Iga rühma alla inimeste nimistu
                    foreach (var person in platoon)
                    {
                        var name = String.Format("{0} {1} - {2}",
                            person.data[FIRST_NAME_FIELD],
                            person.data[LAST_NAME_FIELD],
                            person.data[POSITION_FIELD]);
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


        /// <summary>
        /// Show the tree based on data from the start & end time DateTimePickers.
        /// Disables the showTree button.
        /// Used as event handler for the showTree button.
        /// </summary>
        private void showTree_Click(object sender, EventArgs e)
        {
            DateTime start = startTime.Value;
            DateTime end = endTime.Value;
            showTree.Enabled = false;
            SetPersonnel(reader.ReadTreeViewData(start, end));
        }

        /// <summary>
        /// Show the tree from start time until now, based on data from the personnel reader.
        /// Additionally adjusts the UI to set the start & end times.
        /// </summary>
        public void ShowPeople()
        {
            DateTime start = DateTime.Now - TimeSpan.FromDays(1);
            DateTime end = DateTime.Now;

            startTime.Value = start;
            endTime.Value = end;

            SetPersonnel(reader.ReadTreeViewData(startTime.Value, endTime.Value));
        }

        public DateTime StartDate {
            get {
                return startTime.Value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return endTime.Value;
            }
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

        private void generateAttendanceReport_Click(object sender, EventArgs e)
        {
            generateAttendanceReport.Enabled = false;
            FireAttendanceRequest();
        }

        /// <summary>
        /// Fire the actual attendance report request.
        /// This is caught in Form1.cs, and will make the form generate an attendance report.
        /// </summary>
        protected virtual void FireAttendanceRequest()
        {
            AttendanceReportHandler ev = AttendanceReportRequested;
            if (ev != null)
            {
                if (unitTree.SelectedNode == null)
                {
                    Debug.Print("User has nothing selected.");
                    return;
                }

                // User selected kompanii
                if (unitTree.SelectedNode.Parent == null)
                {
                    Debug.Print("User probably selected a kompanii.");
                    return;
                }

                // User selected person
                if (unitTree.SelectedNode.Parent.Parent != null)
                {
                    Debug.Print("User probably selected a person.");
                    return;
                }

                var platoon = unitTree.SelectedNode.Text;
                if (platoon != null)
                {
                    Debug.Print("Attendance for platoon: {0}", platoon);
                    ev(this, new AttendanceReportRequestEventArgs(platoon));
                }
            }
        }

        private void unitTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (unitTree.SelectedNode == null)
            {
                Debug.Print("User has nothing selected.");
                generateAttendanceReport.Enabled = false;
                return;
            }

            // User selected kompanii
            if (unitTree.SelectedNode.Parent == null)
            {
                Debug.Print("User probably selected a kompanii.");
                generateAttendanceReport.Enabled = false;
                return;
            }

            // User selected person
            if (unitTree.SelectedNode.Parent.Parent != null)
            {
                Debug.Print("User probably selected a person.");
                generateAttendanceReport.Enabled = false;
                return;
            }

            generateAttendanceReport.Enabled = true;
        }
    }

    public class AttendanceReportRequestEventArgs : EventArgs
    {
        public AttendanceReportRequestEventArgs(string platoon)
        {
            this.platoon = platoon;
        }
        private string platoon;
        public string Platoon
        {
            get { return platoon; }
        }
    }
}
