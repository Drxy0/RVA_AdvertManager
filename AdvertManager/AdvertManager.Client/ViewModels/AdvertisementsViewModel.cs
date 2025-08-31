using AdvertManager.Client.Helpers;
using AdvertManager.Domain.Entities;
using System.Collections.ObjectModel;

namespace AdvertManager.Client.ViewModels
{
    public class AdvertisementsViewModel : BindableBase
    {
        // TODO: Note - I put ObservableCollection for old times sake, mby a List<> is enough
        private ObservableCollection<Advertisement> _advertisements;

        public AdvertisementsViewModel()
        {
            this._advertisements = new ObservableCollection<Advertisement>();
        }
    }
}
