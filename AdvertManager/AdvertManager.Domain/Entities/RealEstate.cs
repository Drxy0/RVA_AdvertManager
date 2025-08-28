using AdvertManager.Domain.Enums;

namespace AdvertManager.Domain.Entities
{
	public class RealEstate
	{
		private double areaInSquareMeters;
		private RealEstateType type;
		private int yearBuilt;
        private bool isAvailable;

        public RealEstate(double areaInSquareMeters, RealEstateType type, int yearBuilt, bool isAvailable)
        {
            this.areaInSquareMeters = areaInSquareMeters;
            this.type = type;
            this.yearBuilt = yearBuilt;
            this.isAvailable = isAvailable;
        }

        public double AreaInSquareMeters { get => areaInSquareMeters; set => areaInSquareMeters = value; }
        public RealEstateType Type { get => type; set => type = value; }
        public int YearBuilt { get => yearBuilt; set => yearBuilt = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
    }
}
