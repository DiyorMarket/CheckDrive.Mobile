using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Services.Navigation;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Services
{
    public class SignalRService
    {
        private readonly INavigationService _navigationService; 
        public ICommand SendResponseCommand { get; set; }

        private HubConnection _hubConnection;

        public SignalRService()
        {
            _navigationService = DependencyService.Get<INavigationService>();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://miraziz-001-site1.ctempurl.com/api/chat", options =>
                {
                    options.AccessTokenProvider = async () => await GetTokenAsync();
                })
            .Build();

            _hubConnection.On<int, int, string>("ReceiveMessage", async (status, reviewId, message) =>
            {
                LocalStorage.SaveSignalRDataFOrStatus(status);
                LocalStorage.SaveSignalRDataForReviewID(reviewId);
                await ShowPopupAsync(message);
            });
        }

        public async Task SendResponse(bool isAccepted)
        {
            var signalRData = LocalStorage.GetSignalRData();
            var status = signalRData.statusNumber;
            int reviewId = signalRData.reviewId;
            try
            {
                await Task.Delay(2000);
                await _hubConnection.SendAsync("ReceivePrivateResponse", status, reviewId, isAccepted);
                LocalStorage.RemoveSignalRData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await _hubConnection.StartAsync();
                Console.WriteLine("Соединение установлено.");
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

        private async Task ShowPopupAsync(string message)
        {
            try
            {
                await _navigationService.NavigateToAsync(Models.Enums.NavigationPageType.NotificationPopup, message);

                Console.WriteLine("Popup ochildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Popup ochishda xatolik: {ex.Message}");
            }
        }

        private async Task<string> GetTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync("tasty-cookies");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении токена: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
