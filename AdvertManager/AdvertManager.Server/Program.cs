using AdvertManager.Server.DataStorage;
using AdvertManager.Server.Service;
using AdvertManager.Server.Service.Interfaces;
using System;
using System.IO;
using System.ServiceModel;

namespace AdvertManager.Server
{
    internal class Program
    {
        private static ServiceHost serviceHost;

        static void Main(string[] args)
        {
            IStorageType storageType;

            while (true)
            {
                Console.WriteLine("Select storage format for the server data:");
                Console.WriteLine("1. JSON");
                Console.WriteLine("2. XML");
                Console.WriteLine("3. CSV");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    storageType = IStorageType.JSON;
                    break;
                }
                else if (choice == "2")
                {
                    storageType = IStorageType.XML;
                    break;
                }
                else if (choice == "3")
                {
                    storageType = IStorageType.CSV;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.\n");
                }
            }

            string projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            string dataFolder = Path.Combine(projectRoot, "Data");
            Directory.CreateDirectory(dataFolder);

            string storageFolder = "";
            string filePath = "";
            switch (storageType)
            {
                case IStorageType.JSON:
                    storageFolder = Path.Combine(dataFolder, "json");
                    Directory.CreateDirectory(storageFolder);
                    filePath = Path.Combine(storageFolder, "entities.json");
                    break;
                case IStorageType.XML:
                    storageFolder = Path.Combine(dataFolder, "xml");
                    Directory.CreateDirectory(storageFolder);
                    filePath = Path.Combine(storageFolder, "entities.xml");
                    break;
                case IStorageType.CSV:
                    storageFolder = Path.Combine(dataFolder, "csv");
                    Directory.CreateDirectory(storageFolder);
                    filePath = Path.Combine(storageFolder, "entities.csv");
                    break;
            }

            DataService dataService = new DataService();
            dataService.SetStorage(storageType, filePath);

            serviceHost = new ServiceHost(dataService);
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:8000/Service";
            serviceHost.AddServiceEndpoint(typeof(IDataService), binding, address);

            Console.WriteLine($"ServiceHost opened with {storageType} storage at '{filePath}'.");
            serviceHost.Open();

            Console.WriteLine("Press ENTER to stop the server...");
            Console.ReadLine();
        }
    }
}
