using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class LocationsViewModel : BindableBase
    {
        private ObservableCollection<Location> _locations;
        private ICollectionView _locationsView;

        public MyICommand AddEntityCommand { get; private set; }

        public LocationsViewModel()
        {
            _locations = new ObservableCollection<Location>();
            _locationsView = CollectionViewSource.GetDefaultView(_locations);
            AddEntityCommand = new MyICommand(OnAdd);
        }

        public ICollectionView LocationsView => _locationsView;

        public void OnAdd()
        {
            // No logic for now - just the button exists
        }
    }
}