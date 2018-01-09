using System;
using System.IO;

namespace USPSAddressVerification
{
    public class CsvWriter : StreamWriter
    {
        private string filename;

        public CsvWriter(Stream stream) : base(stream)
        {
        }

        public CsvWriter(string filename) : base(filename)
        {
        }

        //public CsvWriter(string filename)
        //{
        //    this.filename = filename;
        //}

        public void writeHeader()
        {
            
        }

        public void appendRow()
        {
            
        }

    }
}
