using System.Collections.Generic;

namespace AdvertManager.Server.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
    }
}
