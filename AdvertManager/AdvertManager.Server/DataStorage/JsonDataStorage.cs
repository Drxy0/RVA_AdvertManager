using Newtonsoft.Json;
using System;
using System.IO;

namespace AdvertManager.Server.DataStorage
{
    public class JsonDataStorage : IDataStorage
    {
        public void Save<T>(string filePath, T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
