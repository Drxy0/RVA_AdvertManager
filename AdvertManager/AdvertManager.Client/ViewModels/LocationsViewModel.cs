using AdvertManager.Domain.Entities;
using AdvertManager.Client.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class LocationsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<Location> _locations;
        private ICollectionView _locationsView;
        private Location _formLocation;
        private string _errorMessage;

        public LocationsViewModel()
        {
            _locations = new ObservableCollection<Location>();
            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            //if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            //{
            //    LoadData();
            //}
            //dummy data:
            _locations = new ObservableCollection<Location>
            {
                new Location("Belgrade", "Serbia", "11000", "Vuka Karadzica", "10"){ Id = 1},
                new Location("Novi Sad", "Serbia", "21000", "Cara Lazara", "33"){ Id = 2},
                new Location("Niš", "Serbia", "18000", "Kralja Petra", "4"){ Id = 3}
            };

            _locationsView = CollectionViewSource.GetDefaultView(_locations);

            FormLocation = new Location("", "", "", "", "");

            AddCommand = new MyICommand(OnAdd);
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

        public Location FormLocation
        {
            get => _formLocation;
            set => SetProperty(ref _formLocation, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public MyICommand AddCommand { get; }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(FormLocation.City) ||
                string.IsNullOrWhiteSpace(FormLocation.Country) ||
                string.IsNullOrWhiteSpace(FormLocation.PostalCode) ||
                string.IsNullOrWhiteSpace(FormLocation.Street) ||
                string.IsNullOrWhiteSpace(FormLocation.StreetNumber))
            {
                ErrorMessage = "All fields are required.";
                return false;
            }

            if (!Regex.IsMatch(FormLocation.PostalCode, @"^\d+$"))
            {
                ErrorMessage = "Postal code must be digits only.";
                return false;
            }

            if (!Regex.IsMatch(FormLocation.StreetNumber, @"^\d+$"))
            {
                ErrorMessage = "Street number must be digits only.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private void OnAdd()
        {
            if (!Validate()) return;

            int newId = _locations.Any() ? _locations.Max(l => l.Id) + 1 : 1;
            FormLocation.Id = newId;

            //_proxy.AddLocation(FormLocation);
            _locations.Add(FormLocation);

            FormLocation = new Location("", "", "", "", "");

            _locationsView.Refresh();
        }
    }
}
