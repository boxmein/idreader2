﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.IO;

namespace personali_raport
{
    public class CardLogEntry
    {
        public DateTime datetime;
        public long idCode;

        public CardLogEntry()
        {
            datetime = new DateTime();
        }
    }
    public class CardLogReader
    {
        const string DATETIME_FORMAT = "yyyy-M-d H:mm:ss zz00";
        CardLogEntry ParseRow(string row)
        {
            CardLogEntry entry = new CardLogEntry();

            string[] values = row.Split(',');

            values[0] = values[0].Replace("\"", "");

            if (values.Length < 2)
            {
                Debug.Print("Failed to read row at all: the CSV row doesn't have two values.");
                Debug.Print("CSV row is: {0}", row);
                return null;
            }

            bool success = DateTime.TryParseExact(values[0], DATETIME_FORMAT,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out entry.datetime);

            if (!success)
            {
                Debug.Print("Failed to read card log entry because date time parsing failed.");
                Debug.Print("Datetime line is: {0}", values[0]);
                return null;
            }

            success = long.TryParse(values[1], out entry.idCode);

            if (!success)
            {
                Debug.Print("Failed to read ID code because it's not an integer");
                Debug.Print("ID code value is: {0}", values[1]);
                return null;
            }

            return entry;
        }

        public IEnumerable<IEnumerable<CardLogEntry>> LoadAllFiles(string[] dataFiles) {
            foreach (string s in dataFiles)
            {
                Debug.Print("Loading rows for data file " + s);
                yield return LoadRows(s);
            }
        }

        public IEnumerable<CardLogEntry> LoadRows(string dataFile)
        {
            foreach (var line in File.ReadLines(dataFile))
            {
                yield return ParseRow(line);
            }
        }

    }
}