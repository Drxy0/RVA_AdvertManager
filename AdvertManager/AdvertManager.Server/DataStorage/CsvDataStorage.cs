using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdvertManager.Server.DataStorage
{
    public class CsvDataStorage : IDataStorage
    {
        public void Save<T>(string filePath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(new[] { data });
            }
        }

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<T>().FirstOrDefault();
            }
        }
    }
}
