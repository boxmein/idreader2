using System;
using System.Collections.Generic;

namespace personali_raport
{
    public class Person
    {
        /// <summary>
        /// Contains the person's ID code. This is necessary to fetch any other
        /// kind of data from the Excel spreadsheet.
        /// </summary>
        public long idCode { get; set; }
        /// <summary>
        /// Contains any kind of user data from the Excel personnel list.
        /// Data fields may include:
        /// - First name
        /// - Last name
        /// - Division
        /// - Birth age/date
        /// - Address
        /// - Rank
        /// - Phone Number
        /// - Was that person registered?
        /// - Enter date
        /// - Leave date
        /// - Comments
        /// </summary>
        public DateTime signedInOn;
        public Dictionary<string, string> data { get; set; }

        public Person()
        {
            this.data = new Dictionary<string, string>();
        }

    }
}
