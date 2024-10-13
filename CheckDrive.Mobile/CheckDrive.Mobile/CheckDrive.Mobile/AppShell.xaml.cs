using CheckDrive.Mobile.Views;
using CheckDrive.Mobile.Views.Doctor;
using CheckDrive.Mobile.Views.Mechanic;
using System;
using Xamarin.Forms;

namespace CheckDrive.Mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            // await Shell.Current.GoToAsync("//LoginPage");
        }

        private void RegisterRoutes()
        {
            var role = "doctor";

            switch (role)
            {
                case "doctor":
                    HomePage.Content = new MechanicHomePage();
                    HistoryPage.Content = new DoctorHistoryPage();
                    break;
                case "driver":
                    HomePage.ContentTemplate = new DataTemplate(typeof(HomePage));
                    HistoryPage.ContentTemplate = new DataTemplate(typeof(HistoryPage));
                    break;
                default:
                    return;
            }
            // Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            //Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            //Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
        }
    }
}
