namespace AdvertManager.Domain.Entities
{
    namespace AdvertManager.Domain.Entities
    {
        public class NewspaperAdvertisementAdapter : Advertisement
        {
            private NewspaperAdvertisement newspaperAdvertisement;

            public NewspaperAdvertisementAdapter(NewspaperAdvertisement newspaperAdvertisement)
            {
                this.newspaperAdvertisement = newspaperAdvertisement;
            }

            public NewspaperAdvertisement NewspaperAdvertisement => newspaperAdvertisement;
        }
    }
}
