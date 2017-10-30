using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    interface ILogReader
    {
        IEnumerable<CardLogEntry> ReadAllCardsInTimespan(DateTime start, DateTime end);
    }
}
