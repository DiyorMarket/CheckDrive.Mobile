using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using CheckDrive.Mobile.Models.Enums;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    internal class MockAccountStore : IAccountStore
    {
        private string login = string.Empty;

        public Task<AccountDto> GetAccountAsync()
        {
            var account = FakeDataGenerator.GetAccount();

            return Task.FromResult(account);
        }

        public Task<int> GetEmployeeIdAsync()
        {
            var id = new Random().Next(100);
            return Task.FromResult(id);
        }

        public Task<string> GetUserRoleAsync()
        {
            return Task.FromResult(login);
        }

        public async Task<bool> IsLoggedInAsync()
        {
            var token = await LocalStorage.GetAsync<string>(LocalStorageKey.Token);
            this.login = token;

            return !string.IsNullOrEmpty(token);
        }

        public async Task LoginAsync(string login, string password)
        {
            this.login = login;
            await LocalStorage.SaveAsync(login, LocalStorageKey.Token);
        }

        public Task LogoutAsync()
        {
            LocalStorage.ClearAll();
            return Task.CompletedTask;
        }
    }
}
