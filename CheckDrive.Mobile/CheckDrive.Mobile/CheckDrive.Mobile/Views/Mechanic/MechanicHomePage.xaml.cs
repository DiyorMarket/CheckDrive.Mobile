using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Mechanic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MechanicHomePage : ContentPage
    {
        public MechanicHomePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await HandoverView.LoadViewModelData();
            Task.Run(() => AcceptanceView.LoadCheckPointsAsync());

            base.OnAppearing();
        }
    }
}