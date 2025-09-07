using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class RealEstatesViewModel : BindableBase
    {
        private ObservableCollection<RealEstate> _realEstates;
        private ICollectionView _realEstatesView;
        private string _searchText;

        public MyICommand AddEntityCommand { get; private set; }

        public RealEstatesViewModel()
        {
            _realEstates = new ObservableCollection<RealEstate>();

            _realEstatesView = CollectionViewSource.GetDefaultView(_realEstates);
            _realEstatesView.Filter = FilterRealEstates;

            AddEntityCommand = new MyICommand(OnAdd);
        }

        public ICollectionView RealEstatesView => _realEstatesView;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _realEstatesView.Refresh();
            }
        }

        public void OnAdd()
        {
            // No logic for now - just the button exists
        }

        private bool FilterRealEstates(object obj)
        {
            var realEstate = obj as RealEstate;
            if (realEstate == null) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            var term = SearchText.ToLower();

            bool matches = realEstate.AreaInSquareMeters.ToString().Contains(term) ||
                          realEstate.Type.ToString().ToLower().Contains(term) ||
                          realEstate.YearBuilt.ToString().Contains(term) ||
                          realEstate.IsAvailable.ToString().ToLower().Contains(term);

            if (realEstate.Location != null)
            {
                matches |= realEstate.Location.City?.ToLower().Contains(term) == true ||
                          realEstate.Location.Country?.ToLower().Contains(term) == true ||
                          realEstate.Location.PostalCode?.ToLower().Contains(term) == true ||
                          realEstate.Location.Street?.ToLower().Contains(term) == true ||
                          realEstate.Location.StreetNumber?.ToLower().Contains(term) == true;
            }

            return matches;
        }
    }
}