using AdvertManager.Domain.Entities;

namespace AdvertManager.Domain.Observer
{
    public interface IObserver
    {
        void Update(Advertisement advertisement);
    }
}
