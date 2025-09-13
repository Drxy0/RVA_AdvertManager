using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class ActiveState : AdvertisementState
    {
        public override string Name => "Active";
        public override void Handle()
        {
            if (advertisement.RealEstate != null)
            {
                advertisement.RealEstate.IsAvailable = true;
            }

            advertisement.NotifyObservers();

            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new RentedState());
            });
        }
    }
}
