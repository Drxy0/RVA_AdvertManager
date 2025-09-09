using AdvertManager.Domain.Enums;
using System.ComponentModel;

namespace AdvertManager.Domain.Entities
{
    public class RealEstate : INotifyPropertyChanged
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

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(Id)); }
        }

        public double AreaInSquareMeters
        {
            get => areaInSquareMeters;
            set { areaInSquareMeters = value; OnPropertyChanged(nameof(AreaInSquareMeters)); }
        }

        public RealEstateType Type
        {
            get => type;
            set { type = value; OnPropertyChanged(nameof(Type)); }
        }

        public int YearBuilt
        {
            get => yearBuilt;
            set { yearBuilt = value; OnPropertyChanged(nameof(YearBuilt)); }
        }

        public bool IsAvailable
        {
            get => isAvailable;
            set { isAvailable = value; OnPropertyChanged(nameof(IsAvailable)); }
        }

        public Location Location
        {
            get => location;
            set { location = value; OnPropertyChanged(nameof(Location)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
