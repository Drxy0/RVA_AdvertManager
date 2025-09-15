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

            Task.Delay(10000).ContinueWith(_ =>
            {
                if (DateTime.Now >= advertisement.ExpirationDate)
                {
                    advertisement.SetState(new ExpiredState());
                }
                else
                {
                    advertisement.SetState(new ActiveState());
                }
            });
        }
    }
}
