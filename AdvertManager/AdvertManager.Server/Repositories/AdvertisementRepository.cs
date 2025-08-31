using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    public class AdvertisementRepository : IRepository<Advertisement>
    {
        private readonly List<Advertisement> _advertisements = new List<Advertisement>();

        public void Add(Advertisement entity)
        {
            _advertisements.Add(entity);
        }
        public void Update(Advertisement entity)
        {
            var existing = _advertisements.FirstOrDefault
                (a => a.Title == entity.Title && a.Description == entity.Description);

            if (existing != null)
            {
                existing.Title = entity.Title;
                existing.Description = entity.Description;
                existing.CreatedAt = entity.CreatedAt;
                existing.ExpirationDate = entity.ExpirationDate;
                existing.Price = entity.Price;
                existing.Publisher = entity.Publisher;
                existing.RealEstate = entity.RealEstate;
                existing.SetState(entity.State);
            }
        }

        public void Delete(Advertisement entity)
        {
            _advertisements.Remove(entity);
        }

        public IEnumerable<Advertisement> GetAll()
        {
           return _advertisements;
        }

    }
}
