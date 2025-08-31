using AdvertManager.Domain.Entities;
using AdvertManager.Server.DataStorage;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvertManager.Server.Services
{
    public partial class DataService : IDataService
    {
        private List<Advertisement> _advertisements = new List<Advertisement>();
        private IDataStorage _storage;
        private string _filePath;

        public DataService(IDataStorage storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;

            if (File.Exists(filePath))
            {
                var loaded = _storage.Load<List<Advertisement>>(filePath);
                if (loaded != null)
                    _advertisements.AddRange(loaded);
            }
        }

        // Allow switching storage format at runtime
        public void SetStorage(IDataStorage storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;
        }

        public void AddAdvertisement(Advertisement ad)
        {
            _advertisements.Add(ad);
        }

        public void UpdateAdvertisement(Advertisement ad)
        {
            var existing = _advertisements.FirstOrDefault(a => a.GetHashCode() == ad.GetHashCode()); // ideally use an Id
            if (existing != null)
            {
                _advertisements.Remove(existing);
                _advertisements.Add(ad);
            }
        }

        public void DeleteAdvertisement(int adId)
        {
            var ad = _advertisements.FirstOrDefault(a => a.GetHashCode() == adId);
            if (ad != null)
            {
                _advertisements.Remove(ad);
            }
        }

        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            return _advertisements;
        }
    }
}
