namespace AdvertManager.Client.Helpers
{
    public interface IDialogService
    {
        bool? ShowDialog(object dialogViewModel, string title = "");
    }
}
