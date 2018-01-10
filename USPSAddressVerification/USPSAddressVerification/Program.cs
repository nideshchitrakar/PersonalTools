/*
    Program.cs - Utilizes Validator and CSVReader classes to scan through
                 a csv file and check for address errors.
    Author: Nidesh Chitrakar (nideshchitrakar)
    Date: 01/08/2018
*/

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
        private const string USPSWebtoolUserID = "738STUDE3658";    // enter your USPS userID here

        public static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Validator validator = new Validator(USPSWebtoolUserID, true);

            Address addr = new Address();

            var file = "/Users/nideshchitrakar/Documents/CHE database/address verification/2017 PC Mail list/2017/Cohort 2017-Table 1.csv";
            int totalScanned = 0;
            int totalErrors = 0;
            int totalActionReq = 0;

            // replace the following with directory to write the error and correct addresses files in
            // files will be created if no files already exists
            var errorFile = "/Users/nideshchitrakar/Documents/CHE database/address verification/2017 PC Mail list/2017/error-list.csv";
            var correctFile = "/Users/nideshchitrakar/Documents/CHE database/address verification/2017 PC Mail list/2017/correct-list.csv";
            var actionFile = "/Users/nideshchitrakar/Documents/CHE database/address verification/2017 PC Mail list/2017/action-list.csv";

            CsvWriter errorWriter = new CsvWriter(errorFile);
            CsvWriter correctWriter = new CsvWriter(correctFile);
            CsvWriter actionWriter = new CsvWriter(actionFile);

            // open a file which is a CSV file with headers to read from
            using (CsvReader csv = new CsvReader(new StreamReader(file), true))
            {
                int fieldCount = csv.FieldCount;

                //var errorHeaders = csv.GetFieldHeaders().ToList();
                //errorHeaders.Add("Error");

                //var correctHeaders = csv.GetFieldHeaders().ToList();
                //correctHeaders.Add("Action Required");

                //errorWriter.WriteHeader(errorHeaders);
                //correctWriter.WriteHeader(correctHeaders);

                var headers = new List<string> { "StudentID", "FirstName", "LastName", "Address1", "Address2", "City", "State", "ZipCode" };

                var correctHeaders = new List<string>();
                correctHeaders.AddRange(headers);
                correctHeaders.Add("FormattedAddress");

                var errorHeaders = new List<string>();
                errorHeaders.AddRange(headers);
                errorHeaders.Add("Error");

                var actionHeaders = new List<string>();
                actionHeaders.AddRange(headers);
                actionHeaders.Add("FormattedAddress");
                actionHeaders.Add("Action Required");

                correctWriter.WriteHeader(correctHeaders);
                errorWriter.WriteHeader(errorHeaders);
                actionWriter.WriteHeader(actionHeaders);

                while (csv.ReadNextRecord())
                {
                    totalScanned += 1;

                    // the csv file must have headers Address, City, State, and ZipCode
                    // else change the following to match the csv headers
                    addr.Address1 = csv["Address2"];
                    addr.Address2 = csv["Address1"];
                    addr.City = csv["City"];
                    addr.State = csv["State"];
                    addr.Zip = csv["ZipCode"];

                    var result = validator.ValidateAddress(addr);

                    if (result.ContainsKey("Error"))
                    {
                        totalErrors += 1;

                        List<string> row = new List<string>();
                        //for (int i = 0; i < fieldCount; i++)
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            row.Add(csv[headers[i]]);
                        }
                        row.Add(result["Error"]);

                        errorWriter.AppendRow(row);
                    }
                    else if (result.ContainsKey("Action Required"))
                    {
                        totalActionReq += 1;

                        List<string> row = new List<string>();
                        //for (int i = 0; i < fieldCount; i++)
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            row.Add(csv[headers[i]]);
                        }
                        row.Add(result["Formatted Address"]);
                        row.Add(result["Action Required"]);

                        actionWriter.AppendRow(row);
                    }
                    else
                    {
                        List<string> row = new List<string>();
                        //for (int i = 0; i < fieldCount; i++)
                        for (int i = 0; i < headers.Count(); i++)
                        {
                            row.Add(csv[headers[i]]);
                        }
                        row.Add(result["Formatted Address"]);
                        //if (result.ContainsKey("Action Required"))
                        //{
                        //    totalActionReq += 1;
                        //    row.Add(result["Action Required"]);
                        //}

                        correctWriter.AppendRow(row);
                    }
                }
            }

            stopWatch.Stop();
            // get the elapsed time as a TimeSpan value
            TimeSpan ts = stopWatch.Elapsed;

            // format and display the TimeSpan value
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
