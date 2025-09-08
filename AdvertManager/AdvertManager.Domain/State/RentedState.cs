using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class RentedState : AdvertisementState
    {
        public override void Handle()
        {
            advertisement.RealEstate.IsAvailable = false;
            advertisement.NotifyObservers();
            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new ExpiredState());
            });
        }
    }
}
