using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.ServiceModel;

namespace AdvertManager.Server.Service.Interfaces
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract] void AddAdvertisement(Advertisement ad);
        [OperationContract] void UpdateAdvertisement(Advertisement ad);
        [OperationContract] void DeleteAdvertisement(Advertisement ad);
        [OperationContract] IEnumerable<Advertisement> GetAllAdvertisements();
    }
}
