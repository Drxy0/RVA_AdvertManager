using AdvertManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertManager.Domain.Observer
{
    public interface IObserver
    {
        void Update(Advertisement advertisement);
    }
}
