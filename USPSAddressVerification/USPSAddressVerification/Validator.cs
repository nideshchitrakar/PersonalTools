﻿/*
    Validator.cs - Checks the validity of an address using USPS API.
                   Built using reference from USPS Web Tools Wrapper by
                   user johnnycantcode from codeproject.com
    Author: Nidesh Chitrakar (nideshchitrakar)
    Date: 01/08/2018
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace AddressVerification
{
    public class Validator
    {
        private const string ProductionUrl = "http://production.shippingapis.com/ShippingAPI.dll";
        private const string TestingUrl = "http://testing.shippingapis.com/ShippingAPITest.dll";
        private WebClient web;
        private string userid;
        private bool _TestMode;

        /// <summary>
        /// Creates a new USPS Address Validator instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        public Validator(string USPSWebtoolUserID)
        {
            web = new WebClient();
            userid = USPSWebtoolUserID;
            _TestMode = false;
        }

        /// <summary>
        /// Creates a new USPS Address Validator instance
        /// </summary>
        /// <param name="USPSWebtoolUserID">The UserID required by the USPS Web Tools</param>
        /// <param name="testmode">If True, then the USPS Test URL will be used.</param>
        public Validator(string USPSWebtoolUserID, bool testmode)
        {
            web = new WebClient();
            userid = USPSWebtoolUserID;
            _TestMode = testmode;
        }

        /// <summary>
        /// Determines if the Calls to the USPS server is made to the Test or Production server.
        /// </summary>
        public bool TestMode
        {
            get { return _TestMode; }
            set { _TestMode = value; }
        }

        /// <summary>
        /// Gets the URL depending on value of TestMode
        /// </summary>
        /// <returns>The URL.</returns>
        private string GetURL()
        {
            string url = ProductionUrl;
            if (TestMode)
                url = TestingUrl;
            return url;
        }

        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <param name="address">Address object to be validated</param>
        /// <returns>Dictionary representing the address or error.</returns>
        public Dictionary<string,string> ValidateAddress(Address address)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                string validateUrl = "?API=Verify&XML=<AddressValidateRequest USERID=\"{0}\">" +
                    "<Address>" +
                    "<Address1>{1}</Address1>" +
                    "<Address2>{2}</Address2>" +
                    "<City>{3}</City>" +
                    "<State>{4}</State>" +
                    "<Zip5>{5}</Zip5>" +
                    "<Zip4>{6}</Zip4>" +
                    "</Address>" +
                    "</AddressValidateRequest>";
                string url = GetURL() + validateUrl;
                url = String.Format(url,
                                    userid,
                                    address.Address1,
                                    address.Address2,
                                    address.City,
                                    address.State,
                                    address.Zip,
                                    address.ZipPlus4);

                string addressxml = web.DownloadString(url);
                result = Address.FromXml(addressxml);

                if (addressxml.Contains("<Error>"))
                {
                    int idx1 = addressxml.IndexOf("<Description>") + 13;
                    int idx2 = addressxml.IndexOf("</Description>");
                    string errDesc = addressxml.Substring(idx1, idx2 - idx1);

                    result.Add("Error", errDesc);
                }
                else if (addressxml.Contains("<Zip4/>"))
                {
                    result.Add("Action Required", "Zip4 missing. Address may not be deliverable.");
                }
                else if (addressxml.Contains("<ReturnText>"))
                {
                    int idx1 = addressxml.IndexOf("<ReturnText>") + 12;
                    int idx2 = addressxml.IndexOf("</ReturnText>");
                    string errDesc = addressxml.Substring(idx1, idx2 - idx1);

                    result.Add("Action Required", errDesc);
                }

                return result;
            }
            catch (WebException ex)
            {
                var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                Console.WriteLine(resp);
                //Console.ReadKey();

                result.Add("Exception",resp);
                return result;
            }
        }
    }
}
