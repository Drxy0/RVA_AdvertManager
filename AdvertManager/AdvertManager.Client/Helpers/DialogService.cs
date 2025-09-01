using AdvertManager.Client.Views;
using System.Windows;

namespace AdvertManager.Client.Helpers
{
    public class DialogService : IDialogService
    {
        public bool? ShowDialog(object dialogViewModel, string title = "")
        {
            // Create the form view
            var formView = new AdvertisementFormView();

            // Set the DataContext to the provided ViewModel
            formView.DataContext = dialogViewModel;

            var window = new Window
            {
                Title = title,
                Content = formView,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                SizeToContent = SizeToContent.WidthAndHeight,
                ResizeMode = ResizeMode.NoResize,
                MinWidth = 400,
                MinHeight = 300
            };

            return window.ShowDialog();
        }
    }
}
