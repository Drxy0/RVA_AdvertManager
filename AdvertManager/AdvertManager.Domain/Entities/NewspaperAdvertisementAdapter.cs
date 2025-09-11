using System;

namespace AdvertManager.Domain.Entities
{
    public class NewspaperAdvertisementAdapter : Advertisement
    {
        private readonly NewspaperAdvertisement _newspaperAd;

        public NewspaperAdvertisementAdapter(NewspaperAdvertisement newspaperAdvertisement)
        {
            _newspaperAd = newspaperAdvertisement ?? throw new ArgumentNullException(nameof(newspaperAdvertisement));

            Id = _newspaperAd.Id;
            Title = _newspaperAd.Title;
            Description = _newspaperAd.Description;

            Publisher = new Publisher
            {
                Id = -1,
                FirstName = _newspaperAd.PublisherFullName,
                LastName = string.Empty,
                ContactNumber = _newspaperAd.PhoneNumber
            };

            RealEstate = null;
            Price = 0;
        }
    }
}
