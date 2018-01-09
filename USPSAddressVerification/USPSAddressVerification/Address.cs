using System;
using System.Collections.Generic;
using System.Text;

namespace AddressVerification
{
    public class Address
    {
        private string _FirmName = "";
        /// <summary>
        /// Name of the Firm or Company
        /// </summary>
        public string FirmName
        {
            get { return _FirmName; }
            set { _FirmName = value; }
        }

        private string _Address1 = "";
        /// <summary>
        /// Address Line 1 is used to provide an apartment or suite
        /// number, if applicable. Maximum characters allowed: 38
        /// </summary>
        public string Address1
        {
            get { return _Address1; }
            set { _Address1 = value; }
        }

        private string _Address2 = "";
        /// <summary>
        /// Street address
        /// Maximum characters allowed: 38
        /// </summary>
        public string Address2
        {
            get { return _Address2; }
            set { _Address2 = value; }
        }

        private string _City = "";
        /// <summary>
        /// City
        /// Either the City and State or Zip are required.
        /// Maximum characters allowed: 15
        /// </summary>
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _State = "";
        /// <summary>
        /// State
        /// Either the City and State or Zip are required.
        /// Maximum characters allowed = 2
        /// </summary>
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        private string _Zip = "";
        /// <summary>
        /// Zipcode
        /// Maximum characters allowed = 5
        /// </summary>
        public string Zip
        {
            get { return _Zip; }
            set { _Zip = value; }
        }

        private string _ZipPlus4 = "";
        /// <summary>
        /// Zip code extension
        /// Maximum characters allowed = 4
        /// </summary>
        public string ZipPlus4
        {
            get { return _ZipPlus4; }
            set { _ZipPlus4 = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        // FromXML medthod provided by viperguynaz via codeproject
        //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Get a dictionary object representing an address from a xml string.
        /// </summary>
        /// <param name="xml">XML representation of an Address Object</param>
        /// <returns>Dictionary object containing address information</returns>
        public static Dictionary<string,string> FromXml(string xml)
        {
            Dictionary<string, string> address = new Dictionary<string, string>();

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);

            System.Xml.XmlNode element = doc.SelectSingleNode("/AddressValidateResponse/Address/FirmName");
            if (element != null)
                address.Add("Company Name", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/Address1");
            if (element != null)
                address.Add("Apt / Suite / Other", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/Address2");
            if (element != null)
                address.Add("Street Address", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/City");
            if (element != null)
                address.Add("City", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/State");
            if (element != null)
                address.Add("State", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/Zip5");
            if (element != null)
                address.Add("Zip5", element.InnerText);
            element = doc.SelectSingleNode("/AddressValidateResponse/Address/Zip4");
            if (element != null)
                address.Add("Zip4", element.InnerText);

            return address;
        }

        /// <summary>
        /// Get the Xml representation of this address object
        /// </summary>
        /// <returns>String</returns>
        public string ToXml()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Address1>" + this._Address1 + "</Address1>");
            sb.Append("<Address2>" + this._Address2 + "</Address2>");
            sb.Append("<City>" + this.City + "</City>");
            sb.Append("<State>" + this.State + "</State>");
            sb.Append("<Zip5>" + this.Zip + "</Zip5>");
            sb.Append("<Zip4>" + this.ZipPlus4 + "</Zip4>");
            sb.Append("</Address>");
            return sb.ToString();
        }
    }
}
