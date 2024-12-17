using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
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

                _hubConnection.On<int>("CheckPointProgressUpdated", checkPointId =>
                {
                    MessagingCenter.Send(this, "CheckPointProgressUpdated");
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
