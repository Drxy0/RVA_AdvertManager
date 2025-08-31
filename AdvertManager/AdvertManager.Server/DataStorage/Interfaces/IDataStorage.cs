namespace AdvertManager.Server.DataStorage
{
	public interface IDataStorage
	{
        void Save<T>(string filePath, T data);
        T Load<T>(string filePath);
    }
}
