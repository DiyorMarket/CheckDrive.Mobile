using CheckDrive.Mobile.Services;
using CheckDrive.Mobile.Stores.DoctorReviews;
using CheckDrive.Mobile.Stores.MechanicAcceptances;
using CheckDrive.Mobile.Stores.MechanicHandovers;
using CheckDrive.Mobile.Stores.OperatorReviews;
using CheckDrive.Mobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheckDrive.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoadMapPage : ContentPage
    {
        private RoadMapViewModel viewModel;
        public RoadMapPage()
        {
            InitializeComponent();

            var client = new ApiClient();
            var doctorReviewDS = new DoctorReviewDataStore(client);
            var mechanicHandoverDS = new MechanicHandoverDataStore(client);
            var operatorReviewDS = new OperatorReviewDataStore(client);
            var mechanicAcceptanceDS = new MechanicAcceptanceDataStore(client);

            BindingContext = viewModel = new RoadMapViewModel(doctorReviewDS, mechanicAcceptanceDS, operatorReviewDS, mechanicHandoverDS);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadViewPage();
        }

        private async void ClickToShowPopup_Clicked(object sender, EventArgs e)
        {
            var popup = new CheckControlPopup()
            {
                BindingContext = BindingContext
            };

            await Navigation.PushModalAsync(popup);
        }

        private void RoadMapView_Refreshing(Object sender, EventArgs e)
        {
            var client = new ApiClient();
            var doctorReviewDS = new DoctorReviewDataStore(client);
            var mechanicHandoverDS = new MechanicHandoverDataStore(client);
            var operatorReviewDS = new OperatorReviewDataStore(client);
            var mechanicAcceptanceDS = new MechanicAcceptanceDataStore(client);

            BindingContext = viewModel = new RoadMapViewModel(doctorReviewDS, mechanicAcceptanceDS, operatorReviewDS, mechanicHandoverDS);

            RoadMapRefresh.IsRefreshing = false;
        }
    }
}
