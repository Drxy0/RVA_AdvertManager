using AdvertManager.Domain.State;
using System;
using System.Runtime.Serialization;

namespace AdvertManager.Domain.Entities
{
    [DataContract]
    [KnownType(typeof(NewspaperAdvertisementAdapter))]
    public class Advertisement
	{
        private int id;
        private string title;
        private string description;
        private DateTime createdAt;
        private DateTime expirationDate;
        private decimal price;
        private Publisher publisher;
        private RealEstate realEstate;
        private AdvertisementState state;

        public Advertisement()
        {
            createdAt = DateTime.Now;
        }

        [DataMember]
        public int Id { get => id; set => id = value; }

        [DataMember]
        public string Title { get => title; set => title = value; }

        [DataMember]
        public string Description { get => description; set => description = value; }

        [DataMember]
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

        [DataMember]
        public DateTime ExpirationDate { get => expirationDate; set => expirationDate = value; }

        [DataMember]
        public decimal Price { get => price; set => price = value; }

        [DataMember]
        public Publisher Publisher { get => publisher; set => publisher = value; }

        [DataMember]
        public RealEstate RealEstate { get => realEstate; set => realEstate = value; }
        public AdvertisementState State => state;
        
        [DataMember]
        public string StateName { get; set; }

        public void ExtendExpired(int days)
        {
            if (state is ExpiredState)
            {
                expirationDate = expirationDate.AddDays(days);
                SetState(new ActiveState());
            }
        }

        public void SetState(AdvertisementState state)
        {
            this.state = state;
            this.state.SetAdvertisement(this);
            StateName = state.Name;
            state.Handle();
        }

        public decimal CalculatePricePerSquareMeter()
        {
            return realEstate != null && realEstate.AreaInSquareMeters > 0
                ? price / (decimal)realEstate.AreaInSquareMeters
                : 0;
        }
    }
}
