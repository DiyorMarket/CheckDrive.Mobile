using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.ViewModels.Driver.Popups;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Driver.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperatorReviewConfirmationPopup : PopupPage
    {
        public OperatorReviewConfirmationPopup(
            TaskCompletionSource<ReviewConfirmationRequest> completionSource,
            CheckPointDto checkPoint)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            BindingContext = new OperatorReviewConfirmationViewModel(completionSource, checkPoint);
        }
    }
}