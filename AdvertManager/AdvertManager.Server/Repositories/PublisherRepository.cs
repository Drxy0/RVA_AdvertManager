using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    internal class PublisherRepository : IRepository<Publisher>
    {
        private readonly List<Publisher> _publishers = new List<Publisher>();

        public void Add(Publisher entity)
        {
            _publishers.Add(entity);
        }

        public Publisher Get(int id)
        {
            return _publishers.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Publisher> GetAll()
        {
            return _publishers;
        }

        public void AddRange(IEnumerable<Publisher> entities)
        {
            _publishers.AddRange(entities);
        }

        public void Update(Publisher entity)
        {
            var existing = _publishers.FirstOrDefault(p => p.Id == entity.Id);

            if (existing != null)
            {
                existing.FirstName = entity.FirstName;
                existing.LastName = entity.LastName;
                existing.ContactNumber = entity.ContactNumber;
            }
        }

        public void Delete(Publisher entity)
        {
            _publishers.Remove(entity);
        }
    }
}
