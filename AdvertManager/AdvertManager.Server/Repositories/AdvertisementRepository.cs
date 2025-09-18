using AdvertManager.Domain.Entities;
using AdvertManager.Domain.State;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    internal class AdvertisementRepository : IRepository<Advertisement>
    {
        private readonly List<Advertisement> _advertisements = new List<Advertisement>();

        public void Add(Advertisement entity)
        {
            _advertisements.Add(entity);
        }

        public void AddRange(IEnumerable<Advertisement> entities)
        {
            _advertisements.AddRange(entities);
        }

        public void Update(Advertisement entity)
        {
            var existing = _advertisements.FirstOrDefault(a => a.Id == entity.Id);
            if (existing != null)
            {
                existing.Title = entity.Title;
                existing.Description = entity.Description;
                existing.CreatedAt = entity.CreatedAt;
                existing.ExpirationDate = entity.ExpirationDate;
                existing.Price = entity.Price;
                existing.Publisher = entity.Publisher;
                existing.RealEstate = entity.RealEstate;
                AdvertisementState state;
                switch (entity.StateName)
                {
                    case "Active":
                        state = new ActiveState();
                        break;
                    case "Rented":
                        state = new RentedState();
                        break;
                    case "Expired":
                        state = new ExpiredState();
                        break;
                    default:
                        state = new ActiveState();
                        break;
                }

                existing.SetState(state);
            }
        }


        public void Delete(Advertisement entity)
        {
            var existing = _advertisements.FirstOrDefault(a => a.Id == entity.Id);
            if (existing != null)
            {
                _advertisements.Remove(existing);
            }
        }


        public IEnumerable<Advertisement> GetAll()
        {
           return _advertisements;
        }

        public Advertisement Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
