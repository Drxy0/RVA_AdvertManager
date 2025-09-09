using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class LocationsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<Location> _locations;
        private ICollectionView _locationsView;
        private Location _newLocation;
        private string _errorMessage;

        public LocationsViewModel()
        {
            _proxy = new ClientProxy(
            new System.ServiceModel.NetTcpBinding(),
            new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            //dummy data for now, so we can add a location to real estate
            _locations = new ObservableCollection<Location>
            {
                new Location { Id = 1, City = "Belgrade", Country = "Serbia", PostalCode = "11000", Street = "Vuka Karadzica", StreetNumber = "10" },
                new Location { Id = 2, City = "Novi Sad", Country = "Serbia", PostalCode = "21000", Street = "Cara Lazara", StreetNumber = "33" },
                new Location { Id = 3, City = "Niš", Country = "Serbia", PostalCode = "18000", Street = "Kralja Petra", StreetNumber = "4" }
            };
            //LoadData();
            _locationsView = CollectionViewSource.GetDefaultView(_locations);

            NewLocation = new Location();
            AddEntityCommand = new MyICommand(OnAdd);
        }

        //private void LoadData()
        //{
        //    var locations = _proxy.GetAllLocations();
        //    _locations.Clear();
        //    foreach (var location in locations)
        //    {
        //        _locations.Add(location);
        //    }
        //}

        public ICollectionView LocationsView => _locationsView;
        public Location SelectedLocation { get; set; }

        public Location NewLocation
        {
            get => _newLocation;
            set
            {
                if (_newLocation != null)
                {
                    _newLocation.PropertyChanged -= NewLocation_PropertyChanged;
                }

                SetProperty(ref _newLocation, value);

                if (_newLocation != null)
                {
                    _newLocation.PropertyChanged += NewLocation_PropertyChanged;
                }

                OnPropertyChanged(nameof(CanAdd));
            }
        }

        private void NewLocation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Location.City) ||
                e.PropertyName == nameof(Location.Country) ||
                e.PropertyName == nameof(Location.PostalCode) ||
                e.PropertyName == nameof(Location.Street) ||
                e.PropertyName == nameof(Location.StreetNumber))
            {
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanAdd =>
            !string.IsNullOrWhiteSpace(NewLocation.City) &&
            !string.IsNullOrWhiteSpace(NewLocation.Country) &&
            !string.IsNullOrWhiteSpace(NewLocation.PostalCode) &&
            !string.IsNullOrWhiteSpace(NewLocation.Street) &&
            !string.IsNullOrWhiteSpace(NewLocation.StreetNumber) &&
            Regex.IsMatch(NewLocation.PostalCode, @"^\d+$") &&
            Regex.IsMatch(NewLocation.StreetNumber, @"^\d+$");


        public MyICommand AddEntityCommand { get; private set; }

        public void OnAdd()
        {
            if (!CanAdd)
            {
                ErrorMessage = "Please fill all fields";
                return;
            }

            ErrorMessage = string.Empty;

            //_proxy.AddLocation(NewLocation); // for later integration
            _locations.Add(NewLocation);

            NewLocation = new Location();

            _locationsView.Refresh();
        }
    }
}
