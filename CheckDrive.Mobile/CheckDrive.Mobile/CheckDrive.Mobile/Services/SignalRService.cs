using CheckDrive.Mobile.Models.Driver;
using CheckDrive.Mobile.ViewModels.Driver.Popups;
using CheckDrive.Mobile.Views.Driver.Popups;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CheckDrive.Mobile.Services
{
    public class SignalRService
    {
        private HubConnection _hubConnection;

        public async Task StartConnectionAsync(string token)
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("url", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token);
                    })
                    .Build();

                _hubConnection.On<ReviewConfirmationRequest>("ReviewConfirmation", async (request) =>
                {
                    var key = GetKey(request);
                    var json = JsonConvert.SerializeObject(request);
                    await SecureStorage.SetAsync(key, json);

                    await HandleAsync(request);
                });

                await _hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task StopConnectionAsync()
        {
            try
            {
                await _hubConnection.StopAsync();
                Console.WriteLine("Соединение закрыто.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private static async Task HandleAsync(ReviewConfirmationRequest request)
        {
            var completionSource = new TaskCompletionSource<bool>();
            var popup = new ReviewConfirmationPopup()
            {
                BindingContext = new ReviewConfirmationViewModel(request, completionSource)
            };

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(popup);

            var isHandled = await completionSource.Task;

            if (isHandled)
            {
                var key = GetKey(request);
                SecureStorage.Remove(key);
            }
        }

        private static string GetKey(ReviewConfirmationRequest request)
            => $"{nameof(ReviewConfirmationRequest)}-{request.CheckPointId}";
    }
}
