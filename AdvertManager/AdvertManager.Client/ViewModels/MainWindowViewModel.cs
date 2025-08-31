using AdvertManager.Client.Helpers;
using System.Data;

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

        public MainWindowViewModel()
        {
            // ConnectToBackendService(); // TODO

            advertisementsViewModel = new AdvertisementsViewModel();
            realEstatesViewModel = new RealEstatesViewModel();
            locationsViewModel = new LocationsViewModel();
            publishersViewModel = new PublishersViewModel();
            newspaperAdvertsViewModel = new NewspaperAdvertsViewModel();

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
            }
        }
    }
}
