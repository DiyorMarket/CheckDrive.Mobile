using CheckDrive.Mobile.Exceptions;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Stores.Account;
using CheckDrive.Mobile.Stores.History;
using CheckDrive.Mobile.Stores.Review;
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

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            MainPage = new AppShell();

            //var signalRService = new SignalRService();
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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ4NzEyNUAzMjM1MmUzMDJlMzBKTkNydWx1REpPMnYrMDJ3RnhmaFNOU1JIWEw5d2Z5by9OQXhvQThrRnFjPQ==");

            DependencyService.Register<ApiClient>();

            DependencyService.Register<IAccountStore, MockAccountStore>();
            DependencyService.Register<IReviewStore, MockReviewStore>();
            DependencyService.Register<IHistoryStore, MockHistoryStore>();

            DependencyService.Register<INavigationService, NavigationService>();
        }

        private static async Task InitializeAppAsync()
        {
            if (!await IsLoggedInAsync())
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            else
            {
                await Shell.Current.GoToAsync("//HomePage");
            }
        }

        private static async Task<bool> IsLoggedInAsync()
        {
            var token = await LocalStorage.GetAsync<string>(Models.Enums.LocalStorageKey.Token);

            if (string.IsNullOrWhiteSpace(token) || JwtHelper.IsTokenExpired(token))
            {
                return true;
            }

            return true;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess != NetworkAccess.Internet && !(MainPage is NoInternetErrorPage))
            {
                var signalRService = new SignalRService();
                await signalRService.StopConnectionAsync();

                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await PopupNavigation.Instance.PopAllAsync();
                }

                MainPage = new NoInternetErrorPage();
            }
            else if (e.NetworkAccess == NetworkAccess.Internet && !(MainPage is AppShell))
            {
                MainPage = new AppShell();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            ShowExceptionPage(e.ExceptionObject as Exception);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            ShowExceptionPage(e.Exception);
        }

        public static void ShowExceptionPage(Exception ex)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Console.WriteLine($"Unknown error occurred: {ex.Message}");

                if (ex is InvalidTokenException)
                {
                    await Shell.Current.GoToAsync(nameof(LoginPage), true);
                }

                await Shell.Current.GoToAsync(nameof(UnknownErrorPage), true);
            });
        }
    }
}
