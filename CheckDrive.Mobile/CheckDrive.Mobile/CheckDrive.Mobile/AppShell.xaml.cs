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
        }

        protected override void OnAppearing()
        {
            Shell.Current.Navigation.PushAsync(new RoadMapPage());
            base.OnAppearing();
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }
}
