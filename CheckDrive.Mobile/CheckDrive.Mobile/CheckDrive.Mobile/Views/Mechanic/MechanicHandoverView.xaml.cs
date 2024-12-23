using CheckDrive.Mobile.ViewModels.Mechanic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicHandoverView : ContentView
    {
        private readonly MechanicHandoverViewModel _viewModel;

        public MechanicHandoverView()
        {
            InitializeComponent();

            _viewModel = new MechanicHandoverViewModel();

            BindingContext = _viewModel;
        }

        public async Task LoadViewModelData()
        {
            await _viewModel.InitializeAsync();
            await _viewModel.LoadDriversAsync();
        }
    }
}