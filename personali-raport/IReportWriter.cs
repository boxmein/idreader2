﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace personali_raport
{
    public interface IReportWriter
    {
        bool WriteReport(List<Person> personnel);
        bool SaveFile(string fileName);
        bool HandleUnknownPeople(List<Person> unknownPeople);
        void CloseExcel();
    }
}
