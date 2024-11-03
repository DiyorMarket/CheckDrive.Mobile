using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Models.Review;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Services
{
    public class SignalRService
    {
        private HubConnection _hubConnection;

        public async Task StartConnectionAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);

            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("https://4hq2t8p1-7111.euw.devtunnels.ms/review-hub", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token);
                    })
                    .Build();

                _hubConnection.On<ReviewDto>("NotifyDoctorReview", request =>
                {
                    MessagingCenter.Send(this, "NotifyDoctorReview", request);
                });

                _hubConnection.On<MechanicHandoverReview>("MechanicHandoverConfirmation", request =>
                {
                    MessagingCenter.Send(this, "MechanicHandoverConfirmation", request);
                });

                _hubConnection.On<OperatorReview>("OperatorReviewConfirmation", request =>
                {
                    MessagingCenter.Send(this, "OperatorReviewConfirmation", request);
                });

                _hubConnection.On<MechanicAcceptanceReview>("MechanicAcceptanceConfirmation", request =>
                {
                    MessagingCenter.Send(this, "MechanicAcceptanceConfirmation", request);
                });

                _hubConnection.On<ReviewDto>("NotifyDispatcherReview", request =>
                {
                    MessagingCenter.Send(this, "NotifyDispatcherReview", request);
                });

                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                // TODO change to error popup
                Console.WriteLine($"Error connecting to SignalR: {ex.Message}");
            }
        }

        public async Task StopConnectionAsync()
        {
            try
            {
                await _hubConnection.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disconnecting to SignalR: {ex.Message}");
            }
        }
    }
}
