using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading;

namespace AdvertManager.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private BindableBase currentViewModel;

        private AdvertisementsViewModel advertisementsViewModel;
        private RealEstatesViewModel realEstatesViewModel;
        private LocationsViewModel locationsViewModel;
        private PublishersViewModel publishersViewModel;
        private NewspaperAdvertsViewModel newspaperAdvertsViewModel;
        private AdvertisementStateChartViewModel advertisementStateChartViewModel;

        // Shared collections
        public ObservableCollection<Location> Locations { get; }
        public ObservableCollection<RealEstate> RealEstates { get; }
        public ObservableCollection<Publisher> Publishers { get; }
        public ObservableCollection<Advertisement> Advertisements { get; }
        public ObservableCollection<Advertisement> AdvertisementsOnly { get; }
        public ObservableCollection<NewspaperAdvertisement> NewspaperAdverts { get; }

        private ClientProxy _proxy;
        
        public MainWindowViewModel()
        {
            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            Locations = new ObservableCollection<Location>();
            RealEstates = new ObservableCollection<RealEstate>();
            Publishers = new ObservableCollection<Publisher>();
            Advertisements = new ObservableCollection<Advertisement>();
            NewspaperAdverts = new ObservableCollection<NewspaperAdvertisement>();

            LoadData();

            advertisementsViewModel = new AdvertisementsViewModel(Advertisements, Publishers, RealEstates);
            realEstatesViewModel = new RealEstatesViewModel(RealEstates, Locations);
            locationsViewModel = new LocationsViewModel(Locations);
            publishersViewModel = new PublishersViewModel(Publishers);
            newspaperAdvertsViewModel = new NewspaperAdvertsViewModel(NewspaperAdverts, Publishers, Advertisements);
            advertisementStateChartViewModel = new AdvertisementStateChartViewModel();

            newspaperAdvertsViewModel.NewspaperAdAdded += (newsAd) =>
            {
                var adapter = new NewspaperAdvertisementAdapter(newsAd);
                var matchedPublisher = Publishers.FirstOrDefault(p =>
                            $"{p.FirstName} {p.LastName}".Equals(newsAd.PublisherFullName, StringComparison.OrdinalIgnoreCase));
                if (matchedPublisher == null)
                {
                    matchedPublisher = Publishers.FirstOrDefault(p =>
                        p.ContactNumber.Equals(newsAd.PhoneNumber, StringComparison.OrdinalIgnoreCase));
                }
                if (matchedPublisher != null)
                {
                    adapter.Publisher = new Publisher
                    {
                        Id = matchedPublisher.Id,
                        FirstName = matchedPublisher.FirstName,
                        LastName = matchedPublisher.LastName,
                        ContactNumber = matchedPublisher.ContactNumber
                    };
                }
                Advertisements.Add(adapter);
            };

            CurrentViewModel = advertisementsViewModel; // default view

            NavCommand = new MyICommand<string>(OnNav);
        }

        public MyICommand<string> NavCommand { get; private set; }

        public BindableBase CurrentViewModel
        {
            get { return currentViewModel; }
            set
            {
                SetProperty(ref currentViewModel, value);
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "advertisements":
                    CurrentViewModel = advertisementsViewModel;
                    break;
                case "realEstates":
                    CurrentViewModel = realEstatesViewModel;
                    break;
                case "locations":
                    CurrentViewModel = locationsViewModel;
                    break;
                case "publishers":
                    CurrentViewModel = publishersViewModel;
                    break;
                case "newspaperAdverts":
                    CurrentViewModel = newspaperAdvertsViewModel;
                    break;
                case "advertisementChart":
                    CurrentViewModel = advertisementStateChartViewModel;
                    break;
            }
        }



        private void LoadData()
        {
            int retries = 5;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    Locations.Clear();
                    foreach (var loc in _proxy.GetAllLocations())
                        Locations.Add(loc);

                    RealEstates.Clear();
                    foreach (var re in _proxy.GetAllRealEstates())
                        RealEstates.Add(re);

                    Publishers.Clear();
                    foreach (var pub in _proxy.GetAllPublishers())
                        Publishers.Add(pub);

                    Advertisements.Clear();
                    foreach (var ad in _proxy.GetAllAdvertisements()) {
                        Advertisements.Add(ad);
                    }
                    NewspaperAdverts.Clear();
                    foreach (NewspaperAdvertisement newsAd in _proxy.GetAllNewspaperAdvertisements())
                    {
                        NewspaperAdverts.Add(newsAd);

                        // Try to match a Publisher by full name
                        var matchedPublisher = Publishers.FirstOrDefault(p =>
                            $"{p.FirstName} {p.LastName}".Equals(newsAd.PublisherFullName, StringComparison.OrdinalIgnoreCase));

                        // If no match by name, try by phone number
                        if (matchedPublisher == null)
                        {
                            matchedPublisher = Publishers.FirstOrDefault(p =>
                                p.ContactNumber.Equals(newsAd.PhoneNumber, StringComparison.OrdinalIgnoreCase));
                        }

                        var adapter = new NewspaperAdvertisementAdapter(newsAd);
                        
                        if (matchedPublisher != null)
                        {
                            adapter.Publisher = new Publisher
                            {
                                Id = matchedPublisher.Id,
                                FirstName = matchedPublisher.FirstName,
                                LastName = matchedPublisher.LastName,
                                ContactNumber = matchedPublisher.ContactNumber
                            };
                        }

                        // Add the adapter to the main Advertisements collection
                        Advertisements.Add(adapter);
                    }

                    return; // success, exit
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    if (i == retries - 1)
                        throw; // rethrow after max retries

                    Thread.Sleep(2000); // wait and retry
                }
            }
        }
    }
}
