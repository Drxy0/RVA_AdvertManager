using AdvertManager.Domain.Entities;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Repositories;
using AdvertManager.Server.Service.Interfaces;
using System.Collections.Generic;

namespace AdvertManager.Server.Service
{
    public partial class DataService : IDataService
    {
        // TODO: Note, repository pattern might be overkill here since its simple CRUD.
        // Might want to remove it for simplicity even though it would be the 'correct' way of doing things.
        private readonly AdvertisementRepository _advertRepository;
        private IDataStorage _storage;
        private string _filePath;

        public DataService()
        {
            _advertRepository = new AdvertisementRepository();
            _storage = new JsonDataStorage();
            _filePath = "entities.json";

            LoadData();
        }

        public DataService(IDataStorage storage, string filePath)
        {
            _advertRepository = new AdvertisementRepository();
            _storage = storage;
            _filePath = filePath;

            LoadData();
        }

        public void SetStorage(IDataStorage storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;
        }

        public void AddAdvertisement(Advertisement ad)
        {
            _advertRepository.Add(ad);
        }

        public void UpdateAdvertisement(Advertisement ad)
        {
            _advertRepository.Update(ad);
        }

        public void DeleteAdvertisement(Advertisement ad)
        {
            if (ad != null)
            {
                _advertRepository.Delete(ad);
            }
        }

        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            return _advertRepository.GetAll();
        }

    }
}
