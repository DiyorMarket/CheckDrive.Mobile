using CheckDrive.Mobile.ViewModels.Dispatcher;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Dispatcher
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DispatcherHistoryPage : ContentPage
    {
        private readonly DispatcherHistoryViewModel _viewModel;

        public DispatcherHistoryPage()
        {
            InitializeComponent();

            _viewModel = new DispatcherHistoryViewModel();

            BindingContext = _viewModel;
        }
    }
}