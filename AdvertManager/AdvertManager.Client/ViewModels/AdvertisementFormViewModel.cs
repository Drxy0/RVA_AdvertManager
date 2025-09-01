using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System;
using System.Windows;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementFormViewModel : BindableBase
    {
        private readonly Action<Advertisement> _onSave;
        private readonly Action _onCancel;
        private Advertisement _advertisement;
        private bool _isEditMode;

        private string _titleError;
        private string _priceError;
        private string _expirationDateError;

        public AdvertisementFormViewModel(Action<Advertisement> onSave, Action onCancel, bool isEditMode = false)
        {
            _onSave = onSave;
            _onCancel = onCancel;
            _isEditMode = isEditMode;

            Advertisement = new Advertisement
            {
                CreatedAt = DateTime.Now,
            };
            
            SaveCommand = new MyICommand(OnSave);
            CancelCommand = new MyICommand(OnCancel);
        }

        public Advertisement Advertisement
        {
            get => _advertisement;
            set => SetProperty(ref _advertisement, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        // Error properties
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

            _onSave?.Invoke(Advertisement);
        }

        private void OnCancel()
        {
            _onCancel?.Invoke();
        }
    }
}