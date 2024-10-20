using CheckDrive.Mobile.ViewModels.Mechanic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicHistoryPage : ContentPage
    {
        private readonly MechanicHistoryViewModel _viewModel;

        public MechanicHistoryPage()
        {
            InitializeComponent();

            _viewModel = new MechanicHistoryViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadDataAsync();

            base.OnAppearing();
        }
    }
}