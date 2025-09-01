using AdvertManager.Domain.Entities;
using AdvertManager.Server.Service.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace AdvertManager.Client
{
    public class ClientProxy : ChannelFactory<IDataService>, IDataService
    {
        IDataService factory;

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
    }
}
