using System;
using System.Collections.Generic;
using LumenWorks.Framework.IO.Csv;

namespace AddressVerification
{
    class Program
    {
        private const string USPSWebtoolUserID = "738STUDE3658";

        public static void Main(string[] args)
        {
            Validator validator = new Validator(USPSWebtoolUserID);

            Address addr = new Address();
            addr.Address2 = "One College Drive";
            addr.City = "Bennington";
            addr.State = "VT";
            addr.Zip = "05201";

            var result = validator.ValidateAddress(addr);

            foreach (KeyValuePair<string, string> kvp in result)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
        }
    }
}
