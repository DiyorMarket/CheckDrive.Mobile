using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models.Account;
using CheckDrive.Mobile.Models.Enums;
using CheckDrive.Mobile.Services;
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

        public async Task<AccountDto> GetAccountAsync(string accountId)
        {
            var account = await _client.GetAsync<AccountDto>($"accounts/{accountId}");

            await LocalStorage.SaveAsync(account, LocalStorageKey.Account);

            return account;
        }

        public async Task<AccountDto> UpdateAccountAsync(AccountDto account)
        {
            var updatedAccount = await _client.PutAsync<AccountDto, AccountDto>($"accounts/{account.AccountId}", account);

            await LocalStorage.SaveAsync(updatedAccount, LocalStorageKey.Account);

            return updatedAccount;
        }

        public async Task<string> GetAccountIdAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);
            var accountId = JwtHelper.GetAccountId(token);

            return accountId;
        }

        public async Task<int> GetUserIdAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);
            var userId = JwtHelper.GetUserId(token);

            return int.Parse(userId);
        }

        public async Task<string> GetUserRoleAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);
            var userRole = JwtHelper.GetUserRole(token);

            return userRole;
        }
    }
}
