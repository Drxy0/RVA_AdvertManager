using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Repositories;
using AdvertManager.Server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdvertManager.Server.Service
{
    public partial class DataService : IDataService
    {
        private readonly AdvertisementRepository _advertRepository = new AdvertisementRepository();
        private readonly PublisherRepository _publisherRepository = new PublisherRepository();
        private readonly RealEstateRepository _realEstateRepository = new RealEstateRepository();
        private readonly LocationRepository _locationRepository = new LocationRepository();
        private readonly NewspaperAdvertisementRepository _newspaperAdvertRepository = new NewspaperAdvertisementRepository();

        private IDataStorage _storage;
        private string _filePath;

        public DataService()
        {
            _storage = new JsonDataStorage();
            _filePath = "entities.json";

            LoadData();
        }

        public DataService(IDataStorage storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;

            LoadData();
        }

        public void SetStorage(IStorageType type, string filePath)
        {
            _filePath = filePath;

            switch(type)
            {
                case IStorageType.JSON:
                    _storage = new JsonDataStorage();
                    break;
                case IStorageType.XML:
                    _storage = new XmlDataStorage();
                    break;
                case IStorageType.CSV:
                    _storage = new CsvDataStorage();
                    break;
            }
        }

        private void LoadData()
        {
            if (!File.Exists(_filePath)) return;

            var loaded = _storage.Load<PersistedEntities>(_filePath);
            if (loaded != null)
            {
                _advertRepository.AddRange(loaded.Advertisements ?? new List<Advertisement>());
                _publisherRepository.AddRange(loaded.Publishers ?? new List<Publisher>());
            }

            EnsureSeedData();
        }

        private void SaveData()
        {
            var entities = new PersistedEntities
            {
                Advertisements = _advertRepository.GetAll().ToList(),
                Publishers = _publisherRepository.GetAll().ToList()
            };

            _storage.Save(_filePath, entities);
        }

        /// <summary>
        /// Add 3 instances of primary class (Advertisement) if there are none in file.
        /// </summary>
        private void EnsureSeedData()
        {
            if (_advertRepository.GetAll().Any())
                return;

            var dummyAdverts = new List<Advertisement>
            {
                new Advertisement
                {
                    Title = "Flat in the city centre",
                    Description = "Two-bedroom flat, 60m², renovated, excellent location.",
                    Price = 85000,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        ContactNumber = "+44 7700 900123"
                    },
                    RealEstate = new RealEstate(
                        areaInSquareMeters: 60,
                        type: RealEstateType.FLAT,
                        yearBuilt: 2005,
                        isAvailable: true,
                        location: new Location(
                            city: "London",
                            country: "United Kingdom",
                            postalCode: "SW1A 1AA",
                            street: "Oxford Street",
                            streetNumber: "221B"
                        )
                    )
                },
                new Advertisement
                {
                    Title = "House with a garden",
                    Description = "Family house, 120m², large garden, quiet neighbourhood.",
                    Price = 150000,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        FirstName = "Emily",
                        LastName = "Clark",
                        ContactNumber = "+44 7700 900456"
                    },
                    RealEstate = new RealEstate(
                        areaInSquareMeters: 120,
                        type: RealEstateType.HOUSE,
                        yearBuilt: 1998,
                        isAvailable: true,
                        location: new Location(
                            city: "Manchester",
                            country: "United Kingdom",
                            postalCode: "M1 1AE",
                            street: "King Street",
                            streetNumber: "42"
                        )
                    )
                },
                new Advertisement
                {
                    Title = "Office space to let",
                    Description = "Modern office space, 45m², fully furnished, near public transport.",
                    Price = 500,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        FirstName = "Oliver",
                        LastName = "Brown",
                        ContactNumber = "+44 7700 900789"
                    },
                    RealEstate = new RealEstate(
                        areaInSquareMeters: 45,
                        type: RealEstateType.OFFICE,
                        yearBuilt: 2015,
                        isAvailable: false,
                        location: new Location(
                            city: "Birmingham",
                            country: "United Kingdom",
                            postalCode: "B1 1AA",
                            street: "High Street",
                            streetNumber: "7C"
                        )
                    )
                }
            };

            _advertRepository.AddRange(dummyAdverts);
        }
    }
}
