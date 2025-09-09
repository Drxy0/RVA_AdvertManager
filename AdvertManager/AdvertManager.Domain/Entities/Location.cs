namespace AdvertManager.Domain.Entities
{
    public class Location
    {
        private int id;
        private string city;
        private string country;
        private string postalCode;
        private string street;
        private string streetNumber;

        public Location() { }

        public Location(string city, string country, string postalCode, string street, string streetNumber)
        {
            this.city = city;
            this.country = country;
            this.postalCode = postalCode;
            this.street = street;
            this.streetNumber = streetNumber;
        }

        public int Id { get => id; set => id = value; }
        public string City { get => city; set => city = value; }
        public string Country { get => country; set => country = value; }
        public string PostalCode { get => postalCode; set => postalCode = value; }
        public string Street { get => street; set => street = value; }
        public string StreetNumber { get => streetNumber; set => streetNumber = value; }
    }
}