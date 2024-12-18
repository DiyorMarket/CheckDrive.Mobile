using CheckDrive.Mobile.Models.Operator;
using CheckDrive.Mobile.ViewModels.Operator.Popups;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Operator.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperatorFiltersPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly OperatorFiltersViewModel _viewModel;

        public OperatorFiltersPopup(
            OperatorFilterOptions options,
            OperatorFilter currentFilters,
            TaskCompletionSource<OperatorFilter> completionSource)
        {
            InitializeComponent();

            HasKeyboardOffset = false;
            HasSystemPadding = false;

            _viewModel = new OperatorFiltersViewModel(options, currentFilters, completionSource);

            BindingContext = _viewModel;
        }
    }
}