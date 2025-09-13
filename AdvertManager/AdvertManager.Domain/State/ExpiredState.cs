using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class ExpiredState : AdvertisementState
    {
        public override string Name => "Expired";
        public override void Handle()
        {
            if (advertisement.RealEstate != null)
            {
                advertisement.RealEstate.IsAvailable = false;
            }
            advertisement.NotifyObservers();

            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new ActiveState());
            });
        }
    }
}
