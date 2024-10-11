using CheckDrive.Mobile.Helpers;
using CheckDrive.Mobile.Models;
using System.Threading.Tasks;

namespace CheckDrive.Mobile.Stores.Account
{
    internal class MockAccountStore : IAccountStore
    {
        private AccountDto _account;

        public MockAccountStore()
        {
            _account = FakeDataGenerator.GetAccount();
        }

        public Task<AccountDto> GetAccountAsync()
        {
            return Task.FromResult(_account);
        }

        public Task<AccountDto> UpdateAccountAsync(AccountDto account)
        {
            _account = account;

            return Task.FromResult(_account);
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
