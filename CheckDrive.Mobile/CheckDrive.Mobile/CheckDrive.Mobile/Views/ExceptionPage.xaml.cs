using CheckDrive.Mobile.Helpers;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExceptionPage : ContentPage
    {
        public ExceptionPage()
        {
            InitializeComponent();
        }

        private async void OnRetryClicked(object sender, EventArgs e)
        {
            await Task.Run(() => LocalStorage.RemoveAllAcoountData());
            Application.Current.MainPage = new LoginPage();
        }
    }
}