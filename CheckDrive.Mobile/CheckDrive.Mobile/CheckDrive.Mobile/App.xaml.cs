using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Views;
using System;
using Xamarin.Forms;

namespace CheckDrive.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzI3MDgyNEAzMjM1MmUzMDJlMzBIU2RvbkFWNUp2R0FwNDBnYi9yUFFROExGcGVmc0c3NU56bDBhaU85SGZnPQ==");

            var isChecked = CheckloginDate();

            if (isChecked)
            {
                MainPage = new AppShell();
                return;
            }

            MainPage = new LoginPage();
        }

        private bool CheckloginDate()
        {
            var creationDate = DataService.GetCreationDate();
            var driver = DataService.GetAccount();

            if (creationDate != null && driver != null && DateTime.Now.Date.AddDays(-30) <= creationDate.Date)
            {
                return true;
            }

            DataService.RemoveAcoountData();

            return false;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
