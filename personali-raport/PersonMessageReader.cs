using System.IO;

namespace personali_raport
{
    public class PersonMessageReader
    {
        string fileName;
        public PersonMessageReader(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Read the person's personal message if available. Returns null if not found in the list.
        /// </summary>
        /// <param name="idCode">The person's ID code, as a string. Used to match against the FIRST column.</param>
        /// <returns>The person's message, as a string.</returns>
        public string GetPersonMessage(string idCode)
        {
            foreach (var line in File.ReadLines(fileName))
            {
                var lineSplit = line.Split(new[] { ';' }, 2);
                
                if (lineSplit.Length > 1 && idCode == lineSplit[0])
                {
                    return lineSplit[1];
                }
            }

            return null;
        }
    }
}
