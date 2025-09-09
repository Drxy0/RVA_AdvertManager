using AdvertManager.Domain.Enums;

namespace AdvertManager.Domain.Entities
{
    public class RealEstate
    {
        private int id;
        private double areaInSquareMeters;
        private RealEstateType type;
        private int yearBuilt;
        private bool isAvailable;
        private Location location;

        public RealEstate() { }

        public RealEstate(double areaInSquareMeters, RealEstateType type, int yearBuilt, bool isAvailable, Location location)
        {
            this.areaInSquareMeters = areaInSquareMeters;
            this.type = type;
            this.yearBuilt = yearBuilt;
            this.isAvailable = isAvailable;
            this.location = location;
        }

        public int Id { get => id; set => id = value; }
        public double AreaInSquareMeters { get => areaInSquareMeters; set => areaInSquareMeters = value; }
        public RealEstateType Type { get => type; set => type = value; }
        public int YearBuilt { get => yearBuilt; set => yearBuilt = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
        public Location Location { get => location; set => location = value; }
    }
}