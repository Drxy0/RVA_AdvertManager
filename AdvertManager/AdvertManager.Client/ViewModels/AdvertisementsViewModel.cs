using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementsViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<Advertisement> _advertisements;
        private ICollectionView _advertisementsView;
        private string _searchText;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Publisher> Publishers { get; }
        public ObservableCollection<RealEstate> RealEstates { get; }

        public MyICommand AddEntityCommand { get; private set; }
        public MyICommand UpdateEntityCommand { get; private set; }
        public MyICommand RemoveEntityCommand { get; private set; }

        private Advertisement _selectedAdvertisement;
        public Advertisement SelectedAdvertisement
        {
            get => _selectedAdvertisement;
            set
            {
                SetProperty(ref _selectedAdvertisement, value);
                UpdateEntityCommand.RaiseCanExecuteChanged();
                RemoveEntityCommand.RaiseCanExecuteChanged();
            }
        }

        public AdvertisementsViewModel(
            ObservableCollection<Advertisement> advertisements,
            ObservableCollection<Publisher> publishers,
            ObservableCollection<RealEstate> realEstates,
            IDialogService dialogService = null)
        {
            _proxy = new ClientProxy(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8000/Service"));

            _dialogService = dialogService ?? new DialogService();
            _advertisements = advertisements ?? new ObservableCollection<Advertisement>();
            Publishers = publishers ?? new ObservableCollection<Publisher>();
            RealEstates = realEstates ?? new ObservableCollection<RealEstate>();

            _advertisementsView = CollectionViewSource.GetDefaultView(_advertisements);
            _advertisementsView.Filter = FilterAdvertisements;

            AddEntityCommand = new MyICommand(OnAdd);
            UpdateEntityCommand = new MyICommand(OnUpdate, CanModify);
            RemoveEntityCommand = new MyICommand(OnRemove, CanModify);
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

        private void OnAdd()
        {
            var formViewModel = new AdvertisementFormViewModel(
                OnSaveAdvertisement,
                (result) => { /* Optional: do something on close */ },
                isEditMode: false);

            var dialogResult = _dialogService.ShowDialog(formViewModel, "Add New Advertisement");

            if (dialogResult == true)
            {
                _advertisementsView.Refresh();
            }
        }

        private void OnUpdate()
        {
            if (SelectedAdvertisement == null) return;

            var formViewModel = new AdvertisementFormViewModel(
                OnUpdateAdvertisement,
                (result) => { /* Optional */ },
                isEditMode: true,
                SelectedAdvertisement);

            var dialogResult = _dialogService.ShowDialog(formViewModel, "Edit Advertisement");

            if (dialogResult == true)
            {
                _advertisementsView.Refresh();
            }
        }

        private void OnSaveAdvertisement(Advertisement advertisement)
        {
            try
            {
                advertisement.CreatedAt = DateTime.Now;
                _advertisements.Add(advertisement);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving advertisement: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnUpdateAdvertisement(Advertisement advertisement)
        {
            try
            {
                advertisement.CreatedAt = SelectedAdvertisement.CreatedAt;
                _advertisementsView.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating advertisement: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnRemove()
        {
            if (SelectedAdvertisement == null) return;

            var result = MessageBox.Show(
                "Are you sure you want to delete this advertisement?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _advertisements.Remove(SelectedAdvertisement);
            }
        }

        private bool CanModify() => SelectedAdvertisement != null;

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
    }
}
