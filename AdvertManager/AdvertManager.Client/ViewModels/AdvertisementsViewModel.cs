using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public MyICommand AddEntityCommand { get; private set; }
        public MyICommand UpdateEntityCommand { get; private set; }
        public MyICommand RemoveEntityCommand { get; private set; }

        public AdvertisementsViewModel() : this(new DialogService())
        {
        }

        public AdvertisementsViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _advertisements = new ObservableCollection<Advertisement>();

            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                LoadData();
            }

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

        public void OnAdd()
        {
            var formViewModel = new AdvertisementFormViewModel(
                OnSaveAdvertisement,
                () => { /* Cancel logic if needed */ },
                isEditMode: false);

            var result = _dialogService.ShowDialog(formViewModel, "Add New Advertisement");

            if (result == true)
            {
                LoadData(); // Refresh the data after adding
            }
        }

        private void OnUpdate()
        {
            if (SelectedAdvertisement == null) return;

            var formViewModel = new AdvertisementFormViewModel(
                OnUpdateAdvertisement,
                () => { /* Cancel logic */ },
                isEditMode: true)
            {
                Advertisement = SelectedAdvertisement
            };

            _dialogService.ShowDialog(formViewModel, "Edit Advertisement");
        }


        private void OnSaveAdvertisement(Advertisement advertisement)
        {
            try
            {
                // Set creation date for new advertisements
                advertisement.CreatedAt = DateTime.Now;

                _proxy.AddAdvertisement(advertisement);
                _advertisements.Add(advertisement);

                // Close the dialog
                Application.Current.Windows.OfType<Window>().LastOrDefault()?.Close();
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
                // Preserve the original creation date when updating
                var originalCreatedAt = SelectedAdvertisement.CreatedAt;
                advertisement.CreatedAt = originalCreatedAt;

                _proxy.UpdateAdvertisement(advertisement);
                LoadData(); // Refresh data

                Application.Current.Windows.OfType<Window>().LastOrDefault()?.Close();
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
                try
                {
                    _proxy.DeleteAdvertisement(SelectedAdvertisement);
                    _advertisements.Remove(SelectedAdvertisement);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting advertisement: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanModify()
        {
            return SelectedAdvertisement != null;
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
