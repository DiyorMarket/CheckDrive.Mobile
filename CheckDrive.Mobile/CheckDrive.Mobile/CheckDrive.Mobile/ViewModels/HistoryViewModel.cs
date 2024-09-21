using CheckDrive.Mobile.Helpers;
using System.Collections.ObjectModel;

namespace CheckDrive.Mobile.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public ObservableCollection<History> Reviews { get; private set; }

        public HistoryViewModel()
        {
            Reviews = new ObservableCollection<History>();
        }
    }
}
