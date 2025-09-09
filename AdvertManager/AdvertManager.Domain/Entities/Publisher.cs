using System.ComponentModel;

namespace AdvertManager.Domain.Entities
{
    public class Publisher
    {
        private int id;
        private string firstName;
        private string lastName;
        private string contactNumber;

        public int Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string ContactNumber { get => contactNumber; set => contactNumber = value; }
    }
}
