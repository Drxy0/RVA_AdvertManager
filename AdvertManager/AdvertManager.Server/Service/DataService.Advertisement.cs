using AdvertManager.Domain.Entities;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Repositories;
using AdvertManager.Server.Service.Interfaces;
using System.Collections.Generic;

namespace AdvertManager.Server.Service
{
    public partial class DataService : IDataService
    {
        public void AddAdvertisement(Advertisement ad)
        {
            _advertRepository.Add(ad);
            SaveData();
        }

        public void UpdateAdvertisement(Advertisement ad)
        {
            _advertRepository.Update(ad);
            SaveData();
        }

        public void DeleteAdvertisement(Advertisement ad)
        {
            if (ad != null)
            {
                _advertRepository.Delete(ad);
                SaveData();
            }
        }

        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            return _advertRepository.GetAll();
        }
    }
}
