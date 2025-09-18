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
            try
            {
                if (advertisement == null) return;

                if (advertisement.RealEstate != null)
                {
                    advertisement.RealEstate.IsAvailable = false;
                }
            }
            catch { }
        }
    }
}
