using CheckDrive.Mobile.ViewModels.Doctor;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Doctor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoctorHistoryPage : ContentPage
    {
        private readonly DoctorHistoryViewModel _viewModel;

        public DoctorHistoryPage()
        {
            InitializeComponent();

            _viewModel = new DoctorHistoryViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadHistoryAsync();

            base.OnAppearing();
        }
    }
}