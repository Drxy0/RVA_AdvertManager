using AdvertManager.Domain.Entities;

namespace AdvertManager.Domain.Command
{
    public class UpdateAdvertisementCommand : IAdvertisementCommand
    {
        private readonly Advertisement _advert;
        private readonly Advertisement _oldValues;
        private readonly Advertisement _newValues;
        public Advertisement OldAd { get { return _oldValues; } }
        public Advertisement NewAd { get { return _newValues; } }
        public UpdateAdvertisementCommand(Advertisement advert, Advertisement oldValues, Advertisement newValues)
        {
            _advert = advert;
            _oldValues = oldValues;
            _newValues = newValues;
        }

        public void Execute()
        {
            Apply(_newValues);
        }

        public void Unexecute()
        {
            Apply(_oldValues);
        }

        private void Apply(Advertisement source)
        {
            _advert.Title = source.Title;
            _advert.Description = source.Description;
            _advert.CreatedAt = source.CreatedAt;
            _advert.ExpirationDate = source.ExpirationDate;
            _advert.Price = source.Price;
            _advert.Publisher = source.Publisher;
            _advert.RealEstate = source.RealEstate;
            if (!string.IsNullOrEmpty(source.StateName))
            {
                _advert.StateName = source.StateName;
                switch (source.StateName)
                {
                    case "Active":
                        _advert.SetState(new State.ActiveState());
                        break;
                    case "Rented":
                        _advert.SetState(new State.RentedState());
                        break;
                    case "Expired":
                        _advert.SetState(new State.ExpiredState());
                        break;
                }
            }

            _advert.NotifyObservers();
        }
    }
}
