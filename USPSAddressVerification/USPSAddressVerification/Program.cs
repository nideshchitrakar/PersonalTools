////////////////////////////////////////////////////////////////////////////////
///     Program.cs - Utilizes Validator and CSVReader classes to scan through
///                  a csv file and check for address errors.
///     Author: Nidesh Chitrakar (nideshchitrakar)
///     Date: 01/08/2018
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace AddressVerification
{
    class Program
    {
        private const string USPSWebtoolUserID = "738STUDE3658";    // Enter your USPS userID here

        public static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Validator validator = new Validator(USPSWebtoolUserID, true);

            Address addr = new Address();

            var file = "/Users/nideshchitrakar/Documents/CHE database/address verification/Is-bad-address.csv";

            // open a file which is a CSV file with headers
            using (CsvReader csv = new CsvReader(new StreamReader(file), true))
            {
                int fieldCount = csv.FieldCount;

                string[] headers = csv.GetFieldHeaders();

                while (csv.ReadNextRecord())
                {
                    addr.Address2 = csv["Address"];
                    addr.City = csv["City"];
                    addr.State = csv["State"];
                    addr.Zip = csv["ZipCode"];

                    var result = validator.ValidateAddress(addr);

                    if (result.ContainsKey("Error"))
                    {
                        for (int i = 0; i < fieldCount; i++)
                            Console.Write(string.Format("{0} = {1};",
                                          headers[i], csv[i]));
                        Console.WriteLine();
                    }
                }
            }

            //foreach (KeyValuePair<string, string> kvp in result)
            //{
            //    Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            //}

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("Run Time: " + elapsedTime);
        }
    }
}
