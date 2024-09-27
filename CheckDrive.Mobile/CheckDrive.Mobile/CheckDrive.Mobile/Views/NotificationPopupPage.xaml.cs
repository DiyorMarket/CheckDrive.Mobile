using CheckDrive.Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificationPopupPage : PopupPage
    {
        private readonly ChatViewModel _chatViewModel;
        public NotificationPopupPage(string massage)
        {
            InitializeComponent();

            _chatViewModel = new ChatViewModel();
            _chatViewModel.Message = massage; 
            
            BindingContext = _chatViewModel;
        }

        private async void PopupPage_BackgroundClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}