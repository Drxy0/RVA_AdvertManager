using System.ComponentModel;

namespace AdvertManager.Domain.Entities
{
    public class Publisher : INotifyPropertyChanged
    {
        private int id;
        private string firstName;
        private string lastName;
        private string contactNumber;

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

        public string FirstName
        {
            get => firstName;
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                }
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged(nameof(LastName));
                }
            }
        }

        public string ContactNumber
        {
            get => contactNumber;
            set
            {
                if (contactNumber != value)
                {
                    contactNumber = value;
                    OnPropertyChanged(nameof(ContactNumber));
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
