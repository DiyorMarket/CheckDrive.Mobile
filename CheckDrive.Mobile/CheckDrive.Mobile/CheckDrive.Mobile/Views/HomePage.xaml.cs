using CheckDrive.Mobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private readonly HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            _viewModel = new HomeViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadData();
            base.OnAppearing();
        }
    }
}