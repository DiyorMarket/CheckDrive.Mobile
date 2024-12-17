using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.ViewModels.Doctor.Popups;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Doctor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoctorReviewPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly DoctorReviewViewModel _viewModel;

        public DoctorReviewPopup(DriverDto driver, TaskCompletionSource<DoctorReviewRequest> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new DoctorReviewViewModel(driver, completionSource);
            BindingContext = _viewModel;
        }
    }
}