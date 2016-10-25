using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    interface IReportWriter
    {
        bool WriteReport(List<Person> personnel);
        bool SaveFile(string fileName);

        void CloseExcel();
    }
}
