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
            await Task.Run(async () => await AcceptanceView.InitializeAsync());

            base.OnAppearing();
        }
    }
}