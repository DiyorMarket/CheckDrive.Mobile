using CheckDrive.Mobile.ViewModels.Operator;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Operator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OperatorHomePage : ContentPage
    {
        private readonly OperatorHomeViewModel _viewModel;
        public OperatorHomePage()
        {
            InitializeComponent();

            _viewModel = new OperatorHomeViewModel();

            BindingContext = _viewModel;
        }
    }
}