using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdvertManager.Domain.State
{
    public class ExpiredState : AdvertisementState
    {
        public override void Handle()
        {
            advertisement.RealEstate.IsAvailable = false;
            advertisement.NotifyObservers(); 
            Task.Delay(30000).ContinueWith(_ =>
            {
                advertisement.SetState(new ActiveState());
            });
        }
    }
}
