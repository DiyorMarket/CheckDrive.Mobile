using CheckDrive.Mobile.Views;
using CheckDrive.Mobile.Views.Doctor;
using CheckDrive.Mobile.Views.Mechanic;
using CheckDrive.Mobile.Views.Operator;
using Xamarin.Forms;

namespace CheckDrive.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell(string role)
        {
            InitializeComponent();

            // RegiserDefaultRoutes();
            RegisterRoutes(role);
        }

        public void RegisterRoutes(string role)
        {
            // Need to reset if user logged out and wants to login again.
            HomePage.ContentTemplate = null;
            HomePage.Content = null;
            HistoryPage.ContentTemplate = null;
            HistoryPage.Content = null;

            switch (role)
            {
                case "doctor":
                    HomePage.ContentTemplate = new DataTemplate(typeof(DoctorHomePage));
                    HistoryPage.ContentTemplate = new DataTemplate(typeof(DoctorHistoryPage));
                    break;
                case "operator":
                    HomePage.ContentTemplate = new DataTemplate(typeof(OperatorHomePage));
                    HistoryPage.ContentTemplate = new DataTemplate(typeof(OperatorHistoryPage));
                    break;
                case "mechanic":
                    HomePage.ContentTemplate = new DataTemplate(typeof(MechanicHomePage));
                    break;
                case "driver":
                    HomePage.ContentTemplate = new DataTemplate(typeof(HomePage));
                    HistoryPage.ContentTemplate = new DataTemplate(typeof(HistoryPage));
                    break;
                default:
                    return;
            }
        }
    }
}
