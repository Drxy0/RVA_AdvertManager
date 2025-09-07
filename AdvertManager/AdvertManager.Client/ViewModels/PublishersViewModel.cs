using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class PublishersViewModel : BindableBase
    {
        private ObservableCollection<Publisher> _publishers;
        private ICollectionView _publishersView;

        public MyICommand AddEntityCommand { get; private set; }

        public PublishersViewModel()
        {
            _publishers = new ObservableCollection<Publisher>();

            _publishersView = CollectionViewSource.GetDefaultView(_publishers);

            AddEntityCommand = new MyICommand(OnAdd);
        }

        public ICollectionView PublishersView => _publishersView;

        public void OnAdd()
        {
            // No logic for now - just the button exists
        }

    }
}