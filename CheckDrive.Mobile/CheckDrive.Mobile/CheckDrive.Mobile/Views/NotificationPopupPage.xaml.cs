using CheckDrive.Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPopupPage : PopupPage
    {
        private readonly NotificationViewModel _chatViewModel;
        public NotificationPopupPage(string massage)
        {
            InitializeComponent();

            _chatViewModel = new NotificationViewModel(massage);
            
            BindingContext = _chatViewModel;
        }

        private async void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}