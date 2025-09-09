using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using AdvertManager.Client.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class RealEstatesViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<RealEstate> _realEstates;
        private ICollectionView _realEstatesView;
        private RealEstate _formRealEstate;
        private string _areaInput;
        private string _yearBuiltInput;
        private string _errorMessage;

        public ObservableCollection<Location> Locations { get; set; }
        public ObservableCollection<RealEstateType> RealEstateTypes { get; set; }

        public MyICommand AddCommand { get; }

        public RealEstatesViewModel()
        {
            _realEstates = new ObservableCollection<RealEstate>();
            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            //if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            //{
            //    LoadData();
            //}
            //dummy data:
            Locations = new ObservableCollection<Location>
            {
                new Location("Belgrade", "Serbia", "11000", "Vuka Karadzica", "10"){ Id = 1 },
                new Location("Novi Sad", "Serbia", "21000", "Cara Lazara", "33"){ Id = 2 },
                new Location("Niš", "Serbia", "18000", "Kralja Petra", "4"){ Id = 3 }
            };

            RealEstateTypes = new ObservableCollection<RealEstateType>(
                (RealEstateType[])System.Enum.GetValues(typeof(RealEstateType))
            );

            _realEstatesView = CollectionViewSource.GetDefaultView(_realEstates);

            FormRealEstate = new RealEstate
            {
                Type = RealEstateType.HOUSE,
                IsAvailable = true,
                Location = Locations[0]
            };

            AreaInput = "";
            YearBuiltInput = "";

            AddCommand = new MyICommand(OnAdd);
        }

        //private void LoadData()
        //{
        //    var realEstates = _proxy.GetAllRealEstates();
        //    _realEstates.Clear();
        //    foreach (var realEstate in realEstates)
        //    {
        //        _realEstates.Add(realEstate);
        //    }
        //
        //    var locations = _proxy.GetAllLocations();
        //    Locations.Clear();
        //    foreach (var location in locations)
        //    {
        //        Locations.Add(location);
        //    }
        //}

        public ICollectionView RealEstatesView => _realEstatesView;
        public RealEstate SelectedRealEstate { get; set; }

        public RealEstate FormRealEstate
        {
            get => _formRealEstate;
            set => SetProperty(ref _formRealEstate, value);
        }

        public string AreaInput
        {
            get => _areaInput;
            set => SetProperty(ref _areaInput, value);
        }

        public string YearBuiltInput
        {
            get => _yearBuiltInput;
            set => SetProperty(ref _yearBuiltInput, value);
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


        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(AreaInput) ||
                string.IsNullOrWhiteSpace(YearBuiltInput) ||
                FormRealEstate.Location == null)
            {
                ErrorMessage = "All fields are required.";
                return false;
            }

            if (!double.TryParse(AreaInput, out double area))
            {
                ErrorMessage = "Area must be a number.";
                return false;
            }

            if (!int.TryParse(YearBuiltInput, out int year))
            {
                ErrorMessage = "Year built must be a number.";
                return false;
            }

            if (area < 10 || area > 500)
            {
                ErrorMessage = "Area must be between 10 and 500 m².";
                return false;
            }

            if (year < 1800 || year > 2025)
            {
                ErrorMessage = "Year built must be between 1800 and 2025.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private void OnAdd()
        {
            if (!Validate()) return;

            int newId = _realEstates.Any() ? _realEstates.Max(r => r.Id) + 1 : 1;

            //_proxy.AddRealEstate(new RealEstate
            //{
            //    Id = newId,
            //    AreaInSquareMeters = double.Parse(AreaInput),
            //    Type = FormRealEstate.Type,
            //    YearBuilt = int.Parse(YearBuiltInput),
            //    IsAvailable = FormRealEstate.IsAvailable,
            //    Location = FormRealEstate.Location
            //});
            _realEstates.Add(new RealEstate
            {
                Id = newId,
                AreaInSquareMeters = double.Parse(AreaInput),
                Type = FormRealEstate.Type,
                YearBuilt = int.Parse(YearBuiltInput),
                IsAvailable = FormRealEstate.IsAvailable,
                Location = FormRealEstate.Location
            });

            AreaInput = "";
            YearBuiltInput = "";
            FormRealEstate.Type = RealEstateType.HOUSE;
            FormRealEstate.IsAvailable = true;
            FormRealEstate.Location = Locations[0];

            _realEstatesView.Refresh();
        }
    }
}
