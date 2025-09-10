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
        private ObservableCollection<Publisher> _publishers;
        private ICollectionView _publishersView;
        private Publisher _formPublisher;
        private string _errorMessage;

        public PublishersViewModel()
        {
            _publishers = new ObservableCollection<Publisher>();
            _proxy = new ClientProxy(
                new NetTcpBinding(),
                new EndpointAddress("net.tcp://localhost:8000/Service"));

            _publishersView = CollectionViewSource.GetDefaultView(_publishers);

            FormPublisher = new Publisher();

            AddCommand = new MyICommand(OnAdd);

            // Sample data
            _publishers.Add(new Publisher { Id = 1, FirstName = "John", LastName = "Doe", ContactNumber = "1234567890" });
            _publishers.Add(new Publisher { Id = 2, FirstName = "Jane", LastName = "Smith", ContactNumber = "0987654321" });
        }

        //private void LoadData()
        //{
        //    var publishers = _proxy.GetAllPublishers();
        //    _publishers.Clear();
        //    foreach (var publisher in publishers)
        //    {
        //        _publishers.Add(publisher);
        //    }
        //}

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

        public MyICommand AddCommand { get; }

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

            var newId = _publishers.Any() ? _publishers.Max(p => p.Id) + 1 : 1;
            FormPublisher.Id = newId;

            _proxy.AddPublisher(FormPublisher);

            _publishers.Add(FormPublisher);

            FormPublisher = new Publisher();
            _publishersView.Refresh();
        }
    }
}
