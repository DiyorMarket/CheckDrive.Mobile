using CheckDrive.Mobile.ViewModels.Operator;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperatorHistoryPage : ContentPage
    {
        private readonly OperatorHistoryViewModel _viewModel;

        public OperatorHistoryPage()
        {
            InitializeComponent();

            _viewModel = new OperatorHistoryViewModel();

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.LoadHistoryAsync();

            base.OnAppearing();
        }
    }
}