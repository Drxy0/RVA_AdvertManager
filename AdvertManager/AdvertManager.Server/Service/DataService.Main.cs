using AdvertManager.Domain.Entities;
using AdvertManager.Domain.Enums;
using AdvertManager.Domain.State;
using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Repositories;
using AdvertManager.Server.Service.Interfaces;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace AdvertManager.Server.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal partial class DataService : IDataService
    {
        private readonly AdvertisementRepository _advertRepository = new AdvertisementRepository();
        private readonly PublisherRepository _publisherRepository = new PublisherRepository();
        private readonly RealEstateRepository _realEstateRepository = new RealEstateRepository();
        private readonly LocationRepository _locationRepository = new LocationRepository();
        private readonly NewspaperAdvertisementRepository _newspaperAdvertRepository = new NewspaperAdvertisementRepository();
        private static readonly ILog _logger = LogManager.GetLogger(typeof(DataService));

        private IDataStorage _storage;
        private string _filePath;

        public DataService(){}

        public DataService(IDataStorage storage, string filePath)
        {
            _storage = storage;
            _filePath = filePath;

            _logger.Info($"DataService initialized with storage: {storage?.GetType().Name}, filePath: {_filePath}");

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

            _logger.Info($"Storage set to: {_storage?.GetType().Name}, filePath: {_filePath}");

            LoadData();
        }


        private void LoadData()
        {
            try
            {
                _logger.Debug("Loading data started.");
                PersistedEntities loaded = null;

                if ((_storage is CsvDataStorage && Directory.Exists(_filePath)) ||
                    (!(_storage is CsvDataStorage) && File.Exists(_filePath)))
                {
                    loaded = _storage.Load<PersistedEntities>(_filePath);
                }

                if (loaded != null)
                {
                    _advertRepository.AddRange(loaded.Advertisements ?? new List<Advertisement>());
                    _publisherRepository.AddRange(loaded.Publishers ?? new List<Publisher>());
                    _realEstateRepository.AddRange(loaded.RealEstates ?? new List<RealEstate>());
                    _locationRepository.AddRange(loaded.Locations ?? new List<Location>());
                    _newspaperAdvertRepository.AddRange(loaded.NewspaperAdvertisements ?? new List<NewspaperAdvertisement>());

                    ApplyAdvertisementStates(_advertRepository.GetAll());

                    _logger.Info("Data successfully loaded into repositories.");
                }

                if (loaded == null || !_advertRepository.GetAll().Any())
                {
                    _logger.Warn("No data found, seeding default entities...");
                    EnsureSeedData();
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error while loading data.", ex);
                throw;
            }
        }

        private void SaveData()
        {

            try
            {
                _logger.Debug("Saving data started.");

                var entities = new PersistedEntities
                {
                    Advertisements = _advertRepository.GetAll().ToList(),
                    Publishers = _publisherRepository.GetAll().ToList(),
                    RealEstates = _realEstateRepository.GetAll().ToList(),
                    Locations = _locationRepository.GetAll().ToList(),
                    NewspaperAdvertisements = _newspaperAdvertRepository.GetAll().ToList()
                };

                _storage.Save(_filePath, entities);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while saving data.", ex);
                throw;
            }
        }


        /// <summary>
        /// Add 3 instances of primary class (Advertisement) if there are none in file.
        /// Ensures all related entities (Publisher, RealEstate, Location) are also added.
        /// </summary>
        /// 
        private void EnsureSeedData()
        {
            try
            {
                if (_advertRepository.GetAll().Any())
                    return;

                _logger.Info("Seeding default advertisements, publishers, real estates, and locations...");

                var dummyAdverts = new List<Advertisement>
                {
                    new Advertisement
                    {
                        Id = 1,
                        Title = "Flat in the city centre",
                        Description = "Two-bedroom flat, 60m², renovated, excellent location.",
                        Price = 85000,
                        CreatedAt = DateTime.Now,
                        ExpirationDate = DateTime.Now.AddMonths(1).AddHours(5),
                        StateName = "Rented",
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
                        ExpirationDate = DateTime.Now.AddDays(15),
                        StateName = "Active",
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
                        CreatedAt = new DateTime(2020, 1, 10),
                        ExpirationDate = new DateTime(2020, 3, 10),
                        StateName = "Expired",
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
                    },
                    new Advertisement
                    {
                        Id = 4,
                        Title = "Seaside Apartment in Budva",
                        Description = "Cozy 40m² apartment just 200m from the Adriatic Sea. Ideal for summer holidays.",
                        Price = 75000,
                        CreatedAt = new DateTime(2025, 5, 15),
                        ExpirationDate = DateTime.UtcNow.AddMonths(2),
                        StateName = "Active",
                        Publisher = new Publisher
                        {
                            Id = 4,
                            FirstName = "Marko",
                            LastName = "Petrović",
                            ContactNumber = "+382 67 123 456"
                        },
                        RealEstate = new RealEstate(
                            areaInSquareMeters: 40,
                            type: RealEstateType.FLAT,
                            yearBuilt: 2010,
                            isAvailable: false,
                            location: new Location(
                                city: "Budva",
                                country: "Montenegro",
                                postalCode: "85310",
                                street: "Jadranski Put",
                                streetNumber: "12A"
                            ) { Id = 4 }
                        ) { Id = 4 }
                    }
                };

                ApplyAdvertisementStates(dummyAdverts);

                var dummyNewspaperAdverts = new List<NewspaperAdvertisement>
                {
                    new NewspaperAdvertisement
                    {
                        Id = 20,
                        Title = "Charming Studio Flat Available",
                        Description = "Cozy 25m² studio in central Liverpool. Perfect for students or professionals.",
                        PublisherFullName = "John Doe",
                        PhoneNumber = "+44 7700 222333"
                    },
                    new NewspaperAdvertisement
                    {
                        Id = 21,
                        Title = "Spacious Countryside House for Rent",
                        Description = "4-bedroom house with garden and garage, located in the quiet suburbs of York.",
                        PublisherFullName = "Emily Clark",
                        PhoneNumber = "+44 7700 444555"
                    },
                };

                _advertRepository.AddRange(dummyAdverts);

                _publisherRepository.AddRange(dummyAdverts
                    .Where(a => a.Publisher != null)
                    .Select(a => a.Publisher));

                _realEstateRepository.AddRange(dummyAdverts
                    .Where(a => a.RealEstate != null)
                    .Select(a => a.RealEstate));

                _locationRepository.AddRange(dummyAdverts
                    .Where(a => a.RealEstate?.Location != null)
                    .Select(a => a.RealEstate.Location));

                if (!_newspaperAdvertRepository.GetAll().Any())
                {
                    _newspaperAdvertRepository.AddRange(dummyNewspaperAdverts);
                }

                SaveData();
                _logger.Info("Default seed data created and saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error while seeding default data.", ex);
                throw;
            }
        }

        private void ApplyAdvertisementStates(IEnumerable<Advertisement> adverts)
        {
            foreach (var ad in adverts)
            {
                if (ad.ExpirationDate <= DateTime.Now)
                {
                    ad.SetState(new ExpiredState());
                    continue;
                }

                switch (ad.StateName)
                {
                    case "Active": ad.SetState(new ActiveState()); break;
                    case "Rented": ad.SetState(new RentedState()); break;
                    case "Expired": ad.SetState(new ExpiredState()); break;
                    default: ad.SetState(new ActiveState()); break;
                }
            }
        }
    }
}
