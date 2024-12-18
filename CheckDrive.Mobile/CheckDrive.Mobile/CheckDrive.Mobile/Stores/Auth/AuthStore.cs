using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Account;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Auth
{
    public sealed class AuthStore : IAuthStore
    {
        private readonly ApiClient _client;

        public AuthStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var request = new LoginRequest(username, password);

            var response = await _client.PostAsync("auth/login", request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<string>(json);

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Authentication token cannot be empty.");
            }

            await LocalStorage.SaveAsync(token, LocalStorageKey.Token);

            return true;
        }

        public void Logout()
        {
            LocalStorage.ClearAll();
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            return JwtHelper.IsTokenValid(token);
        }
    }
}
