using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    public class CardLogEntry
    {
        public DateTime datetime;
        public long idCode;
        public string firstName;
        public string lastName;

        public CardLogEntry()
        {
            datetime = new DateTime();
        }
    }
}
