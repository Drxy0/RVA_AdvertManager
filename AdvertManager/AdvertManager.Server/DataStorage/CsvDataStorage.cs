using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AdvertManager.Domain.Entities;

namespace AdvertManager.Server.DataStorage
{
    internal class CsvDataStorage : IDataStorage
    {
        public void Save<T>(string folderPath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (!(data is PersistedEntities entities))
                throw new InvalidOperationException("CsvDataStorage only supports PersistedEntities.");

            Directory.CreateDirectory(folderPath);

            SaveList(Path.Combine(folderPath, "Advertisements.csv"), entities.Advertisements);
            SaveList(Path.Combine(folderPath, "Publishers.csv"), entities.Publishers);
            SaveList(Path.Combine(folderPath, "RealEstates.csv"), entities.RealEstates);
            SaveList(Path.Combine(folderPath, "Locations.csv"), entities.Locations);
            SaveList(Path.Combine(folderPath, "NewspaperAdvertisements.csv"), entities.NewspaperAdvertisements);
        }

        public T Load<T>(string folderPath)
        {
            if (typeof(T) != typeof(PersistedEntities))
                throw new InvalidOperationException("CsvDataStorage only supports PersistedEntities.");

            var entities = new PersistedEntities
            {
                Advertisements = LoadList<Advertisement>(Path.Combine(folderPath, "Advertisements.csv")),
                Publishers = LoadList<Publisher>(Path.Combine(folderPath, "Publishers.csv")),
                RealEstates = LoadList<RealEstate>(Path.Combine(folderPath, "RealEstates.csv")),
                Locations = LoadList<Location>(Path.Combine(folderPath, "Locations.csv")),
                NewspaperAdvertisements = LoadList<NewspaperAdvertisement>(Path.Combine(folderPath, "NewspaperAdvertisements.csv"))
            };

            return (T)(object)entities;
        }

        private void SaveList<T>(string filePath, List<T> list)
        {
            if (list == null || !list.Any())
                return;

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(list);
            }
        }

        private List<T> LoadList<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
                return new List<T>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().ToList();
            }
        }
    }
}
