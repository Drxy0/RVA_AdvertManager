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

        //[OperationContract] void AddPublisher(Publisher publisher);
        //[OperationContract] IEnumerable<Publisher> GetAllPublishers();

        //[OperationContract] void AddLocation(Location location);
        //[OperationContract] IEnumerable<Location> GetAllLocations();

        //[OperationContract] void AddRealEstate(RealEstate realEstate);
        //[OperationContract] IEnumerable<RealEstate> GetAllRealEstates();

        //[OperationContract] void AddNewspaperAdvertisement(NewspaperAdvertisement newspaperAdvertisement);
        //[OperationContract] IEnumerable<NewspaperAdvertisement> GetAllNewspaperAdvertisements();
    }
}
