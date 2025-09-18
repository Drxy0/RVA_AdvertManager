using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class RentedState : AdvertisementState
    {
        public override string Name => "Rented";
        public override void Handle()
        {
            try
            {
                if (advertisement == null) return;

                if (advertisement.RealEstate != null)
                {
                    advertisement.RealEstate.IsAvailable = false;
                }

                Task.Delay(5000).ContinueWith(_ =>
                {
                    if (advertisement == null) return;
                    if (DateTime.Now >= advertisement.ExpirationDate)
                        advertisement.SetState(new ExpiredState());
                    else
                        advertisement.SetState(new ActiveState());
                });
            }
            catch { } // ignore
        }
    }
}
