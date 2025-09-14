using AdvertManager.Domain.Entities;
using AdvertManager.Client.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.ServiceModel;
using System;

namespace AdvertManager.Client.ViewModels
{
    public class NewspaperAdvertsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ICollectionView _adsView;
        private NewspaperAdvertisement _formAd;
        private string _errorMessage;

        public ObservableCollection<NewspaperAdvertisement> Ads { get; }
        public ObservableCollection<Publisher> Publishers { get; }

        public ObservableCollection<Advertisement> AllAds { get; }

        public event Action<NewspaperAdvertisement> NewspaperAdAdded;

        public MyICommand AddCommand { get; }

        // Constructor for injecting shared collections
        public NewspaperAdvertsViewModel(
            ObservableCollection<NewspaperAdvertisement> newspaperAds,
            ObservableCollection<Publisher> publishers,
            ObservableCollection<Advertisement> advertisements)
        {
            _proxy = new ClientProxy(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8000/Service"));

            Ads = newspaperAds ?? new ObservableCollection<NewspaperAdvertisement>();
            Publishers = publishers ?? new ObservableCollection<Publisher>();
            AllAds = advertisements ?? new ObservableCollection<Advertisement>();

            _adsView = CollectionViewSource.GetDefaultView(Ads);

            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            FormAd = new NewspaperAdvertisement("", "", "", "");

            AddCommand = new MyICommand(OnAdd);
        }

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

            int maxNewspaperId = Ads.Any() ? Ads.Max(a => a.Id) + 1 : 1;
            int maxAdId = AllAds.Any() ? AllAds.Max(a => a.Id) + 1 : 1;
            FormAd.Id = Math.Max(maxNewspaperId, maxAdId);

            _proxy.AddNewspaperAdvertisement(FormAd);

            Ads.Add(FormAd);

            NewspaperAdAdded?.Invoke(FormAd);
            FormAd = new NewspaperAdvertisement("", "", "", "");

            _adsView.Refresh();
        }
    }
}
