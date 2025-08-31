using System;
using System.Collections.Generic;
using System.Text;

namespace AdvertManager.Domain.State
{
    public class ExpiredState : AdvertisementState
    {
        public override void Handle()
        {
            throw new NotImplementedException();
        }
    }
}
