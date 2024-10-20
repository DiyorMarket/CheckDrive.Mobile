using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Errors
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnknownErrorPage : ContentPage
    {
        public UnknownErrorPage()
        {
            InitializeComponent();
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}