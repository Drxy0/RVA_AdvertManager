using AdvertManager.Client.Views;
using AdvertManager.Domain.Entities;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Service.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AdvertManager.Client
{
    public class ClientProxy : ChannelFactory<IDataService>, IDataService
    {
        private readonly IDataService factory;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ClientProxy));

        public ClientProxy(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            factory = this.CreateChannel();

            log4net.Config.XmlConfigurator.Configure();
            _logger.Info("ClientProxy initialized and logging configured.");
        }

        public void SetStorage(IStorageType type, string filePath)
        {
            try
            {
                _logger.Info($"Setting storage: {type}, Path: {filePath}");
                factory.SetStorage(type, filePath);
            }
            catch (Exception ex)
            {
                _logger.Error("Error setting storage", ex);
                throw;
            }
        }

        // Advertisement
        public void AddAdvertisement(Advertisement ad)
        {
            try
            {
                _logger.Info($"Adding Advertisement: {ad.Title} (ID: {ad.Id})");
                factory.AddAdvertisement(ad);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding advertisement", ex);
                throw;
            }
        }

        public void UpdateAdvertisement(Advertisement ad)
        {
            try
            {
                _logger.Info($"Updating Advertisement: {ad.Title} (ID: {ad.Id})");
                factory.UpdateAdvertisement(ad);
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating advertisement", ex);
                throw;
            }
        }

        public void DeleteAdvertisement(Advertisement ad)
        {
            try
            {
                _logger.Warn($"Deleting Advertisement: {ad?.Title} (ID: {ad?.Id})");
                factory.DeleteAdvertisement(ad);
            }
            catch (Exception ex)
            {
                _logger.Error("Error deleting advertisement", ex);
                throw;
            }
        }

        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            try
            {
                _logger.Debug("Fetching all advertisements");
                return factory.GetAllAdvertisements();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching advertisements", ex);
                throw;
            }
        }

        // Publisher
        public void AddPublisher(Publisher publisher)
        {
            try
            {
                _logger.Info($"Adding Publisher: {publisher.FirstName} {publisher.LastName}");
                factory.AddPublisher(publisher);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding publisher", ex);
                throw;
            }
        }

        public IEnumerable<Publisher> GetAllPublishers()
        {
            try
            {
                _logger.Debug("Fetching all publishers");
                return factory.GetAllPublishers();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching publishers", ex);
                throw;
            }
        }

        // RealEstate
        public void AddRealEstate(RealEstate realEstate)
        {
            try
            {
                _logger.Info($"Adding RealEstate (ID: {realEstate.Id})");
                factory.AddRealEstate(realEstate);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding real estate", ex);
                throw;
            }
        }

        public IEnumerable<RealEstate> GetAllRealEstates()
        {
            try
            {
                _logger.Debug("Fetching all real estates");
                return factory.GetAllRealEstates();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching real estates", ex);
                throw;
            }
        }

        // Location
        public void AddLocation(Location location)
        {
            try
            {
                _logger.Info($"Adding Location: {location.City}, {location.Country}");
                factory.AddLocation(location);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding location", ex);
                throw;
            }
        }

        public IEnumerable<Location> GetAllLocations()
        {
            try
            {
                _logger.Debug("Fetching all locations");
                return factory.GetAllLocations();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching locations", ex);
                throw;
            }
        }

        // NewspaperAdvertisement
        public void AddNewspaperAdvertisement(NewspaperAdvertisement newspaperAdvertisement)
        {
            try
            {
                _logger.Info($"Adding Newspaper Advertisement: {newspaperAdvertisement.Title}");
                factory.AddNewspaperAdvertisement(newspaperAdvertisement);
            }
            catch (Exception ex)
            {
                _logger.Error("Error adding newspaper advertisement", ex);
                throw;
            }
        }

        public IEnumerable<NewspaperAdvertisement> GetAllNewspaperAdvertisements()
        {
            try
            {
                _logger.Debug("Fetching all newspaper advertisements");
                return factory.GetAllNewspaperAdvertisements();
            }
            catch (Exception ex)
            {
                _logger.Error("Error fetching newspaper advertisements", ex);
                throw;
            }
        }
    }
}
