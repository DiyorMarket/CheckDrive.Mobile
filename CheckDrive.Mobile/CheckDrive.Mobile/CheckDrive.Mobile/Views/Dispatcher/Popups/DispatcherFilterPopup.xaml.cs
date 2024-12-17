using CheckDrive.Mobile.Models.Dispatcher;
using CheckDrive.Mobile.ViewModels.Dispatcher.Popups;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Dispatcher.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DispatcherFilterPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly DispatcherFiltersViewModel _viewModel;

        public DispatcherFilterPopup(
            DispatcherFilterOptions options,
            DispatcherFilter preSelectedFilter,
            TaskCompletionSource<DispatcherFilter> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new DispatcherFiltersViewModel(options, preSelectedFilter, completionSource);

            BindingContext = _viewModel;
        }
    }
}