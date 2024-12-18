using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Mechanic.Acceptance;
using CheckDrive.Mobile.ViewModels.Mechanic.Popups;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicAcceptanceReviewPopup : PopupPage
    {
        private readonly MechanicAcceptanceReviewViewModel _viewModel;

        public MechanicAcceptanceReviewPopup(
            CheckPointDto checkPoint,
            TaskCompletionSource<MechanicAcceptanceRequest> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new MechanicAcceptanceReviewViewModel(checkPoint, completionSource);

            BindingContext = _viewModel;
        }
    }
}