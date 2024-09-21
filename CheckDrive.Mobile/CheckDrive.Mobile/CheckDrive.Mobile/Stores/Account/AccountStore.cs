using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

            await LocalStorage.SaveAsync(token, LocalStorageKey.Token);
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

            var accountId = ExtractAccountIdFromToken(token);
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
            var accountId = ExtractAccountIdFromToken(token);
            var driver = await FetchDriverDataAsync(accountId);

            if (driver != null)
            {
                await LocalStorage.SaveAsync(driver, LocalStorageKey.Account);
            }
        }

        private static int ExtractAccountIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Cannot extract id from empty string.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            return int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        private async Task<AccountDto> FetchAccountAsync(int accountId)
        {
            var response = await _client.GetAsync($"accounts/{accountId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);

            return result;
        }

        private async Task<AccountDto> FetchDriverDataAsync(int accountId)
        {
            var query = $"drivers?accountId={accountId}";
            var response = await _client.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AccountDto>(json);

            return result;
        }
    }
}
