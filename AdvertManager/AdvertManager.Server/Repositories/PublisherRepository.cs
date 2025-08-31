using AdvertManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Repositories
{
    public class PublisherRepository : IRepository<Publisher>
    {
        private readonly List<Publisher> _publishers = new List<Publisher>();

        public void Add(Publisher entity)
        {
            _publishers.Add(entity);
        }

        public void Update(Publisher entity)
        {
            var existing = _publishers.FirstOrDefault(p =>
                p.FirstName == entity.FirstName &&
                p.LastName == entity.LastName &&
                p.ContactNumber == entity.ContactNumber
            );

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

        public IEnumerable<Publisher> GetAll()
        {
            return _publishers;
        }
    }
}
