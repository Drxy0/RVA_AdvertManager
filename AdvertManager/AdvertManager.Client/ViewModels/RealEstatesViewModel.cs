using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class RealEstatesViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<RealEstate> _realEstates;
        private ICollectionView _realEstatesView;
        private RealEstate _newRealEstate;
        private string _errorMessage;

        private string _areaInput;
        private string _yearBuiltInput;

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<RealEstateType> RealEstateTypes { get; set; }

        public MyICommand AddEntityCommand { get; private set; }

        public RealEstatesViewModel()
        {
            _proxy = new ClientProxy(
            new System.ServiceModel.NetTcpBinding(),
            new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            //dummy data for now, later will be pulled from locations repository
            Locations = new ObservableCollection<Location>
            {
                new Location { Id = 1, City = "Belgrade", Country = "Serbia", PostalCode = "11000", Street = "Vuka Karadzica", StreetNumber = "10" },
                new Location { Id = 2, City = "Novi Sad", Country = "Serbia", PostalCode = "21000", Street = "Cara Lazara", StreetNumber = "33" },
                new Location { Id = 3, City = "Niš", Country = "Serbia", PostalCode = "18000", Street = "Kralja Petra", StreetNumber = "4" }
            };

            RealEstateTypes = new ObservableCollection<RealEstateType>(
                (RealEstateType[])System.Enum.GetValues(typeof(RealEstateType))
            );

            _realEstates = new ObservableCollection<RealEstate>();
            //LoadData();
            _realEstatesView = CollectionViewSource.GetDefaultView(_realEstates);

            NewRealEstate = new RealEstate
            {
                Type = RealEstateType.HOUSE,
                IsAvailable = true,
                Location = Locations[0]
            };

            AreaInput = "";
            YearBuiltInput = "";

            AddEntityCommand = new MyICommand(OnAdd);
        }

        //private void LoadData()
        //{
        //    var realEstates = _proxy.GetAllRealEstates();
        //    _realEstates.Clear();
        //    foreach (var realEstate in realEstates)
        //    {
        //        _realEstates.Add(realEstate);
        //    }
        //    var locations = _proxy.GetAllLocations();
        //    Locations.Clear();
        //    foreach (var location in locations)
        //    {
        //        Locations.Add(location);
        //    }
        //}

        public ICollectionView RealEstatesView => _realEstatesView;
        public RealEstate SelectedRealEstate { get; set; }

        public RealEstate NewRealEstate
        {
            get => _newRealEstate;
            set
            {
                SetProperty(ref _newRealEstate, value);
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public string AreaInput
        {
            get => _areaInput;
            set
            {
                SetProperty(ref _areaInput, value);
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public string YearBuiltInput
        {
            get => _yearBuiltInput;
            set
            {
                SetProperty(ref _yearBuiltInput, value);
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
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanAdd =>
            double.TryParse(AreaInput, out double area) && area > 0 &&
            int.TryParse(YearBuiltInput, out int year) && year > 0 &&
            NewRealEstate.Type != 0 &&
            NewRealEstate.Location != null;

        public void OnAdd()
        {
            if (!CanAdd)
            {
                ErrorMessage = "Please fill all fields correctly";
                return;
            }

            ErrorMessage = string.Empty;

            //_proxy.AddRealEstate(new RealEstate
            //{
            //    AreaInSquareMeters = double.Parse(AreaInput),
            //    Type = NewRealEstate.Type,
            //    YearBuilt = int.Parse(YearBuiltInput),
            //    IsAvailable = NewRealEstate.IsAvailable,
            //    Location = NewRealEstate.Location
            //}););  for later integration
            _realEstates.Add(new RealEstate
            {
                AreaInSquareMeters = double.Parse(AreaInput),
                Type = NewRealEstate.Type,
                YearBuilt = int.Parse(YearBuiltInput),
                IsAvailable = NewRealEstate.IsAvailable,
                Location = NewRealEstate.Location
            });

            // Reset inputs
            AreaInput = "";
            YearBuiltInput = "";
            NewRealEstate.Type = RealEstateType.HOUSE;
            NewRealEstate.IsAvailable = true;
            NewRealEstate.Location = Locations[0];

            _realEstatesView.Refresh();
        }
    }
}
