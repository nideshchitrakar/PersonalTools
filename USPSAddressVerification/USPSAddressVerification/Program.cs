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
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using USPSAddressVerification;

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
            int totalScanned = 0;
            int totalErrors = 0;
            int totalActionReq = 0;

            var errorFile = "/Users/nideshchitrakar/Documents/CHE database/address verification/error-list.csv";
            var correctFile = "/Users/nideshchitrakar/Documents/CHE database/address verification/correct-list.csv";

            CsvWriter errorWriter = new CsvWriter(errorFile);
            CsvWriter correctWriter = new CsvWriter(correctFile);

            // open a file which is a CSV file with headers
            using (CsvReader csv = new CsvReader(new StreamReader(file), true))
            {
                int fieldCount = csv.FieldCount;

                var errorHeaders = csv.GetFieldHeaders().ToList();
                errorHeaders.Add("Error");

                var correctHeaders = csv.GetFieldHeaders().ToList();
                correctHeaders.Add("Action Required");

                errorWriter.WriteHeader(errorHeaders);
                correctWriter.WriteHeader(correctHeaders);

                while (csv.ReadNextRecord())
                {
                    totalScanned += 1;

                    addr.Address2 = csv["Address"];
                    addr.City = csv["City"];
                    addr.State = csv["State"];
                    addr.Zip = csv["ZipCode"];

                    var result = validator.ValidateAddress(addr);

                    if (result.ContainsKey("Error"))
                    {
                        totalErrors += 1;

                        List<string> row = new List<string>();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            row.Add(csv[i]);
                        }
                        row.Add(result["Error"]);

                        errorWriter.AppendRow(row);
                    }
                    else if (result.ContainsKey("Action Required"))
                    {
                        totalActionReq += 1;

                        List<string> row = new List<string>();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            row.Add(csv[i]);
                        }
                        row.Add(result["Action Required"]);

                        correctWriter.AppendRow(row);
                    }
                    else
                    {
                        List<string> row = new List<string>();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            row.Add(csv[i]);
                        }

                        correctWriter.AppendRow(row);
                    }
                }
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine();
            Console.WriteLine("Run Time: " + elapsedTime);
            Console.WriteLine("Total records scanned: " + totalScanned);
            Console.WriteLine("Total errors detected: " + totalErrors);
            Console.WriteLine("Total actions required: " + totalActionReq);
        }
    }
}
