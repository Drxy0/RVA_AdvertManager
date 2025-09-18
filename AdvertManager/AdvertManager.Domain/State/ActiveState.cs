using System;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class ActiveState : AdvertisementState
    {
        public override string Name => "Active";
        public override void Handle()
        {
            try
            {
                if (advertisement == null) return;
                if (advertisement.RealEstate != null)
                {
                    advertisement.RealEstate.IsAvailable = true;
                }

                Task.Delay(5000).ContinueWith(_ =>
                {
                    if (advertisement == null) return;
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
            catch { }
        }
    }
}
