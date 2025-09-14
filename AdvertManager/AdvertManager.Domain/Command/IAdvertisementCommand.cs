using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertManager.Domain.Command
{
    public interface IAdvertisementCommand
    {
        void Execute();
        void Unexecute();
    }
}
