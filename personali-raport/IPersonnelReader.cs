using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    public interface IPersonnelReader : IDisposable
    {
        Person ReadPersonalData(string idCode);
    }
}
