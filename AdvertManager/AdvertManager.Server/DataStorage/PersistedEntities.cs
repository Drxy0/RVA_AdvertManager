using AdvertManager.Domain.Entities;
using System.Collections.Generic;

namespace AdvertManager.Server.DataStorage
{
    public class PersistedEntities
    {
        public List<Advertisement> Advertisements { get; set; } = new List<Advertisement>();
        public List<Publisher> Publishers { get; set; } = new List<Publisher>();
        public List<RealEstate> RealEstates { get; set; } = new List<RealEstate>();
        public List<Location> Locations { get; set; } = new List<Location>();
        public List<NewspaperAdvertisement> NewspaperAdvertisements { get; set; } = new List<NewspaperAdvertisement>();
    }
}
