using AdvertManager.Domain.Entities;
using AdvertManager.Client.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class NewspaperAdvertsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<NewspaperAdvertisement> _ads;
        private ICollectionView _adsView;
        private NewspaperAdvertisement _formAd;
        private string _errorMessage;

        public ObservableCollection<NewspaperAdvertisement> Ads => _ads;
        public MyICommand AddCommand { get; }

        public NewspaperAdvertsViewModel()
        {
            _ads = new ObservableCollection<NewspaperAdvertisement>();
            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            //if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            //{
            //    LoadData();
            //}

            // Dummy data
            _ads = new ObservableCollection<NewspaperAdvertisement>
            {
                new NewspaperAdvertisement("Sale: Old Car", "Selling a used car in good condition", "John Doe", "1234567890") { Id = 1 },
                new NewspaperAdvertisement("House Rent", "Spacious 3-bedroom house for rent", "Jane Smith", "9876543210") { Id = 2 }
            };

            _adsView = CollectionViewSource.GetDefaultView(_ads);

            FormAd = new NewspaperAdvertisement("", "", "", "");

            AddCommand = new MyICommand(OnAdd);
        }

        //private void LoadData()
        //{
        //    var newspaperAdvertisements = _proxy.GetAllNewspaperAdvertisements();
        //    _ads.Clear();
        //    foreach (var newspaperAdvertisement in newspaperAdvertisements)
        //    {
        //        _ads.Add(newspaperAdvertisement);
        //    }
        //}

        public ICollectionView AdsView => _adsView;

        public NewspaperAdvertisement FormAd
        {
            get => _formAd;
            set => SetProperty(ref _formAd, value);
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
            if (string.IsNullOrWhiteSpace(FormAd.Title) ||
                string.IsNullOrWhiteSpace(FormAd.Description) ||
                string.IsNullOrWhiteSpace(FormAd.PublisherFullName) ||
                string.IsNullOrWhiteSpace(FormAd.PhoneNumber))
            {
                ErrorMessage = "All fields are required.";
                return false;
            }

            if (!FormAd.PublisherFullName.Trim().Contains(" "))
            {
                ErrorMessage = "Publisher full name must contain at least first and last name.";
                return false;
            }

            if (!Regex.IsMatch(FormAd.PhoneNumber, @"^\d+$"))
            {
                ErrorMessage = "Phone number must contain digits only.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private void OnAdd()
        {
            if (!Validate()) return;

            int newId = _ads.Any() ? _ads.Max(a => a.Id) + 1 : 1;
            FormAd.Id = newId;

            //_proxy.AddNewspaperAdvertisement(FormAd);
            _ads.Add(FormAd);

            FormAd = new NewspaperAdvertisement("", "", "", "");

            _adsView.Refresh();
        }
    }
}
