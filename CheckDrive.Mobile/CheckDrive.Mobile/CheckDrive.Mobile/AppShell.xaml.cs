using CheckDrive.Mobile.Views;
using System;
using Xamarin.Forms;

namespace CheckDrive.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
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

        private static void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        }
    }
}
