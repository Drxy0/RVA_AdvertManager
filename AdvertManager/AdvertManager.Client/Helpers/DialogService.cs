using AdvertManager.Client.ViewModels;
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

            if (dialogViewModel is AdvertisementFormViewModel viewModel)
            {
                // Store the original close action
                var originalCloseAction = viewModel._onClose;

                // Replace with an action that closes the window
                viewModel._onClose = (result) =>
                {
                    window.DialogResult = result;
                    window.Close();
                    originalCloseAction?.Invoke(result);
                };
            }

            return window.ShowDialog();
        }
    }
}
