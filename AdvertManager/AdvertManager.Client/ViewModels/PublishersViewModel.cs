using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace AdvertManager.Client.ViewModels
{
    public class PublishersViewModel : BindableBase
    {
        private ClientProxy _proxy;
        private ObservableCollection<Publisher> _publishers;
        private ICollectionView _publishersView;
        private Publisher _newPublisher;
        private string _errorMessage;

        public PublishersViewModel()
        {
            _proxy = new ClientProxy(
            new System.ServiceModel.NetTcpBinding(),
            new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            _publishers = new ObservableCollection<Publisher>();
            //LoadData();
            _publishersView = CollectionViewSource.GetDefaultView(_publishers);

            NewPublisher = new Publisher();
            AddEntityCommand = new MyICommand(OnAdd);
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
        public Publisher SelectedPublisher { get; set; }

        public Publisher NewPublisher
        {
            get => _newPublisher;
            set
            {
                if (_newPublisher != null)
                {
                    _newPublisher.PropertyChanged -= NewPublisher_PropertyChanged;
                }

                SetProperty(ref _newPublisher, value);

                if (_newPublisher != null)
                {
                    _newPublisher.PropertyChanged += NewPublisher_PropertyChanged;
                }

                OnPropertyChanged(nameof(CanAdd));
            }
        }

        private void NewPublisher_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Publisher.FirstName) ||
                e.PropertyName == nameof(Publisher.LastName) ||
                e.PropertyName == nameof(Publisher.ContactNumber))
            {
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
                OnPropertyChanged(nameof(CanAdd));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        public bool CanAdd =>
            !string.IsNullOrWhiteSpace(NewPublisher?.FirstName) &&
            !string.IsNullOrWhiteSpace(NewPublisher?.LastName) &&
            !string.IsNullOrWhiteSpace(NewPublisher?.ContactNumber) &&
            NewPublisher.ContactNumber.All(char.IsDigit); // 👈 digits-only check

        public MyICommand AddEntityCommand { get; private set; }

        public void OnAdd()
        {
            if (!CanAdd)
            {
                ErrorMessage = "Please fill all fields (contact number must be digits only)";
                return;
            }

            ErrorMessage = string.Empty;

            //_proxy.AddPublisher(NewPublisher);
            _publishers.Add(NewPublisher);

            NewPublisher = new Publisher();

            _publishersView.Refresh();
        }
    }
}
    