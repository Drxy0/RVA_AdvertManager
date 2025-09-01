using AdvertManager.Server.Service;
using AdvertManager.Server.Service.Interfaces;
using System;
using System.ServiceModel;

namespace AdvertManager.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost serviceHost = new ServiceHost(typeof(DataService));
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:8000/Service";

            serviceHost.AddServiceEndpoint(typeof(IDataService), binding, address);

            Console.WriteLine("ServiceHost opened.");
            serviceHost.Open();
            Console.ReadLine();
            serviceHost.Close();
        }
    }
}
