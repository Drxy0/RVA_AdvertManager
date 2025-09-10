using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class RealEstatesViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<RealEstate> _realEstates;
        private ObservableCollection<Location> _locations;
        private ICollectionView _realEstatesView;
        private RealEstate _formRealEstate;
        private string _areaInput;
        private string _yearBuiltInput;
        private string _errorMessage;

        public ObservableCollection<Location> Locations => _locations;
        public ObservableCollection<RealEstateType> RealEstateTypes { get; }

        public MyICommand AddCommand { get; }

        public RealEstatesViewModel(ObservableCollection<RealEstate> sharedRealEstates, ObservableCollection<Location> sharedLocations)
        {
            _proxy = new ClientProxy(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8000/Service"));

            _realEstates = sharedRealEstates;
            _locations = sharedLocations;

            RealEstateTypes = new ObservableCollection<RealEstateType>(
                (RealEstateType[])System.Enum.GetValues(typeof(RealEstateType))
            );

            _realEstatesView = CollectionViewSource.GetDefaultView(_realEstates);

            FormRealEstate = new RealEstate
            {
                Type = RealEstateType.HOUSE,
                IsAvailable = true,
            };

            AreaInput = "";
            YearBuiltInput = "";
            AddCommand = new MyICommand(OnAdd);
        }

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

            RealEstate realEstate = new RealEstate
            {
                Id = newId,
                AreaInSquareMeters = double.Parse(AreaInput),
                Type = FormRealEstate.Type,
                YearBuilt = int.Parse(YearBuiltInput),
                IsAvailable = FormRealEstate.IsAvailable,
                Location = FormRealEstate.Location
            };

            _proxy.AddRealEstate(realEstate);
            _realEstates.Add(realEstate);

            AreaInput = "";
            YearBuiltInput = "";
            FormRealEstate.Type = RealEstateType.HOUSE;
            FormRealEstate.IsAvailable = true;
            FormRealEstate.Location = _locations.FirstOrDefault();

            _realEstatesView.Refresh();
        }
    }
}
