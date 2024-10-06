using CheckDrive.Mobile.Exceptions;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheckDrive.Mobile.Stores.Account
{
    public class AccountStore : IAccountStore
    {
        private readonly ApiClient _client;

        public AccountStore()
        {
            _client = DependencyService.Get<ApiClient>();
        }

        public async Task LoginAsync(string login, string password)
        {
            var token = await AuthenticateUserAsync(login, password);

            await ProcessLoginAsync(token);
        }

        public Task LogoutAsync()
        {
            LocalStorage.RemoveAsync(LocalStorageKey.Token);
            LocalStorage.RemoveAsync(LocalStorageKey.Account);

            return Task.CompletedTask;
        }

        public async Task<AccountDto> GetAccountAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);

            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var accountId = JwtHelper.ExtractAccountIdFromToken(token);
            var account = await FetchAccountAsync(accountId);

            return account;
        }

        private async Task<string> AuthenticateUserAsync(string login, string password)
        {
            var request = new { login, password };
            var json = JsonConvert.SerializeObject(request);

            var response = await _client.PostAsync("login/login", json);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<string>(responseBody);
        }

        private async Task ProcessLoginAsync(string token)
        {
            VerifyAccountRole(token);

            await LocalStorage.SaveAsync(token, LocalStorageKey.Token);

            var accountId = JwtHelper.ExtractAccountIdFromToken(token);
            var driver = await FetchAccountAsync(accountId);

            if (driver != null)
            {
                await LocalStorage.SaveAsync(driver, LocalStorageKey.Account);
            }
        }

        private static void VerifyAccountRole(string token)
        {
            var role = JwtHelper.ExtractRoleFromToken(token);

            if (!role.Equals("driver", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidAccountException("User account must driver.");
            }
        }

        private async Task<AccountDto> FetchAccountAsync(string accountId)
        {
            var response = await _client.GetAsync($"accounts/{accountId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);

            return result;
        }
    }
}
