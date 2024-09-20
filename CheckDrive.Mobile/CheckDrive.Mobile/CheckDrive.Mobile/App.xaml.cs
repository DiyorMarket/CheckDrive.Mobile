using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Stores.Accounts;
using CheckDrive.Mobile.Stores.Drivers;
using CheckDrive.Mobile.Views;
using CheckDrive.Mobile.Views.Errors;
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

            InitializeErrorHandlers();

            ConfigureServices();

            MainPage = new AppShell();
        }

        private void InitializeErrorHandlers()
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

        private static void ConfigureServices()
        {
            DependencyService.Register<ApiClient>();

            DependencyService.Register<IAccountStore, AccountStore>();
            DependencyService.Register<IDriverDataStore, DriverDataStore>();

            DependencyService.Register<INavigationService, NavigationService>();
        }

        private async Task InitializeAppAsync()
        {
            if (!await IsLoggedInAsync())
            {
                // Resolve the LoginPage from the DI container and navigate
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            else
            {
                // Resolve the DashboardPage from the DI container and navigate
                await Shell.Current.GoToAsync(nameof(ProfilePage));
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
            Console.WriteLine($"Unknown error occured: {ex.Message}.");
            Device.BeginInvokeOnMainThread(() =>
            {
                Current.MainPage = new UnknownErrorPage();
            });
        }
    }
}
