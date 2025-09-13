using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class RentedState : AdvertisementState
    {
        public override string Name => "Rented";
        public override void Handle()
        {
            if (advertisement.RealEstate != null)
            {
                advertisement.RealEstate.IsAvailable = false;
            }
            advertisement.NotifyObservers();

            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new ExpiredState());
            });
        }
    }
}
