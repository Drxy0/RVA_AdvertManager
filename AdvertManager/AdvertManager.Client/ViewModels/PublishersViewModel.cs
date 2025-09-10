using AdvertManager.Domain.Entities;
using AdvertManager.Client.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.ServiceModel;

namespace AdvertManager.Client.ViewModels
{
    public class PublishersViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private Publisher _formPublisher;
        private string _errorMessage;
        private ICollectionView _publishersView;

        public ObservableCollection<Publisher> Publishers { get; }

        public MyICommand AddCommand { get; }

        // Constructor for injecting shared collection
        public PublishersViewModel(ObservableCollection<Publisher> publishers)
        {
            _proxy = new ClientProxy(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8000/Service"));

            Publishers = publishers ?? new ObservableCollection<Publisher>();

            _publishersView = CollectionViewSource.GetDefaultView(Publishers);

            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            FormPublisher = new Publisher();

            AddCommand = new MyICommand(OnAdd);
        }

        public ICollectionView PublishersView => _publishersView;

        public Publisher FormPublisher
        {
            get => _formPublisher;
            set => SetProperty(ref _formPublisher, value);
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
            if (string.IsNullOrWhiteSpace(FormPublisher.FirstName) ||
                string.IsNullOrWhiteSpace(FormPublisher.LastName) ||
                string.IsNullOrWhiteSpace(FormPublisher.ContactNumber))
            {
                ErrorMessage = "All fields are required.";
                return false;
            }

            if (!FormPublisher.ContactNumber.Any(char.IsDigit))
            {
                ErrorMessage = "Contact number must contain digits.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        private void OnAdd()
        {
            if (!Validate()) return;

            int newId = Publishers.Any() ? Publishers.Max(p => p.Id) + 1 : 1;
            FormPublisher.Id = newId;

            _proxy.AddPublisher(FormPublisher);

            Publishers.Add(FormPublisher);
            FormPublisher = new Publisher();

            _publishersView.Refresh();
        }
    }
}
