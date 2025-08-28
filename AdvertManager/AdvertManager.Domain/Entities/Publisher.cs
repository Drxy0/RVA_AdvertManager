namespace AdvertManager.Domain.Entities
{
	public class Publisher
	{
		private string firstName;
		private string lastName;
        private string contactNumber;

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
    }
}
