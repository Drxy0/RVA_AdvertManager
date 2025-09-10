using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    public class LocationRepository : IRepository<Location>
    {
        private readonly List<Location> _locations = new List<Location>();

        public void Add(Location entity)
        {
            _locations.Add(entity);
        }

        public void AddRange(IEnumerable<Location> entities)
        {
            _locations.AddRange(entities);
        }

        public void Update(Location entity)
        {
            var existing = _locations.FirstOrDefault(l =>
                l.City == entity.City &&
                l.Country == entity.Country &&
                l.PostalCode == entity.PostalCode &&
                l.Street == entity.Street &&
                l.StreetNumber == entity.StreetNumber
            );

            if (existing != null)
            {
                existing.City = entity.City;
                existing.Country = entity.Country;
                existing.PostalCode = entity.PostalCode;
                existing.Street = entity.Street;
                existing.StreetNumber = entity.StreetNumber;
            }
        }

        public void Delete(Location entity)
        {
            _locations.Remove(entity);
        }

        public IEnumerable<Location> GetAll()
        {
            return _locations;
        }

        public Location Get(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
