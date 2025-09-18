namespace AdvertManager.Server.DataStorage
{
	internal interface IDataStorage
	{
        void Save<T>(string filePath, T data);
        T Load<T>(string filePath);
    }
}
