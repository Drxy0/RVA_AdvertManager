using AdvertManager.Domain.Entities;

namespace AdvertManager.Domain.Command
{
    public class RemoveCommand : AdvertisementCommand
    {
        public RemoveCommand(Advertisement advertisement) : base(advertisement)
        {
        }

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
