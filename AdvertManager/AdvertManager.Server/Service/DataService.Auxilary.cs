using AdvertManager.Domain.Entities;
using AdvertManager.Server.Service.Interfaces;
using System.Collections.Generic;

namespace AdvertManager.Server.Service
{
    public partial class DataService : IDataService
    {
        public void AddPublisher(Publisher publisher)
        {
            _publisherRepository.Add(publisher);
            SaveData();
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll();
        }

        public void AddRealEstate(RealEstate realEstate)
        {
            _realEstateRepository.Add(realEstate);
            SaveData();
        }

        public IEnumerable<RealEstate> GetAllRealEstates()
        {
            return _realEstateRepository.GetAll();
        }

        public void AddLocation(Location location)
        {
            _locationRepository.Add(location);
            SaveData();
        }

        public IEnumerable<Location> GetAllLocations()
        {
            return _locationRepository.GetAll();
        }

        public void AddNewspaperAdvertisement(NewspaperAdvertisement newspaperAdvertisement)
        {
            _newspaperAdvertRepository.Add(newspaperAdvertisement);
            SaveData();
        }

        public IEnumerable<NewspaperAdvertisement> GetAllNewspaperAdvertisements()
        {
            return _newspaperAdvertRepository.GetAll();
        }
    }
}
