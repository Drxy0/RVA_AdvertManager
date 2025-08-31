using System;
using System.IO;
using System.Xml.Serialization;

namespace AdvertManager.Server.DataStorage
{
    public class XmlDataStorage : IDataStorage
    {
        public void Save<T>(string filePath, T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(stream, data);
            }
        }

        public T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                return (T)serializer.Deserialize(stream);
            }
        }
    }
}
