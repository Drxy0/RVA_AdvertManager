using AdvertManager.Domain.Entities;
using AdvertManager.Server.Service.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdvertManager.Server.Service
{
    internal partial class DataService : IDataService
    {
        public void AddPublisher(Publisher publisher)
        {
            try
            {
                _publisherRepository.Add(publisher);
                SaveData();
                _logger.Info($"Publisher added: {publisher.FirstName} {publisher.LastName} (ID: {publisher.Id})");
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
                var publishers = _publisherRepository.GetAll();
                _logger.Info($"Retrieved {publishers?.Count() ?? 0} publishers.");
                return publishers;
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving publishers", ex);
                throw;
            }
        }

        public void AddRealEstate(RealEstate realEstate)
        {
            try
            {
                _realEstateRepository.Add(realEstate);
                SaveData();
                _logger.Info($"Real estate added (ID: {realEstate.Id}, Type: {realEstate.Type}, Area: {realEstate.AreaInSquareMeters}m²)");
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
                var realEstates = _realEstateRepository.GetAll();
                _logger.Info($"Retrieved {realEstates?.Count() ?? 0} real estates.");
                return realEstates;
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving real estates", ex);
                throw;
            }
        }

        public void AddLocation(Location location)
        {
            try
            {
                _locationRepository.Add(location);
                SaveData();
                _logger.Info($"Location added: {location.City}, {location.Country} (ID: {location.Id})");
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
                var locations = _locationRepository.GetAll();
                _logger.Info($"Retrieved {locations?.Count() ?? 0} locations.");
                return locations;
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving locations", ex);
                throw;
            }
        }

        public void AddNewspaperAdvertisement(NewspaperAdvertisement newspaperAdvertisement)
        {
            try
            {
                _newspaperAdvertRepository.Add(newspaperAdvertisement);
                SaveData();
                _logger.Info($"Newspaper advertisement added: {newspaperAdvertisement.Title} (ID: {newspaperAdvertisement.Id})");
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
                var ads = _newspaperAdvertRepository.GetAll();
                _logger.Info($"Retrieved {ads?.Count() ?? 0} newspaper advertisements.");
                return ads;
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving newspaper advertisements", ex);
                throw;
            }
        }
    }
}
