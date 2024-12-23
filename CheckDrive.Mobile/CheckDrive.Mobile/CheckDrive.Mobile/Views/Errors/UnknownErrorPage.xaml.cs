using System;
using Xamarin.CommunityToolkit.UI.Views;
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

        public UnknownErrorPage(string message)
        {
            ErrorDetails.Text = message;
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }

        private void Expander_Tapped(object sender, System.EventArgs e)
        {
            if (sender is Expander expander)
            {
                if (expander.IsExpanded)
                {
                    ExpanderIcon.Source = "arrow_up.png";
                }
                else
                {
                    ExpanderIcon.Source = "arrow_down.png";
                }
            }
        }
    }
}