using CheckDrive.Mobile.ViewModels.Dispatcher;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Dispatcher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DispatcherHomePage : ContentPage
    {
        private readonly DispatcherHomeViewModel _viewModel;

        public DispatcherHomePage()
        {
            InitializeComponent();

            _viewModel = new DispatcherHomeViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadCheckPointsAsync();
            base.OnAppearing();
        }
    }
}