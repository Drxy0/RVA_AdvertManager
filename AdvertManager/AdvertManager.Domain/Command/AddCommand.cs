using AdvertManager.Domain.Entities;
using System;

namespace AdvertManager.Domain.Command
{
    public class AddCommand : AdvertisementCommand
    {
        public AddCommand(Advertisement advertisement) : base(advertisement)
        {
        }

        public override void Execute()
        {
            //advertisement.DoSmth();
            throw new NotImplementedException();
        }
    }
}
