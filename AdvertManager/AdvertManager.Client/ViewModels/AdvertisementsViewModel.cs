using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Command;
using AdvertManager.Domain.Entities;
using AdvertManager.Domain.State;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Windows;
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
        private readonly IDialogService _dialogService;
        private readonly CommandManager _commandManager = new CommandManager();

        public ObservableCollection<Publisher> Publishers { get; }
        public ObservableCollection<RealEstate> RealEstates { get; }

        public MyICommand AddEntityCommand { get; private set; }
        public MyICommand UpdateEntityCommand { get; private set; }
        public MyICommand RemoveEntityCommand { get; private set; }
        public MyICommand UndoCommand { get; private set; }
        public MyICommand RedoCommand { get; private set; }

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
            UndoCommand = new MyICommand(OnUndo, CanUndo);
            RedoCommand = new MyICommand(OnRedo, CanRedo);
        }

        public ICollectionView AdvertisementsView => _advertisementsView;

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _advertisementsView.Refresh();
            }
        }

        private void OnAdd()
        {
            var formViewModel = new AdvertisementFormViewModel(
                OnSaveAdvertisement,
                (result) => { },
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
                (result) => { },
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
                int newId = _advertisements.Any() ? _advertisements.Max(a => a.Id) + 1 : 1;
                advertisement.Id = newId;
                advertisement.CreatedAt = DateTime.Now;
                advertisement.SetState(new ActiveState());
                _proxy.AddAdvertisement(advertisement);
                var cmd = new AddAdvertisementCommand(_advertisements, advertisement);
                _commandManager.ExecuteCommand(cmd);
                UndoCommand.RaiseCanExecuteChanged();
                RedoCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving advertisement: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnUpdateAdvertisement(Advertisement editedAd)
        {
            if (SelectedAdvertisement == null) return;

            try
            {
                var original = SelectedAdvertisement;

                var oldCopy = new Advertisement
                {
                    Id = original.Id,
                    Title = original.Title,
                    Description = original.Description,
                    CreatedAt = original.CreatedAt,
                    ExpirationDate = original.ExpirationDate,
                    Price = original.Price,
                    Publisher = original.Publisher,
                    RealEstate = original.RealEstate,
                    StateName = original.StateName,
                };

                var newCopy = new Advertisement
                {
                    Id = editedAd.Id,
                    Title = editedAd.Title,
                    Description = editedAd.Description,
                    CreatedAt = original.CreatedAt,
                    ExpirationDate = editedAd.ExpirationDate,
                    Price = editedAd.Price,
                    Publisher = editedAd.Publisher,
                    RealEstate = editedAd.RealEstate,
                    StateName = editedAd.StateName,
                };

                var cmd = new UpdateAdvertisementCommand(original, oldCopy, newCopy);
                _commandManager.ExecuteCommand(cmd);
                _proxy.UpdateAdvertisement(newCopy);

                _advertisementsView.Refresh();
                UndoCommand.RaiseCanExecuteChanged();
                RedoCommand.RaiseCanExecuteChanged();
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
                _proxy.DeleteAdvertisement(SelectedAdvertisement);
                var cmd = new RemoveAdvertisementCommand(_advertisements, SelectedAdvertisement);
                _commandManager.ExecuteCommand(cmd);
                UndoCommand.RaiseCanExecuteChanged();
                RedoCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanModify() => SelectedAdvertisement != null;

        private bool CanUndo() => _commandManager.CanUndo;
        private bool CanRedo() => _commandManager.CanRedo;

        private void OnUndo()
        {
            var lastCommand = _commandManager.PeekUndo();
            if (lastCommand is AddAdvertisementCommand addCmd)
            {
                _proxy.DeleteAdvertisement(addCmd.Advertisement);
            }
            else if (lastCommand is RemoveAdvertisementCommand removeCmd)
            {
                _proxy.AddAdvertisement(removeCmd.Advertisement);
            }
            else if (lastCommand is UpdateAdvertisementCommand updateCmd)
            {
                _proxy.UpdateAdvertisement(updateCmd.OldAd);
            }

            _commandManager.Undo();
            _advertisementsView.Refresh();
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private void OnRedo()
        {
            var lastCommand = _commandManager.PeekRedo();
            if (lastCommand is AddAdvertisementCommand addCmd)
            {
                _proxy.AddAdvertisement(addCmd.Advertisement);
            }
            else if (lastCommand is RemoveAdvertisementCommand removeCmd)
            {
                _proxy.DeleteAdvertisement(removeCmd.Advertisement);
            }
            else if (lastCommand is UpdateAdvertisementCommand updateCmd)
            {
                _proxy.UpdateAdvertisement(updateCmd.NewAd);
            }
            _commandManager.Redo();
            _advertisementsView.Refresh();
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private bool FilterAdvertisements(object obj)
        {
            var ad = obj as Advertisement;
            if (ad == null) return false;
            if (string.IsNullOrWhiteSpace(SearchText)) return true;

            var term = SearchText.ToLower();
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
