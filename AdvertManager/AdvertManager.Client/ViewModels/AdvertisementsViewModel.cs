using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<Advertisement> _advertisements;
        private ICollectionView _advertisementsView;
        private string _searchText;

        public AdvertisementsViewModel()
        {
            _advertisements = new ObservableCollection<Advertisement>();

            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            LoadData();

            // Wrap collection so we can filter it
            _advertisementsView = CollectionViewSource.GetDefaultView(_advertisements);
            _advertisementsView.Filter = FilterAdvertisements;
        }

        public ICollectionView AdvertisementsView => _advertisementsView;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _advertisementsView.Refresh(); // triggers filtering
            }
        }

        private bool FilterAdvertisements(object obj)
        {
            var ad = obj as Advertisement;
            if (ad == null) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            var term = SearchText.ToLower();

            // Search in Advertisement properties
            bool matches =
                ad.Title?.ToLower().Contains(term) == true ||
                ad.Description?.ToLower().Contains(term) == true ||
                ad.Price.ToString().Contains(term) ||
                ad.CreatedAt.ToString("d").Contains(term) ||
                ad.ExpirationDate.ToString("d").Contains(term);

            if (ad.Publisher != null)
            {
                matches |= ad.Publisher.FirstName?.ToLower().Contains(term) == true;
                matches |= ad.Publisher.LastName?.ToLower().Contains(term) == true;
                matches |= ad.Publisher.ContactNumber?.ToLower().Contains(term) == true;
            }

            if (ad.RealEstate != null)
            {
                matches |= ad.RealEstate.AreaInSquareMeters.ToString().Contains(term);
                matches |= ad.RealEstate.Type.ToString().ToLower().Contains(term);
                matches |= ad.RealEstate.YearBuilt.ToString().Contains(term);
                matches |= ad.RealEstate.IsAvailable.ToString().ToLower().Contains(term);
            }

            return matches;
        }


        private void LoadData()
        {
            var ads = _proxy.GetAllAdvertisements();
            _advertisements.Clear();
            foreach (var ad in ads)
            {
                _advertisements.Add(ad);
            }
        }
    }
}
