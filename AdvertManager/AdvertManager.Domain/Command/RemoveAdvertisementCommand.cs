using System.Collections.ObjectModel;
using AdvertManager.Domain.Entities;

namespace AdvertManager.Domain.Command
{
    public class RemoveAdvertisementCommand : IAdvertisementCommand
    {
        private readonly ObservableCollection<Advertisement> _ads;
        private readonly Advertisement _advert;
        public Advertisement Advertisement { get { return _advert; } }
        public RemoveAdvertisementCommand(ObservableCollection<Advertisement> ads, Advertisement advert)
        {
            _ads = ads;
            _advert = advert;
        }

        public void Execute()
        {
            if (_ads.Contains(_advert))
                _ads.Remove(_advert);
        }

        public void Unexecute()
        {
            if (!_ads.Contains(_advert))
                _ads.Add(_advert);
        }
    }
}
