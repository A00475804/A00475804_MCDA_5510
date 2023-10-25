using CsvHelper.Configuration.Attributes;

namespace ProgAssign1
{
    internal class Customer
    {
        [Name("First Name")]
        public string firstName { get; set; }
        [Name("Last Name")]
        public string lastName { get; set; }
        [Name("Street Number")]
        public int streetNumber { get; set; }
        [Name("Street")]
        public string street { get; set; }
        [Name("City")]
        public string city { get; set; }
        [Name("Province")]
        public string province { get; set; }
        [Name("Postal Code")]
        public string postalCode { get; set; }
        [Name("Country")]
        public string country { get; set; }
        [Name("Phone Number")]
        public int phoneNumber { get; set; }
        [Name("email Address")]
        public string email { get; set; }


        public override String ToString()
        {
            string value = "First Name " + firstName + " \nLast Name " + lastName + "\nStreet Number " + streetNumber
                            + "\nStreet " + street + "\n City " + city + " \nprovince " + province + " \npostal code "
                            + postalCode + " \ncountry " + country + " \nPhone Number " + phoneNumber + " \nemail Address "
                            + email + "\n";
            return value;
        }

    }
}
