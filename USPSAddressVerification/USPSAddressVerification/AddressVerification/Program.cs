using System;
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
            Console.WriteLine(result.Values);
        }
    }
}
