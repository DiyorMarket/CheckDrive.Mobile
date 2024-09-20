using CheckDrive.ApiContracts.Driver;
using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Responses;
using CheckDrive.Mobile.Services;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Accounts
{
    public class AccountStore : IAccountStore
    {
        private readonly ApiClient _client;

        public AccountStore(ApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task LoginAsync(string login, string password)
        {
            var token = await AuthenticateUserAsync(login, password);

            await LocalStorage.SaveAsync(token, LocalStorageKey.Token);
            await ProcessLoginAsync(token, password);
        }

        public Task LogoutAsync()
        {
            LocalStorage.RemoveAsync(LocalStorageKey.Token);
            LocalStorage.RemoveAsync(LocalStorageKey.Account);

            return Task.CompletedTask;
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

        private async Task ProcessLoginAsync(string token, string password)
        {
            var accountId = ExtractAccountIdFromToken(token);
            var driver = await FetchDriverDataAsync(accountId);

            if (driver != null)
            {
                driver.Password = password;
                await LocalStorage.SaveAsync(driver, LocalStorageKey.Account);
            }
        }

        private static int ExtractAccountIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            return int.Parse(jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        private async Task<DriverDto> FetchDriverDataAsync(int accountId)
        {
            var query = $"drivers?accountId={accountId}";
            var response = await _client.GetAsync(query);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetDriverResponse>(json);

            return result.Data.FirstOrDefault();
        }
    }
}
