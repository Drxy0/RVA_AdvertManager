using AdvertManager.Domain.State;
using System;

namespace AdvertManager.Domain.Entities
{
	public class Advertisement
	{
		private string title;
		private string description;
		private DateTime createdAt;
		private DateTime expirationDate;
        private decimal price;
		private Publisher publisher;
        private RealEstate realEstate;
		private AdvertisementState state;

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime ExpirationDate { get => expirationDate; set => expirationDate = value; }
        public decimal Price { get => price; set => price = value; }
        public Publisher Publisher { get => publisher; set => publisher = value; }
        public RealEstate RealEstate { get => realEstate; set => realEstate = value; }
        public AdvertisementState State { get => state; }

        //private List<IObserver> observers;

        public void ExtendExpired(int days)
		{
			throw new NotImplementedException();
		}

		public void SetState(AdvertisementState state)
		{
			this.state = state;
		}

		public decimal CalculatePricePerSquareMeter()
		{
			throw new NotImplementedException();
		}

		public void NotifyObservers()
		{
			throw new NotImplementedException();
		}
	}
}
