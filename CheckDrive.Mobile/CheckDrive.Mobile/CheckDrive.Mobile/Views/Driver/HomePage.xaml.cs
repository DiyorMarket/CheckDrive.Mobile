using CheckDrive.Mobile.ViewModels.Driver;
using System.Threading.Tasks;
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
            var initializeTask = _viewModel.InitializeAsync();
            var loadTask = _viewModel.OnRefreshAsync();
            await Task.WhenAll(initializeTask, loadTask);
            base.OnAppearing();
        }
    }
}