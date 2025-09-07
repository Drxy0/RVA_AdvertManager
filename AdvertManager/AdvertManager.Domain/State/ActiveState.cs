using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class ActiveState : AdvertisementState
    {
        public override void Handle()
        {
            advertisement.RealEstate.IsAvailable = true;
            advertisement.NotifyObservers(); 
            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new RentedState());
            });
        }
    }
}
