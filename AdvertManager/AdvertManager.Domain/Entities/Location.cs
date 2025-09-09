using System.ComponentModel;

namespace AdvertManager.Domain.Entities
{
    public class Location : INotifyPropertyChanged
    {
        private int id;
        private string city;
        private string country;
        private string postalCode;
        private string street;
        private string streetNumber;

        public Location()
        {
        }

        public Location(string city, string country, string postalCode, string street, string streetNumber)
        {
            this.city = city;
            this.country = country;
            this.postalCode = postalCode;
            this.street = street;
            this.streetNumber = streetNumber;
        }

        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string City
        {
            get => city;
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged(nameof(City));
                }
            }
        }

        public string Country
        {
            get => country;
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged(nameof(Country));
                }
            }
        }

        public string PostalCode
        {
            get => postalCode;
            set
            {
                if (postalCode != value)
                {
                    postalCode = value;
                    OnPropertyChanged(nameof(PostalCode));
                }
            }
        }

        public string Street
        {
            get => street;
            set
            {
                if (street != value)
                {
                    street = value;
                    OnPropertyChanged(nameof(Street));
                }
            }
        }

        public string StreetNumber
        {
            get => streetNumber;
            set
            {
                if (streetNumber != value)
                {
                    streetNumber = value;
                    OnPropertyChanged(nameof(StreetNumber));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
