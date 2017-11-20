using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    public interface IReportWriter
    {
        bool WriteReport(List<PersrepItem> personnel);
        bool WriteReport(List<AttendanceItem> personnel);
        void HandleUnknownPeople(List<Person> personnel);
        bool SaveFile(string fileName);
        void CloseExcel();
    }
}
