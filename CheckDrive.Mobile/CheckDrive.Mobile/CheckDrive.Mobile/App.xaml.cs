using CheckDrive.Mobile.Exceptions;
using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Services.Navigation;
using CheckDrive.Mobile.Stores.Account;
using CheckDrive.Mobile.Stores.Auth;
using CheckDrive.Mobile.Stores.Car;
using CheckDrive.Mobile.Stores.CheckPoint;
using CheckDrive.Mobile.Stores.Dispatcher;
using CheckDrive.Mobile.Stores.Doctor;
using CheckDrive.Mobile.Stores.Driver;
using CheckDrive.Mobile.Stores.History;
using CheckDrive.Mobile.Stores.Mechanic;
using CheckDrive.Mobile.Stores.Operator;
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
        }

        protected override async void OnStart()
        {
            try
            {
                var authStore = DependencyService.Get<IAuthStore>();
                var isLoggedIn = await authStore.IsLoggedInAsync();

                if (isLoggedIn)
                {
                    var accountStore = DependencyService.Get<IAccountStore>();
                    var role = await accountStore.GetUserRoleAsync();

                    MainPage = new AppShell(role);

                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    MainPage = new LoginPage();
                }
            }
            catch (Exception ex)
            {
                MainPage = new LoginPage();
                Console.WriteLine($"Startup error: {ex.Message}.");
            }
        }

        private void InitializeErrorHandlers()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private static void ConfigureServices()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzQ4NzEyNUAzMjM1MmUzMDJlMzBKTkNydWx1REpPMnYrMDJ3RnhmaFNOU1JIWEw5d2Z5by9OQXhvQThrRnFjPQ==");

            DependencyService.Register<ApiClient>();
            DependencyService.Register<SignalRService>();

            DependencyService.Register<IAuthStore, AuthStore>();
            DependencyService.Register<IAccountStore, AccountStore>();
            DependencyService.Register<IReviewStore, MockReviewStore>();
            DependencyService.Register<IHistoryStore, MockHistoryStore>();
            DependencyService.Register<IDoctorStore, DoctorStore>();
            DependencyService.Register<IMechanicStore, MechanicStore>();
            DependencyService.Register<ICheckPointStore, CheckPointStore>();
            DependencyService.Register<ICarStore, CarStore>();
            DependencyService.Register<IDriverStore, DriverStore>();
            DependencyService.Register<IOperatorStore, OperatorStore>();
            DependencyService.Register<IDispatcherStore, DispatcherStore>();

            DependencyService.Register<INavigationService, NavigationService>();
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
                // MainPage = new AppShell();
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
                if (ex is InvalidTokenException)
                {
                    await Shell.Current.GoToAsync(nameof(LoginPage), true);
                }

                await Shell.Current.Navigation.PushAsync(new UnknownErrorPage(ex.Message), true);
            });
        }
    }
}
