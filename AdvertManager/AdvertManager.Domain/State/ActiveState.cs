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

            Task.Delay(10000).ContinueWith(_ =>
            {
                if (DateTime.Now >= advertisement.ExpirationDate)
                {
                    advertisement.SetState(new ExpiredState());
                }
                else
                {
                    advertisement.SetState(new RentedState());
                }
            });
        }
    }
}
