using AdvertManager.Domain.Entities;
using AdvertManager.Server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AdvertManager.Server.Service
{
    public partial class DataService : IDataService
    {
        public void AddAdvertisement(Advertisement ad)
        {
            try
            {
                _advertRepository.Add(ad);
                SaveData();
                _logger.Info($"Advertisement added: {ad.Title} (ID: {ad.Id})");
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
                _advertRepository.Update(ad);
                SaveData();
                _logger.Info($"Advertisement updated: {ad.Title} (ID: {ad.Id})");
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
                if (ad != null)
                {
                    _advertRepository.Delete(ad);
                    SaveData();
                    _logger.Info($"Advertisement deleted: {ad.Title} (ID: {ad.Id})");
                }
                else
                {
                    _logger.Warn("DeleteAdvertisement called with null parameter.");
                }
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
                var ads = _advertRepository.GetAll();
                _logger.Info($"Retrieved {ads?.Count() ?? 0} advertisements.");
                return ads;
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving advertisements", ex);
                throw;
            }
        }
    }
}
