using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using AdvertManager.Domain.State;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementFormViewModel : BindableBase
    {
        private readonly Action<Advertisement> _onSave;
        internal Action<bool> _onClose;
        private Advertisement _editableAdvertisement;
        private Advertisement _originalAdvertisement;
        private bool _isEditMode;

        private string _titleError;
        private string _priceError;
        private string _expirationDateError;

        private ClientProxy _proxy;

        public AdvertisementFormViewModel(
            Action<Advertisement> onSave,
            Action<bool> onClose,
            bool isEditMode = false,
            Advertisement existingAd = null)
        {
            _onSave = onSave;
            _onClose = onClose;
            _isEditMode = isEditMode;

            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            if (existingAd != null)
            {
                _originalAdvertisement = existingAd;
                Advertisement = CloneAdvertisement(existingAd);
            }
            else
            {
                Advertisement = new Advertisement { CreatedAt = DateTime.Now };
                Advertisement.SetState(new ActiveState());
            }

            LoadPublishers();
            LoadRealEstates();

            if (_isEditMode && Advertisement.Publisher != null)
            {
                SelectedPublisher = Publishers.FirstOrDefault(p => p.Id == Advertisement.Publisher.Id);
            }

            if (_isEditMode && Advertisement.RealEstate != null)
            {
                SelectedRealEstate = RealEstates.FirstOrDefault(r => r.Id == Advertisement.RealEstate.Id);
            }

            SaveCommand = new MyICommand(OnSave);
            CancelCommand = new MyICommand(OnCancel);
        }

        public Advertisement Advertisement
        {
            get => _editableAdvertisement;
            set => SetProperty(ref _editableAdvertisement, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public ObservableCollection<Publisher> Publishers { get; } = new ObservableCollection<Publisher>();
        public ObservableCollection<RealEstate> RealEstates { get; } = new ObservableCollection<RealEstate>();

        private Publisher _selectedPublisher;
        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set
            {
                SetProperty(ref _selectedPublisher, value);
                Advertisement.Publisher = value;
            }
        }

        private RealEstate _selectedRealEstate;
        public RealEstate SelectedRealEstate
        {
            get => _selectedRealEstate;
            set
            {
                SetProperty(ref _selectedRealEstate, value);
                Advertisement.RealEstate = value;
            }
        }

        private void LoadPublishers()
        {
            Publishers.Clear();
            foreach (var pub in _proxy.GetAllPublishers())
            {
                Publishers.Add(pub);
            }
        }

        private void LoadRealEstates()
        {
            RealEstates.Clear();
            foreach (var re in _proxy.GetAllRealEstates())
            {
                RealEstates.Add(re);
            }
        }

        public string TitleError
        {
            get => _titleError;
            set
            {
                SetProperty(ref _titleError, value);
                OnPropertyChanged(nameof(HasTitleError));
            }
        }

        public string PriceError
        {
            get => _priceError;
            set
            {
                SetProperty(ref _priceError, value);
                OnPropertyChanged(nameof(HasPriceError));
            }
        }

        public string ExpirationDateError
        {
            get => _expirationDateError;
            set
            {
                SetProperty(ref _expirationDateError, value);
                OnPropertyChanged(nameof(HasExpirationDateError));
            }
        }

        public bool HasTitleError => !string.IsNullOrEmpty(TitleError);
        public bool HasPriceError => !string.IsNullOrEmpty(PriceError);
        public bool HasExpirationDateError => !string.IsNullOrEmpty(ExpirationDateError);

        public MyICommand SaveCommand { get; }
        public MyICommand CancelCommand { get; }

        private Advertisement CloneAdvertisement(Advertisement source)
        {
            var clone = new Advertisement
            {
                Id = source.Id,
                Title = source.Title,
                Description = source.Description,
                CreatedAt = source.CreatedAt,
                ExpirationDate = source.ExpirationDate,
                Price = source.Price,
                Publisher = source.Publisher,
                RealEstate = source.RealEstate,
                StateName = source.StateName
            };
            if (source.State != null)
            {
                switch (source.StateName)
                {
                    case "Active":
                        clone.SetState(new ActiveState());
                        break;
                    case "Expired":
                        clone.SetState(new ExpiredState());
                        break;
                    default:
                        clone.SetState(new ActiveState());
                        break;
                }
            }

            return clone;
        }

        private string ValidateTitle()
        {
            if (string.IsNullOrWhiteSpace(Advertisement.Title))
                return "Title is required";

            if (Advertisement.Title.Length > 100)
                return "Title cannot exceed 100 characters";

            return null;
        }

        private string ValidatePrice()
        {
            if (Advertisement.Price < 0)
                return "Price cannot be negative";

            if (Advertisement.Price > 1000000000)
                return "Price is too high";

            return null;
        }

        private string ValidateExpirationDate()
        {
            if (Advertisement.ExpirationDate == default)
                return "Expiration date is required";

            if (Advertisement.ExpirationDate <= Advertisement.CreatedAt)
                return "Expiration date must be after creation date";

            if (Advertisement.ExpirationDate < DateTime.Today)
                return "Expiration date cannot be in the past";

            return null;
        }

        private bool IsFormValid()
        {
            TitleError = ValidateTitle();
            PriceError = ValidatePrice();
            ExpirationDateError = ValidateExpirationDate();

            return string.IsNullOrEmpty(TitleError) &&
                   string.IsNullOrEmpty(PriceError) &&
                   string.IsNullOrEmpty(ExpirationDateError);
        }

        private void OnSave()
        {
            if (!IsFormValid())
                return;
            var adToSave = _isEditMode ? Advertisement : Advertisement;

            _onSave?.Invoke(adToSave);
            _onClose?.Invoke(true);
        }

        private void OnCancel()
        {
            _onClose?.Invoke(false);
        }
    }
}
