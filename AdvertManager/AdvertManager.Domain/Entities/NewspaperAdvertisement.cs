namespace AdvertManager.Domain.Entities
{
	public class NewspaperAdvertisement : Entity
	{
		private string title;
		private string description;
		private string publisherFullName;
        private string phoneNumber;

        public NewspaperAdvertisement(string title, string description, string publisherFullName, string phoneNumber)
        {
            this.title = title;
            this.description = description;
            this.publisherFullName = publisherFullName;
            this.phoneNumber = phoneNumber;
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string PublisherFullName { get => publisherFullName; set => publisherFullName = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
    }
}
