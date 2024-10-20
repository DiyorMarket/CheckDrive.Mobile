using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using CheckDrive.Mobile.ViewModels.Driver;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        private readonly HistoryViewModel _viewModel;

        public HistoryPage()
        {
            InitializeComponent();
            _viewModel = new HistoryViewModel();

            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            _ = Task.Run(async () => await _viewModel.LoadHistories());

            base.OnAppearing();
        }
    }
}