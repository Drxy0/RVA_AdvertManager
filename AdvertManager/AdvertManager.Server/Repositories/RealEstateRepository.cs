using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace AdvertManager.Server.Repositories
{
    public class RealEstateRepository : IRepository<RealEstate>
    {
        private readonly List<RealEstate> _realEstates = new List<RealEstate>();

        public void Add(RealEstate entity)
        {
            _realEstates.Add(entity);
        }

        public void AddRange(IEnumerable<RealEstate> entities)
        {
            _realEstates.AddRange(entities);
        }

        public void Update(RealEstate entity)
        {
            var existing = _realEstates.FirstOrDefault(r =>
                r.AreaInSquareMeters == entity.AreaInSquareMeters &&
                r.Type == entity.Type &&
                r.YearBuilt == entity.YearBuilt &&
                r.IsAvailable == entity.IsAvailable
            );

            if (existing != null)
            {
                existing.AreaInSquareMeters = entity.AreaInSquareMeters;
                existing.Type = entity.Type;
                existing.YearBuilt = entity.YearBuilt;
                existing.IsAvailable = entity.IsAvailable;
            }
        }

        public void Delete(RealEstate entity)
        {
            _realEstates.Remove(entity);
        }

        public IEnumerable<RealEstate> GetAll()
        {
            return _realEstates;
        }
    }
}
