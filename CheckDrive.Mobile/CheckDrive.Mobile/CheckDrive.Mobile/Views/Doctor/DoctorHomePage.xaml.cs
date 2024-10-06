using CheckDrive.Mobile.ViewModels.Doctor;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Doctor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DoctorHomePage : ContentPage
    {
        private readonly DoctorHomeViewModel _viewModel;

        public DoctorHomePage()
        {
            InitializeComponent();

            _viewModel = new DoctorHomeViewModel();

            BindingContext = _viewModel;
        }
    }
}