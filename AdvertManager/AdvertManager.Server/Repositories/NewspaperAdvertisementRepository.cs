using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    public class NewspaperAdvertisementRepository : IRepository<NewspaperAdvertisement>
    {
        private readonly List<NewspaperAdvertisement> _ads = new List<NewspaperAdvertisement>();

        public void Add(NewspaperAdvertisement entity)
        {
            _ads.Add(entity);
        }

        public void Update(NewspaperAdvertisement entity)
        {
            var existing = _ads.FirstOrDefault(a =>
                a.Title == entity.Title &&
                a.PublisherFullName == entity.PublisherFullName &&
                a.PhoneNumber == entity.PhoneNumber
            );

            if (existing != null)
            {
                existing.Title = entity.Title;
                existing.Description = entity.Description;
                existing.PublisherFullName = entity.PublisherFullName;
                existing.PhoneNumber = entity.PhoneNumber;
            }
        }

        public void Delete(NewspaperAdvertisement entity)
        {
            _ads.Remove(entity);
        }

        public IEnumerable<NewspaperAdvertisement> GetAll()
        {
            return _ads;
        }
    }
}
