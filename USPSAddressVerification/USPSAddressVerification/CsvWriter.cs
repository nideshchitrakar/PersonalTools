using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace USPSAddressVerification
{
    public class CsvWriter : StreamWriter
    {
        private string filename;
        private int columnNumber;

        public CsvWriter(Stream stream) : base(stream)
        {
        }

        public CsvWriter(string filename) : base(filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        /// <param name="headers">Array of Headers.</param>
        /// <param name="addHeaders">Add additional headers to the csv file.</param>
        //public void WriteHeader(string[] headers, string[] addHeaders = null)
        public void WriteHeader(List<string> headers)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < headers.Count; i++)
            {
                if (i < headers.Count - 1) builder.Append("\"" + headers[i] + "\",");
                else builder.Append("\"" + headers[i] + "\"\n");
                columnNumber += 1;
            }
            File.WriteAllText(filename, builder.ToString());
        }

        /// <summary>
        /// Appends the row to the csv file.
        /// </summary>
        /// <param name="row">Row to be appended.</param>
        public void AppendRow(List<string> row)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < row.Count; i++)
            {
                if (i < row.Count - 1) builder.Append("\"" + row[i] + "\",");
                else builder.Append("\"" + row[i] + "\"\n");
            }
            File.AppendAllText(filename, builder.ToString());
        }

    }
}
