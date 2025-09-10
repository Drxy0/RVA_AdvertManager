using AdvertManager.Domain.Entities;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Service;
using AdvertManager.Server.Service.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace AdvertManager.Client
{
    public class ClientProxy : ChannelFactory<IDataService>, IDataService
    {
        IDataService factory;
        public void SetStorage(IStorageType type, string filePath)
        {
            factory.SetStorage(type, filePath);
        }

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void AddAdvertisement(Advertisement ad)
        {
            factory.AddAdvertisement(ad);
        }

        public void UpdateAdvertisement(Advertisement ad)
        {
            factory.UpdateAdvertisement(ad);
        }

        public void DeleteAdvertisement(Advertisement ad)
        {
            factory.DeleteAdvertisement(ad);
        }

        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            return factory.GetAllAdvertisements();
        }


        //// AUXILIARY ////

        public void AddPublisher(Publisher publisher)
        {
            factory.AddPublisher(publisher);
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return factory.GetAllPublishers();
        }

        public void AddRealEstate(RealEstate realEstate)
        {
            factory.AddRealEstate(realEstate);
        }

        public IEnumerable<RealEstate> GetAllRealEstates()
        {
            return factory.GetAllRealEstates();
        }

        public void AddLocation(Location location)
        {
            factory.AddLocation(location);
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return factory.GetAllLocations();
        }

        public void AddNewspaperAdvertisement(NewspaperAdvertisement newspaperAdvertisement)
        {
            factory.AddNewspaperAdvertisement(newspaperAdvertisement);
        }

        public IEnumerable<NewspaperAdvertisement> GetAllNewspaperAdvertisements()
        {
            return factory.GetAllNewspaperAdvertisements();
        }
    }
}
