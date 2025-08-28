namespace AdvertManager.Domain.Entities
{
	public class NewspaperAdvertisementAdapter : Advertisement
	{
		private NewspaperAdvertisement newspaperAdvert;

        public NewspaperAdvertisementAdapter(NewspaperAdvertisement newspaperAdvert)
        {
            this.newspaperAdvert = newspaperAdvert;
        }
    }
}
