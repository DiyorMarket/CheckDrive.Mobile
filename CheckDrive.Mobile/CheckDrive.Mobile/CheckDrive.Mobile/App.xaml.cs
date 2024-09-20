using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckDrive.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        protected override async void OnStart()
        {
            try
            {
                await InitializeAppAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Startup error: {ex.Message}.");
            }
        }

        private async Task InitializeAppAsync()
        {
            if (!ConnectivityService.IsConnected())
            {
                Device.BeginInvokeOnMainThread(() => MainPage = new NoInternetPage());
                return;
            }

            if (!await IsLoggedInAsync())
            {
                Device.BeginInvokeOnMainThread(() => MainPage = new LoginPage());
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => MainPage = new AppShell());
            }
        }

        private static async Task<bool> IsLoggedInAsync()
        {
            var token = await LocalStorage.GetAsync<string>(Models.Enums.LocalStorageKey.Token);

            if (string.IsNullOrEmpty(token) || JwtHelper.IsTokenExpired(token))
            {
                return false;
            }

            return true;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet && !(MainPage is NoInternetPage))
            {
                var signalRService = new SignalRService();
                await signalRService.StopConnectionAsync();

                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }

                MainPage = new NoInternetPage();
            }
            else if (e.NetworkAccess == NetworkAccess.Internet && !(MainPage is AppShell))
            {
                MainPage = new AppShell();
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowExceptionPage(e.ExceptionObject as Exception);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ShowExceptionPage(e.Exception);
        }

        public void ShowExceptionPage(Exception ex)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MainPage = new ExceptionPage();
            });
        }
    }
}
