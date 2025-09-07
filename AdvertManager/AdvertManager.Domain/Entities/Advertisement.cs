using AdvertManager.Domain.Observer;
using AdvertManager.Domain.State;
using System;
using System.Collections.Generic;

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
        private List<IObserver> observers;

        public Advertisement()
        {
            createdAt = DateTime.Now;
            observers = new List<IObserver>();
            SetState(new ActiveState());
        }

        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime ExpirationDate { get => expirationDate; set => expirationDate = value; }
        public decimal Price { get => price; set => price = value; }
        public Publisher Publisher { get => publisher; set => publisher = value; }
        public RealEstate RealEstate { get => realEstate; set => realEstate = value; }
        public AdvertisementState State => state;

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
            this.state.Handle();
        }

        public decimal CalculatePricePerSquareMeter()
        {
            return realEstate != null && realEstate.AreaInSquareMeters > 0
                ? price / (decimal)realEstate.AreaInSquareMeters
                : 0;
        }

        public void NotifyObservers()
        {
            foreach (var observer in observers)
            {
                observer.Update(this);
            }
        }

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void UnregisterObserver(IObserver observer)
        {
            observers.Remove(observer);
        }
    }
}
