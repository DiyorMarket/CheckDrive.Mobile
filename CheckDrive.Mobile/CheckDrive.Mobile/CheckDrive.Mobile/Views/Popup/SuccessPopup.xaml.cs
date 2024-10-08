using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public string Message { get; }

        public SuccessPopup()
        {

        }

        public SuccessPopup(string message)
        {
            InitializeComponent();

            Message = message;

            BindingContext = this;
        }

        private async void Close_Clicked(object sender, System.EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }
    }
}