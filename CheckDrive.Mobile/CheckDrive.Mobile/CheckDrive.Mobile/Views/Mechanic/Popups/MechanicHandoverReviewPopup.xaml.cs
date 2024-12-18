using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Mechanic.Handover;
using CheckDrive.Mobile.ViewModels.Mechanic.Popups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicHandoverReviewPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly MechanicHandoverReviewViewModel _viewModel;

        public MechanicHandoverReviewPopup(
            CheckPointDto checkPoint,
            List<CarDto> cars,
            TaskCompletionSource<MechanicHandoverRequest> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new MechanicHandoverReviewViewModel(checkPoint, cars, completionSource);

            BindingContext = _viewModel;
        }
    }
}