using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    internal class MockAccountStore : IAccountStore
    {
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

        public Task LoginAsync(string login, string password)
        {
            return Task.CompletedTask;
        }

        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }
    }
}
