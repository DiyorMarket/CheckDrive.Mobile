using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.ViewModels.Dispatcher.Popups;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Dispatcher.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DispatcherReviewPopup : PopupPage
    {
        private readonly DispatcherReviewViewModel _viewModel;

        public DispatcherReviewPopup(
            CheckPointDto checkPoint,
            TaskCompletionSource<DispatcherReviewRequest> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new DispatcherReviewViewModel(checkPoint, completionSource);

            BindingContext = _viewModel;
        }
    }
}