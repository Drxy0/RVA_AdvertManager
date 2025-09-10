using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Repositories;
using AdvertManager.Server.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace AdvertManager.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class DataService : IDataService
    {
        private readonly AdvertisementRepository _advertRepository = new AdvertisementRepository();
        private readonly PublisherRepository _publisherRepository = new PublisherRepository();
        private readonly RealEstateRepository _realEstateRepository = new RealEstateRepository();
        private readonly LocationRepository _locationRepository = new LocationRepository();
        private readonly NewspaperAdvertisementRepository _newspaperAdvertRepository = new NewspaperAdvertisementRepository();

        private IDataStorage _storage;
        private string _filePath;

        public DataService(){}

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
            LoadData();
        }

        private void LoadData()
        {
            PersistedEntities loaded = null;

            if (_storage is CsvDataStorage csvStorage)
            {
                string folderPath = Path.GetDirectoryName(_filePath);
                loaded = csvStorage.LoadEntities(folderPath);
            }
            else
            {
                if (!File.Exists(_filePath)) return;
                loaded = _storage.Load<PersistedEntities>(_filePath);
            }

            if (loaded != null)
            {
                _advertRepository.AddRange(loaded.Advertisements ?? new List<Advertisement>());
                _publisherRepository.AddRange(loaded.Publishers ?? new List<Publisher>());
                _realEstateRepository.AddRange(loaded.RealEstates ?? new List<RealEstate>());
                _locationRepository.AddRange(loaded.Locations ?? new List<Location>());
                _newspaperAdvertRepository.AddRange(loaded.NewspaperAdvertisements ?? new List<NewspaperAdvertisement>());
            }

            EnsureSeedData();
        }


        private void SaveData()
        {
            var entities = new PersistedEntities
            {
                Advertisements = _advertRepository.GetAll().ToList(),
                Publishers = _publisherRepository.GetAll().ToList(),
                RealEstates = _realEstateRepository.GetAll().ToList(),
                Locations = _locationRepository.GetAll().ToList(),
                NewspaperAdvertisements = _newspaperAdvertRepository.GetAll().ToList()
            };

            if (_storage is CsvDataStorage csvStorage)
            {
                string folderPath = Path.GetDirectoryName(_filePath);
                csvStorage.SaveEntities(folderPath, entities);
            }
            else
            {
                _storage.Save(_filePath, entities);
            }
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
                    Id = 1,
                    Title = "Flat in the city centre",
                    Description = "Two-bedroom flat, 60m², renovated, excellent location.",
                    Price = 85000,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        Id = 1,
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
                        ) { Id = 1 }
                    ) { Id = 1 }
                },
                new Advertisement
                {
                    Id = 2,
                    Title = "House with a garden",
                    Description = "Family house, 120m², large garden, quiet neighbourhood.",
                    Price = 150000,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        Id = 2,
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
                        ) { Id = 2 }
                    ) { Id = 2 }
                },
                new Advertisement
                {
                    Id = 3,
                    Title = "Office space to let",
                    Description = "Modern office space, 45m², fully furnished, near public transport.",
                    Price = 500,
                    CreatedAt = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    Publisher = new Publisher
                    {
                        Id = 3,
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
                        ) { Id = 3 }
                    ) { Id = 3 }
                }
            };

            _advertRepository.AddRange(dummyAdverts);
        }
    }
}
