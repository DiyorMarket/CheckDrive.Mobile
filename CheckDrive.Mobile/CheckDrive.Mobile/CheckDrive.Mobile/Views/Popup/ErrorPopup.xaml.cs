using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ErrorPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public string Message { get; set; }
        public string Details { get; set; }

        public ErrorPopup(string message, string details)
        {
            InitializeComponent();

            Message = message;
            Details = details;

            BindingContext = this;
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

        private async void Close_Clicked(object sender, System.EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}