////////////////////////////////////////////////////////////////////////////////
///     Program.cs - Utilizes Validator and CSVReader classes to scan through
///                  a csv file and check for address errors.
///     Author: Nidesh Chitrakar (nideshchitrakar)
///     Date: 01/08/2018
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

namespace AddressVerification
{
    class Program
    {
        private const string USPSWebtoolUserID = "738STUDE3658";    // Enter your USPS userID here

        public static void Main(string[] args)
        {
            Validator validator = new Validator(USPSWebtoolUserID);

            Address addr = new Address();
            addr.Address2 = "1930 N. Mansards Blvd.";
            addr.City = "Griffith";
            addr.State = "IN";
            addr.Zip = "46319";

            var result = validator.ValidateAddress(addr);

            foreach (KeyValuePair<string, string> kvp in result)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
