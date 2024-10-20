using CheckDrive.Mobile.ViewModels.Mechanic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicAcceptanceView : ContentView
    {
        private readonly MechanicAcceptanceViewModel _viewModel;

        public MechanicAcceptanceView()
        {
            InitializeComponent();

            _viewModel = new MechanicAcceptanceViewModel();

            BindingContext = _viewModel;
        }

        public Task LoadCheckPointsAsync()
        {
            return _viewModel.LoadDriversAsync();
        }
    }
}