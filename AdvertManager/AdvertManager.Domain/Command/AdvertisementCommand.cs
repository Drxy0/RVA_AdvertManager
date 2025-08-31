using AdvertManager.Domain.Entities;

namespace AdvertManager.Domain.Command
{
	public abstract class AdvertisementCommand
	{
		protected Advertisement advertisement; // receiver

        protected AdvertisementCommand(Advertisement advertisement)
        {
            this.advertisement = advertisement;
        }

        public abstract void Execute();
	}
}
