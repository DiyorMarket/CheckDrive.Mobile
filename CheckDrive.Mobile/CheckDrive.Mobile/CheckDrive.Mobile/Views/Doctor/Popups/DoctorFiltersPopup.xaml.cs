using CheckDrive.Mobile.Models.Doctor;
using CheckDrive.Mobile.ViewModels.Doctor.Popups;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Doctor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoctorFiltersPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly DoctorFiltersViewModel _viewModel;

        public DoctorFiltersPopup(DoctorFilterOptions options, DoctorFilter preSelectedFilters, TaskCompletionSource<DoctorFilter> completionSource)
        {
            InitializeComponent();

            _viewModel = new DoctorFiltersViewModel(options, preSelectedFilters, completionSource);

            BindingContext = _viewModel;
        }
    }
}