using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.ViewModels.Operator.Popups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperatorReviewPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly OperatorReviewViewModel _viewModel;

        public OperatorReviewPopup(
            CheckPointDto checkPoint,
            List<OilMark> oilMarks,
            TaskCompletionSource<OperatorReviewRequest> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new OperatorReviewViewModel(checkPoint, oilMarks, completionSource);

            BindingContext = _viewModel;
        }
    }
}