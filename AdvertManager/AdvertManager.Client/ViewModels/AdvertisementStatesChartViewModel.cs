using AdvertManager.Domain.Entities;
using AdvertManager.Client.Helpers;
using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Threading;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementStateChartViewModel : BindableBase
    {
        private ClientProxy _proxy;

        private PieSeries activeSeries;
        private PieSeries rentedSeries;
        private PieSeries expiredSeries;

        public SeriesCollection SeriesCollection { get; private set; }
        public ObservableCollection<Advertisement> Advertisements { get; private set; }

        private readonly DispatcherTimer timer;

        public AdvertisementStateChartViewModel()
        {
            _proxy = new ClientProxy(
                new System.ServiceModel.NetTcpBinding(),
                new System.ServiceModel.EndpointAddress("net.tcp://localhost:8000/Service"));

            Advertisements = new ObservableCollection<Advertisement>();

            InitializeSeries();
            LoadData(); // initial load

            timer = new DispatcherTimer
            {
                Interval = System.TimeSpan.FromSeconds(1)
            };
            timer.Tick += (s, e) => PollAndUpdate();
            timer.Start();
        }

        private void InitializeSeries()
        {
            activeSeries = new PieSeries { Title = "Active", Values = new ChartValues<double> { 0 }, DataLabels = true };
            rentedSeries = new PieSeries { Title = "Rented", Values = new ChartValues<double> { 0 }, DataLabels = true };
            expiredSeries = new PieSeries { Title = "Expired", Values = new ChartValues<double> { 0 }, DataLabels = true };

            SeriesCollection = new SeriesCollection { activeSeries, rentedSeries, expiredSeries };
        }

        private void PollAndUpdate()
        {
            var adverts = _proxy.GetAllAdvertisements();

            Advertisements.Clear();
            foreach (var ad in adverts)
                Advertisements.Add(ad);
            activeSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Active");
            rentedSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Rented");
            expiredSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Expired");
        }

        private void LoadData()
        {
            var adverts = _proxy.GetAllAdvertisements();
            Advertisements.Clear();
            foreach (var ad in adverts)
                Advertisements.Add(ad);

            activeSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Active");
            rentedSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Rented");
            expiredSeries.Values[0] = (double)Advertisements.Count(a => a.StateName == "Expired");
        }
    }
}
