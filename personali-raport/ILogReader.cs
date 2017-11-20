using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    public interface ILogReader
    {
        IEnumerable<CardLogEntry> ReadAllCardsInTimespan(DateTime start, DateTime end);
        List<PersrepItem> ReadPersrepData(DateTime start, DateTime end, string companyFilter = null, int? j1Filter = null, int? j2Filter = null);
        List<AttendanceItem> ReadAttendanceData(DateTime start, DateTime end, string platoonFilter = null, int? j1Filter = null, int? j2Filter = null);
        List<Person> ReadTreeViewData(DateTime start, DateTime end);
        List<Person> ReadUnknownPeople(DateTime start, DateTime end);
    }
}
