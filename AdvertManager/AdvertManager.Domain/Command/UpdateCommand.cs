using AdvertManager.Domain.Entities;
using System;

namespace AdvertManager.Domain.Command
{
    public class UpdateCommand : AdvertisementCommand
    {
        public UpdateCommand(Advertisement advertisement) : base(advertisement)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
