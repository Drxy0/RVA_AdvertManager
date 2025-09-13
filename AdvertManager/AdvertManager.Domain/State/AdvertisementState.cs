using AdvertManager.Domain.Entities;
using System.Runtime.Remoting.Contexts;

namespace AdvertManager.Domain.State
{
	public abstract class AdvertisementState
	{
        protected Advertisement advertisement;

        public abstract string Name { get; }

        public void SetAdvertisement(Advertisement advertisement)
        {
            this.advertisement = advertisement;
        }

        public abstract void Handle();
    }
}
